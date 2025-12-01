using Spectrum.DTOs;

namespace Spectrum.Services;

public interface IRoleService
{
    Task<(bool Success, string Message, RoleResponseDTO? Role)> GetByIdAsync(int roleId);
    Task<(bool Success, string Message, IEnumerable<RoleResponseDTO> Roles)> GetAllAsync();
    Task<(bool Success, string Message, int? RoleId)> CreateAsync(CreateRoleDTO createDto);
    Task<(bool Success, string Message, RoleResponseDTO? Role)> UpdateAsync(int roleId, UpdateRoleDTO updateDto);
    Task<(bool Success, string Message)> DeleteAsync(int roleId);
}