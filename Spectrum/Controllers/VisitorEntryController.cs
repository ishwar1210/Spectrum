using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Spectrum.DTOs;
using Spectrum.Services;

namespace Spectrum.Controllers;

[ApiController]
[Route("api/[controller]")]
public class VisitorEntryController : ControllerBase
{
    private readonly IVisitorEntryService _service;
    private readonly ILogger<VisitorEntryController> _logger;

    public VisitorEntryController(IVisitorEntryService service, ILogger<VisitorEntryController> logger)
    {
        _service = service;
        _logger = logger;
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
    public async Task<IActionResult> Create([FromBody] CreateVisitorEntryDTO dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        var (success, message, id) = await _service.CreateAsync(dto);
        if (!success) return BadRequest(new { message });
        return CreatedAtAction(nameof(GetById), new { id }, new { message, id });
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateVisitorEntryDTO dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        try
        {
            var (success, message, item) = await _service.UpdateAsync(id, dto);
            if (!success) return BadRequest(new { message });
            return Ok(new { message, item });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating VisitorEntry {Id}", id);
            return StatusCode(500, new { message = "Server error while updating entry", detail = ex.Message });
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var (success, message) = await _service.DeleteAsync(id);
        if (!success) return NotFound(new { message });
        return Ok(new { message });
    }
}
