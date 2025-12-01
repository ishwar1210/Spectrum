using Spectrum.DTOs;
using Spectrum.Models;
using Spectrum.Repositories;
using BCrypt.Net;

namespace Spectrum.Services;

public interface IAuthService
{
    Task<(bool Success, string Message, int UserId)> RegisterAsync(RegisterDTO registerDto);
    Task<(bool Success, string Message, LoginResponseDTO? Response)> LoginAsync(LoginDTO loginDto);
    Task<(bool Success, string Message, UserResponseDTO? User)> UpdateUserAsync(int userId, UpdateUserDTO updateDto);
    Task<UserResponseDTO?> GetUserByIdAsync(int userId);
    Task<IEnumerable<UserResponseDTO>> GetAllUsersAsync();
    Task<(bool Success, string Message)> DeleteUserAsync(int userId);
}

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly IJwtService _jwtService;
    private readonly IRoleRepository _roleRepository;

    public AuthService(IUserRepository userRepository, IJwtService jwtService, IRoleRepository roleRepository)
    {
        _userRepository = userRepository;
        _jwtService = jwtService;
        _roleRepository = roleRepository;
    }

    public async Task<(bool Success, string Message, int UserId)> RegisterAsync(RegisterDTO registerDto)
    {
        if (await _userRepository.UsernameExistsAsync(registerDto.Username))
            return (false, "Username already exists", 0);

        if (registerDto.U_ReportingToId.HasValue)
        {
            var manager = await _userRepository.GetByIdAsync(registerDto.U_ReportingToId.Value);
            if (manager == null)
                return (false, "ReportingToId references a non-existent user", 0);
        }

        var passwordHash = BCrypt.Net.BCrypt.HashPassword(registerDto.Password);

        var user = new User
        {
            Username = registerDto.Username,
            Password_hash = passwordHash,
            U_Name = registerDto.U_Name,
            U_Mobile = registerDto.U_Mobile,
            U_Email = registerDto.U_Email,
            U_Address = registerDto.U_Address,
            U_RoleId = registerDto.U_RoleId,
            U_DepartmentID = registerDto.U_DepartmentID,
            U_ReportingToId = registerDto.U_ReportingToId
        };

        var userId = await _userRepository.CreateAsync(user);
        return (true, "User registered successfully", userId);
    }

    public async Task<(bool Success, string Message, LoginResponseDTO? Response)> LoginAsync(LoginDTO loginDto)
    {
        var user = await _userRepository.GetByUsernameAsync(loginDto.Username);

        if (user == null)
        {
            return (false, "Invalid username or password", null);
        }

        if (!BCrypt.Net.BCrypt.Verify(loginDto.Password, user.Password_hash))
        {
            return (false, "Invalid username or password", null);
        }

        var token = await _jwtService.GenerateTokenAsync(user);

        string? roleName = null;
        if (user.U_RoleId.HasValue)
        {
            var role = await _roleRepository.GetByIdAsync(user.U_RoleId.Value);
            if (role != null) roleName = role.RoleName;
        }

        var response = new LoginResponseDTO
        {
            Token = token,
            UserId = user.UserId,
            Username = user.Username,
            U_Name = user.U_Name,
            U_Email = user.U_Email,
            U_RoleId = user.U_RoleId,
            U_RoleName = roleName,
            U_DepartmentID = user.U_DepartmentID
        };

        return (true, "Login successful", response);
    }

    public async Task<(bool Success, string Message, UserResponseDTO? User)> UpdateUserAsync(int userId, UpdateUserDTO updateDto)
    {
        var existingUser = await _userRepository.GetByIdAsync(userId);

        if (existingUser == null)
        {
            return (false, "User not found", null);
        }

        if (updateDto.U_Name != null)
            existingUser.U_Name = updateDto.U_Name;

        if (updateDto.U_Mobile != null)
            existingUser.U_Mobile = updateDto.U_Mobile;

        if (updateDto.U_Email != null)
            existingUser.U_Email = updateDto.U_Email;

        if (updateDto.U_Address != null)
            existingUser.U_Address = updateDto.U_Address;

        if (updateDto.U_RoleId.HasValue)
            existingUser.U_RoleId = updateDto.U_RoleId;

        if (updateDto.U_DepartmentID.HasValue)
            existingUser.U_DepartmentID = updateDto.U_DepartmentID;

        if (updateDto.U_ReportingToId.HasValue)
            existingUser.U_ReportingToId = updateDto.U_ReportingToId;

        if (!string.IsNullOrEmpty(updateDto.NewPassword))
        {
            existingUser.Password_hash = BCrypt.Net.BCrypt.HashPassword(updateDto.NewPassword);
        }

        var success = await _userRepository.UpdateAsync(userId, existingUser);

        if (!success)
        {
            return (false, "Failed to update user", null);
        }

        var updatedUser = await GetUserByIdAsync(userId);
        return (true, "User updated successfully", updatedUser);
    }

    public async Task<UserResponseDTO?> GetUserByIdAsync(int userId)
    {
        var user = await _userRepository.GetByIdAsync(userId);

        if (user == null)
            return null;

        return new UserResponseDTO
        {
            UserId = user.UserId,
            Username = user.Username,
            U_Name = user.U_Name,
            U_Mobile = user.U_Mobile,
            U_Email = user.U_Email,
            U_Address = user.U_Address,
            U_RoleId = user.U_RoleId,
            U_DepartmentID = user.U_DepartmentID,
            U_ReportingToId = user.U_ReportingToId,
            CreatedDate = user.CreatedDate,
            UpdatedDate = user.UpdatedDate
        };
    }

    public async Task<IEnumerable<UserResponseDTO>> GetAllUsersAsync()
    {
        var users = await _userRepository.GetAllAsync();

        return users.Select(user => new UserResponseDTO
        {
            UserId = user.UserId,
            Username = user.Username,
            U_Name = user.U_Name,
            U_Mobile = user.U_Mobile,
            U_Email = user.U_Email,
            U_Address = user.U_Address,
            U_RoleId = user.U_RoleId,
            U_DepartmentID = user.U_DepartmentID,
            U_ReportingToId = user.U_ReportingToId,
            CreatedDate = user.CreatedDate,
            UpdatedDate = user.UpdatedDate
        });
    }

    public async Task<(bool Success, string Message)> DeleteUserAsync(int userId)
    {
        var existingUser = await _userRepository.GetByIdAsync(userId);

        if (existingUser == null)
        {
            return (false, "User not found");
        }

        var success = await _userRepository.DeleteAsync(userId);

        if (!success)
        {
            return (false, "Failed to delete user");
        }

        return (true, "User deleted successfully");
    }
}
