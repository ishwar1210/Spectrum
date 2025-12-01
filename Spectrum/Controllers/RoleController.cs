using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Spectrum.DTOs;
using Spectrum.Services;

namespace Spectrum.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class RoleController : ControllerBase
{
    private readonly IRoleService _roleService;

    public RoleController(IRoleService roleService)
    {
        _roleService = roleService;
    }

    [HttpGet("{roleId}")]
    public async Task<IActionResult> GetById(int roleId)
    {
        var (success, message, role) = await _roleService.GetByIdAsync(roleId);

        if (!success)
        {
            return NotFound(new { message });
        }

        return Ok(new { message, role });
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var (success, message, roles) = await _roleService.GetAllAsync();
        return Ok(new { message, roles });
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateRoleDTO createDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var (success, message, roleId) = await _roleService.CreateAsync(createDto);

        if (!success)
        {
            return BadRequest(new { message });
        }

        return CreatedAtAction(nameof(GetById), new { roleId }, new { message, roleId });
    }

    [HttpPut("{roleId}")]
    public async Task<IActionResult> Update(int roleId, [FromBody] UpdateRoleDTO updateDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var (success, message, role) = await _roleService.UpdateAsync(roleId, updateDto);

        if (!success)
        {
            return message == "Role not found" ? NotFound(new { message }) : BadRequest(new { message });
        }

        return Ok(new { message, role });
    }

    [HttpDelete("{roleId}")]
    public async Task<IActionResult> Delete(int roleId)
    {
        var (success, message) = await _roleService.DeleteAsync(roleId);

        if (!success)
        {
            return NotFound(new { message });
        }

        return Ok(new { message });
    }
}