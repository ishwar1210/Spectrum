using Spectrum.DTOs;
using Spectrum.Models;
using Spectrum.Repositories;

namespace Spectrum.Services;

public class RoleService : IRoleService
{
    private readonly IRoleRepository _roleRepository;

    public RoleService(IRoleRepository roleRepository)
    {
        _roleRepository = roleRepository;
    }

    public async Task<(bool Success, string Message, RoleResponseDTO? Role)> GetByIdAsync(int roleId)
    {
        var role = await _roleRepository.GetByIdAsync(roleId);

        if (role == null)
        {
            return (false, "Role not found", null);
        }

        var response = MapToResponseDTO(role);
        return (true, "Role retrieved successfully", response);
    }

    public async Task<(bool Success, string Message, IEnumerable<RoleResponseDTO> Roles)> GetAllAsync()
    {
        var roles = await _roleRepository.GetAllAsync();
        var response = roles.Select(MapToResponseDTO);
        return (true, "Roles retrieved successfully", response);
    }

    public async Task<(bool Success, string Message, int? RoleId)> CreateAsync(CreateRoleDTO createDto)
    {
        // Check if role name already exists
        if (await _roleRepository.RoleNameExistsAsync(createDto.RoleName))
        {
            return (false, "Role name already exists", null);
        }

        var role = new Role
        {
            RoleName = createDto.RoleName
        };

        var roleId = await _roleRepository.CreateAsync(role);
        return (true, "Role created successfully", roleId);
    }

    public async Task<(bool Success, string Message, RoleResponseDTO? Role)> UpdateAsync(int roleId, UpdateRoleDTO updateDto)
    {
        var existingRole = await _roleRepository.GetByIdAsync(roleId);

        if (existingRole == null)
        {
            return (false, "Role not found", null);
        }

        // Check if new role name already exists (excluding current role)
        if (!string.IsNullOrWhiteSpace(updateDto.RoleName) && 
            await _roleRepository.RoleNameExistsAsync(updateDto.RoleName, roleId))
        {
            return (false, "Role name already exists", null);
        }

        // Update only provided fields
        if (!string.IsNullOrWhiteSpace(updateDto.RoleName))
        {
            existingRole.RoleName = updateDto.RoleName;
        }

        var success = await _roleRepository.UpdateAsync(roleId, existingRole);

        if (!success)
        {
            return (false, "Failed to update role", null);
        }

        var updatedRole = await _roleRepository.GetByIdAsync(roleId);
        var response = MapToResponseDTO(updatedRole!);
        return (true, "Role updated successfully", response);
    }

    public async Task<(bool Success, string Message)> DeleteAsync(int roleId)
    {
        var existingRole = await _roleRepository.GetByIdAsync(roleId);

        if (existingRole == null)
        {
            return (false, "Role not found");
        }

        var success = await _roleRepository.DeleteAsync(roleId);

        if (!success)
        {
            return (false, "Failed to delete role");
        }

        return (true, "Role deleted successfully");
    }

    private static RoleResponseDTO MapToResponseDTO(Role role)
    {
        return new RoleResponseDTO
        {
            RoleId = role.RoleId,
            RoleName = role.RoleName,
            CreatedDate = role.CreatedDate,
            UpdatedDate = role.UpdatedDate
        };
    }
}