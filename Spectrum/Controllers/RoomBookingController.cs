using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Spectrum.DTOs;
using Spectrum.Services;

namespace Spectrum.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class RoomBookingController : ControllerBase
{
    private readonly IRoomBookingService _service;

    public RoomBookingController(IRoomBookingService service)
    {
        _service = service;
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
    public async Task<IActionResult> Create([FromBody] CreateRoomBookingDTO dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        var (success, message, id) = await _service.CreateAsync(dto);
        if (!success) return BadRequest(new { message });
        return CreatedAtAction(nameof(GetById), new { id }, new { message, id });
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateRoomBookingDTO dto)
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
}
