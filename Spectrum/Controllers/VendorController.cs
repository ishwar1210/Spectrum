using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Spectrum.DTOs;
using Spectrum.Services;

namespace Spectrum.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class VendorController : ControllerBase
{
    private readonly IVendorService _vendorService;

    public VendorController(IVendorService vendorService)
    {
        _vendorService = vendorService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var (success, message, vendors) = await _vendorService.GetAllAsync();
        if (!success) return BadRequest(new { message });
        return Ok(vendors);
    }

    [HttpGet("active")]
    public async Task<IActionResult> GetActive()
    {
        var (success, message, vendors) = await _vendorService.GetActiveAsync();
        if (!success) return BadRequest(new { message });
        return Ok(vendors);
    }

    [HttpGet("{vendorId}")]
    public async Task<IActionResult> GetById(int vendorId)
    {
        var (success, message, vendor) = await _vendorService.GetByIdAsync(vendorId);
        if (!success) return NotFound(new { message });
        return Ok(vendor);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateVendorDTO createDto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var (success, message, vendorId) = await _vendorService.CreateAsync(createDto);
        if (!success) return BadRequest(new { message });

        return CreatedAtAction(nameof(GetById), new { vendorId }, new { message, vendorId });
    }

    [HttpPut("{vendorId}")]
    public async Task<IActionResult> Update(int vendorId, [FromBody] UpdateVendorDTO updateDto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var (success, message, vendor) = await _vendorService.UpdateAsync(vendorId, updateDto);
        if (!success) return BadRequest(new { message });

        return Ok(new { message, vendor });
    }

    [HttpDelete("{vendorId}")]
    public async Task<IActionResult> Delete(int vendorId)
    {
        var (success, message) = await _vendorService.DeleteAsync(vendorId);
        if (!success) return NotFound(new { message });
        return Ok(new { message });
    }
}
