using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Spectrum.DTOs;
using Spectrum.Services;

namespace Spectrum.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class LocationController : ControllerBase
{
    private readonly ILocationService _locationService;

    public LocationController(ILocationService locationService)
    {
        _locationService = locationService;
    }

    // Allow GET /api/location as alias for GetAll
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var (success, message, locations) = await _locationService.GetAllAsync();
        if (!success) return BadRequest(new { message });
        return Ok(locations);
    }

    [HttpGet("all")]
    public async Task<IActionResult> GetAll()
    {
        var (success, message, locations) = await _locationService.GetAllAsync();
        if (!success) return BadRequest(new { message });
        return Ok(locations);
    }

    [HttpGet("active")]
    public async Task<IActionResult> GetActive()
    {
        var (success, message, locations) = await _locationService.GetActiveAsync();
        if (!success) return BadRequest(new { message });
        return Ok(locations);
    }

    [HttpGet("{locationId}")]
    public async Task<IActionResult> GetById(int locationId)
    {
        var (success, message, location) = await _locationService.GetByIdAsync(locationId);
        if (!success) return NotFound(new { message });
        return Ok(location);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateLocationDTO createDto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var (success, message, locationId) = await _locationService.CreateAsync(createDto);
        if (!success) return BadRequest(new { message });

        return CreatedAtAction(nameof(GetById), new { locationId }, new { message, locationId });
    }

    [HttpPut("{locationId}")]
    public async Task<IActionResult> Update(int locationId, [FromBody] UpdateLocationDTO updateDto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var (success, message, location) = await _locationService.UpdateAsync(locationId, updateDto);
        if (!success) return BadRequest(new { message });

        return Ok(new { message, location });
    }

    [HttpDelete("{locationId}")]
    public async Task<IActionResult> Delete(int locationId)
    {
        var (success, message) = await _locationService.DeleteAsync(locationId);
        if (!success) return NotFound(new { message });
        return Ok(new { message });
    }
}
