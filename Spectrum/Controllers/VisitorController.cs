using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Spectrum.DTOs;
using Spectrum.Services;

namespace Spectrum.Controllers;

[ApiController]
[Route("api/[controller]")]
public class VisitorController : ControllerBase
{
    private readonly IVisitorService _service;
    private readonly IWebHostEnvironment _environment;
    private readonly EmailService _emailService;

    public VisitorController(IVisitorService service, IWebHostEnvironment environment, EmailService emailService)
    {
        _service = service;
        _environment = environment;
        _emailService = emailService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var (success, message, items) = await _service.GetAllAsync();
        if (!success) return BadRequest(new { message });
        return Ok(items);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var (success, message, item) = await _service.GetByIdAsync(id);
        if (!success) return NotFound(new { message });
        return Ok(item);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateVisitorDTO dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        var (success, message, id) = await _service.CreateAsync(dto);
        if (!success) return BadRequest(new { message });
        return CreatedAtAction(nameof(GetById), new { id }, new { message, id });
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateVisitorDTO dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        var (success, message, item) = await _service.UpdateAsync(id, dto);
        if (!success) return BadRequest(new { message });
        return Ok(new { message, item });
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var (success, message) = await _service.DeleteAsync(id);
        if (!success) return NotFound(new { message });
        return Ok(new { message });
    }

    [HttpPost("upload-image")]
    public async Task<IActionResult> UploadImage([FromForm] IFormFile file)
    {
        if (file == null || file.Length == 0)
            return BadRequest(new { message = "No file uploaded" });

        var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
        var extension = Path.GetExtension(file.FileName).ToLowerInvariant();

        if (!allowedExtensions.Contains(extension))
            return BadRequest(new { message = "Invalid file type. Only images are allowed." });

        if (file.Length > 5 * 1024 * 1024)
            return BadRequest(new { message = "File size must be less than 5MB" });

        try
        {
            var uploadsFolder = Path.Combine(_environment.WebRootPath ?? _environment.ContentRootPath, "uploads", "visitors");
            
            if (!Directory.Exists(uploadsFolder))
                Directory.CreateDirectory(uploadsFolder);

            var uniqueFileName = $"{Guid.NewGuid()}{extension}";
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }

            var imageUrl = $"/uploads/visitors/{uniqueFileName}";
            return Ok(new { message = "Image uploaded successfully", imagePath = imageUrl });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Error uploading file", error = ex.Message });
        }
    }

    [HttpGet("image/{fileName}")]
    [AllowAnonymous]
    public IActionResult GetImage(string fileName)
    {
        var uploadsFolder = Path.Combine(_environment.WebRootPath ?? _environment.ContentRootPath, "uploads", "visitors");
        var filePath = Path.Combine(uploadsFolder, fileName);

        if (!System.IO.File.Exists(filePath))
            return NotFound(new { message = "Image not found" });

        var extension = Path.GetExtension(fileName).ToLowerInvariant();
        var contentType = extension switch
        {
            ".jpg" or ".jpeg" => "image/jpeg",
            ".png" => "image/png",
            ".gif" => "image/gif",
            _ => "application/octet-stream"
        };

        return PhysicalFile(filePath, contentType);
    }

    [HttpPost("send-appointment-link")]
    [AllowAnonymous]
    public async Task<IActionResult> SendAppointmentLink([FromBody] SendAppointmentEmailDTO dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var sent = await _emailService.SendAppointmentEmailAsync(dto.Email, dto.Name);
        if (!sent)
            return StatusCode(500, new { message = "Failed to send appointment email. Check server email settings." });

        return Ok(new { message = "Appointment link sent. Please check your email." });
    }
}
