using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Spectrum.Models;
using Spectrum.Repositories;

namespace Spectrum.Services;

public interface IJwtService
{
    Task<string> GenerateTokenAsync(User user);
}

public class JwtService : IJwtService
{
    private readonly IConfiguration _configuration;
    private readonly IRoleRepository _roleRepository;

    public JwtService(IConfiguration configuration, IRoleRepository roleRepository)
    {
        _configuration = configuration;
        _roleRepository = roleRepository;
    }

    public async Task<string> GenerateTokenAsync(User user)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
            _configuration["Jwt:Key"] ?? throw new InvalidOperationException("JWT Key not found")));
        
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        string roleName = string.Empty;
        if (user.U_RoleId.HasValue)
        {
            var role = await _roleRepository.GetByIdAsync(user.U_RoleId.Value);
            if (role != null)
            {
                roleName = role.RoleName;
            }
        }

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
            new Claim(ClaimTypes.Name, user.Username),
            new Claim("U_Name", user.U_Name),
            new Claim(ClaimTypes.Email, user.U_Email ?? string.Empty),
            new Claim("U_RoleId", user.U_RoleId?.ToString() ?? string.Empty),
            new Claim("U_DepartmentID", user.U_DepartmentID?.ToString() ?? string.Empty)
        };

        if (!string.IsNullOrWhiteSpace(roleName))
        {
            // standard role claim
            claims.Add(new Claim(ClaimTypes.Role, roleName));
            // custom claim with role name
            claims.Add(new Claim("U_RoleName", roleName));
        }

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(
                Convert.ToDouble(_configuration["Jwt:ExpiresMinutes"] ?? "1440")),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
