using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Spectrum.DTOs;
using Spectrum.Services;

namespace Spectrum.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class DepartmentController : ControllerBase
{
    private readonly IDepartmentService _departmentService;

    public DepartmentController(IDepartmentService departmentService)
    {
        _departmentService = departmentService;
    }

    /// <summary>
    /// Get all departments
    /// </summary>
    [HttpGet("all")]
    public async Task<IActionResult> GetAll()
    {
        var (success, message, departments) = await _departmentService.GetAllAsync();
        
        if (!success)
        {
            return BadRequest(new { message });
        }

        return Ok(departments);
    }

    /// <summary>
    /// Get all active departments
    /// </summary>
    [HttpGet("active")]
    public async Task<IActionResult> GetActive()
    {
        var (success, message, departments) = await _departmentService.GetActiveAsync();
        
        if (!success)
        {
            return BadRequest(new { message });
        }

        return Ok(departments);
    }

    /// <summary>
    /// Get department by ID
    /// </summary>
    [HttpGet("{departmentId}")]
    public async Task<IActionResult> GetById(int departmentId)
    {
        var (success, message, department) = await _departmentService.GetByIdAsync(departmentId);

        if (!success)
        {
            return NotFound(new { message });
        }

        return Ok(department);
    }

    /// <summary>
    /// Create a new department
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateDepartmentDTO createDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var (success, message, departmentId) = await _departmentService.CreateAsync(createDto);

        if (!success)
        {
            return BadRequest(new { message });
        }

        return CreatedAtAction(nameof(GetById), new { departmentId }, new { message, departmentId });
    }

    /// <summary>
    /// Update a department
    /// </summary>
    [HttpPut("{departmentId}")]
    public async Task<IActionResult> Update(int departmentId, [FromBody] UpdateDepartmentDTO updateDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var (success, message, department) = await _departmentService.UpdateAsync(departmentId, updateDto);

        if (!success)
        {
            return department == null ? NotFound(new { message }) : BadRequest(new { message });
        }

        return Ok(new { message, department });
    }

    /// <summary>
    /// Delete a department
    /// </summary>
    [HttpDelete("{departmentId}")]
    public async Task<IActionResult> Delete(int departmentId)
    {
        var (success, message) = await _departmentService.DeleteAsync(departmentId);

        if (!success)
        {
            return NotFound(new { message });
        }

        return Ok(new { message });
    }
}