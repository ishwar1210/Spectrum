using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ClosedXML.Excel;
using Spectrum.DTOs;
using Spectrum.Models;
using Spectrum.Repositories;
using BCrypt.Net;
using Spectrum.Services;
using System.Security.Cryptography;

namespace Spectrum.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class UserImportController : ControllerBase
{
    private readonly IUserRepository _userRepository;
    private readonly IRoleRepository _roleRepository;
    private readonly IDepartmentRepository _departmentRepository;
    private readonly EmailService _emailService;
    private readonly ILogger<UserImportController> _logger;

    public UserImportController(
        IUserRepository userRepository,
        IRoleRepository roleRepository,
        IDepartmentRepository departmentRepository,
        EmailService emailService,
        ILogger<UserImportController> logger)
    {
        _userRepository = userRepository;
        _roleRepository = roleRepository;
        _departmentRepository = departmentRepository;
        _emailService = emailService;
        _logger = logger;
    }

    [HttpPost("upload")]
    public async Task<IActionResult> UploadExcel(IFormFile file)
    {
        if (file == null || file.Length == 0)
            return BadRequest(new { message = "Empty file" });

        if (!file.FileName.EndsWith(".xlsx", StringComparison.OrdinalIgnoreCase))
            return BadRequest(new { message = "Only .xlsx files are supported" });

        using var stream = file.OpenReadStream();
        using var workbook = new XLWorkbook(stream);
        var worksheet = workbook.Worksheets.First();

        var rows = worksheet.RangeUsed()?.RowsUsed().Skip(1); // skip header
        if (rows == null)
            return BadRequest(new { message = "No rows found" });

        var created = new List<int>();
        var emailsSent = 0;
        var emailsFailed = 0;

        foreach (var row in rows)
        {
            // Expected column order (adjust if your frontend uses different order):
            // 1: FullName, 2: Mobile, 3: Email, 4: RoleName, 5: DepartmentName, 6: ReportingToName, 7: Address
            var fullName = row.Cell(1).GetString().Trim();
            var mobile = row.Cell(2).GetString().Trim();
            var email = row.Cell(3).GetString().Trim();

            if (string.IsNullOrEmpty(fullName) || string.IsNullOrEmpty(mobile))
                continue; // skip invalid rows

            // optional fields (names -> will be resolved to IDs)
            var roleName = row.Cell(4).GetString().Trim();
            var departmentName = row.Cell(5).GetString().Trim();
            var reportingToName = row.Cell(6).GetString().Trim();
            var address = row.Cell(7).GetString().Trim();

            int? roleId = null;
            if (!string.IsNullOrEmpty(roleName))
            {
                var role = await _roleRepository.GetByNameAsync(roleName);
                if (role != null) roleId = role.RoleId;
            }

            int? departmentId = null;
            if (!string.IsNullOrEmpty(departmentName))
            {
                var dep = await _departmentRepository.GetByNameAsync(departmentName);
                if (dep != null) departmentId = dep.DepartmentId;
            }

            int? reportingToId = null;
            if (!string.IsNullOrEmpty(reportingToName))
            {
                var manager = await _userRepository.GetByNameAsync(reportingToName);
                if (manager != null) reportingToId = manager.UserId;
            }

            // generate base credential (initials + last 4 digits)
            var baseCredential = GenerateBaseCredential(fullName, mobile);
            var baseUsername = baseCredential.ToLowerInvariant();

            // ensure username length <= 100 and make unique
            var username = await MakeUniqueUsernameAsync(baseUsername);

            // ensure username does not exceed DB length
            if (username.Length > 100)
                username = username[..100];

            // enforce DB column lengths by truncating inputs if necessary
            if (mobile.Length > 20) mobile = mobile[..20];
            if (email.Length > 200) email = email[..200];
            if (address.Length > 400) address = address[..400];

            var plainPassword = baseCredential; // base password; will be emailed
            var passwordHash = BCrypt.Net.BCrypt.HashPassword(plainPassword);

            var user = new User
            {
                Username = username,
                Password_hash = passwordHash,
                U_Name = fullName.Length > 200 ? fullName[..200] : fullName,
                U_Mobile = string.IsNullOrEmpty(mobile) ? null : mobile,
                U_Email = string.IsNullOrEmpty(email) ? null : email,
                U_Address = string.IsNullOrEmpty(address) ? null : address,
                U_RoleId = roleId,
                U_DepartmentID = departmentId,
                U_ReportingToId = reportingToId
            };

            var id = await _userRepository.CreateAsync(user);
            if (id > 0)
            {
                created.Add(id);

                // send credentials email if email provided
                if (!string.IsNullOrEmpty(user.U_Email))
                {
                    _logger.LogInformation("Attempting to send email to {Email} for user {Username}", user.U_Email, user.Username);
                    var emailSent = await _emailService.SendUserCredentialsEmailAsync(user.U_Email!, user.Username, plainPassword);
                    if (emailSent)
                        emailsSent++;
                    else
                        emailsFailed++;
                }
            }
        }

        return Ok(new
        {
            message = "Import completed",
            createdCount = created.Count,
            emailsSent,
            emailsFailed
        });
    }

    private static int? TryGetInt(string s)
    {
        if (int.TryParse(s, out var v)) return v;
        return null;
    }

    private static string GenerateBaseCredential(string fullName, string mobile)
    {
        // Use full first name + a random 4-digit slice from mobile digits (e.g., "nisha6540")
        var parts = fullName.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        var firstName = parts.Length > 0 ? parts[0] : fullName;
        // normalize: remove non-alphanumeric from first name
        var cleanFirst = new string(firstName.Where(char.IsLetterOrDigit).ToArray());
        if (string.IsNullOrEmpty(cleanFirst)) cleanFirst = "user";

        var digits = new string(mobile.Where(char.IsDigit).ToArray());
        string four;
        if (digits.Length >= 4)
        {
            // pick a random contiguous 4-digit substring from the mobile digits
            var maxStart = digits.Length - 4;
            var start = RandomNumberGenerator.GetInt32(0, maxStart + 1); // inclusive
            four = digits.Substring(start, 4);
        }
        else
        {
            // pad with zeros if not enough digits
            four = digits.PadLeft(4, '0');
        }

        return (cleanFirst + four).ToLowerInvariant();
    }

    private async Task<string> MakeUniqueUsernameAsync(string baseUsername)
    {
        var username = baseUsername;
        // truncate to 100 to start
        if (username.Length > 100) username = username[..100];

        if (!await _userRepository.UsernameExistsAsync(username))
            return username;

        var counter = 1;
        while (true)
        {
            var suffix = counter.ToString();
            var maxBaseLen = 100 - suffix.Length;
            var candidateBase = baseUsername.Length > maxBaseLen ? baseUsername[..maxBaseLen] : baseUsername;
            var candidate = candidateBase + suffix;
            if (!await _userRepository.UsernameExistsAsync(candidate))
                return candidate;
            counter++;
        }
    }
}
