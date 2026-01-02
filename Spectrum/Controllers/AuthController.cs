using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Spectrum.DTOs;
using Spectrum.Services;
using System.Security.Claims;

namespace Spectrum.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterDTO registerDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var (success, message, userId) = await _authService.RegisterAsync(registerDto);

        if (!success)
        {
            return BadRequest(new { message });
        }

        return Ok(new { message, userId });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDTO loginDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var (success, message, response) = await _authService.LoginAsync(loginDto);

        if (!success)
        {
            return Unauthorized(new { message });
        }

        return Ok(response);
    }

    [Authorize]
    [HttpPut("update")]
    public async Task<IActionResult> UpdateCurrentUser([FromBody] UpdateUserDTO updateDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        
        if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
        {
            return Unauthorized(new { message = "Invalid token" });
        }

        var (success, message, user) = await _authService.UpdateUserAsync(userId, updateDto);

        if (!success)
        {
            return BadRequest(new { message });
        }

        return Ok(new { message, user });
    }

    [Authorize]
    [HttpPut("update/{userId}")]
    public async Task<IActionResult> UpdateUser(int userId, [FromBody] UpdateUserDTO updateDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var (success, message, user) = await _authService.UpdateUserAsync(userId, updateDto);

        if (!success)
        {
            return BadRequest(new { message });
        }

        return Ok(new { message, user });
    }

    [HttpGet("me")]
    public async Task<IActionResult> GetCurrentUser()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        
        if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
        {
            return Unauthorized(new { message = "Invalid token" });
        }

        var user = await _authService.GetUserByIdAsync(userId);

        if (user == null)
        {
            return NotFound(new { message = "User not found" });
        }

        return Ok(user);
    }

    [HttpGet("{userId}")]
    public async Task<IActionResult> GetUserById(int userId)
    {
        var user = await _authService.GetUserByIdAsync(userId);

        if (user == null)
        {
            return NotFound(new { message = "User not found" });
        }

        return Ok(user);
    }

    [HttpGet("all")]
    public async Task<IActionResult> GetAllUsers()
    {
        var users = await _authService.GetAllUsersAsync();
        return Ok(users);
    }

    /// <summary>
    /// Delete a user by ID
    /// </summary>
    [Authorize]
    [HttpDelete("{userId}")]
    public async Task<IActionResult> DeleteUser(int userId)
    {
        var (success, message) = await _authService.DeleteUserAsync(userId);

        if (!success)
        {
            return NotFound(new { message });
        }

        return Ok(new { message });
    }
}
