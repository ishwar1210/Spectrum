using Spectrum.Models;

namespace Spectrum.Repositories;

public interface IRoleRepository
{
    Task<Role?> GetByIdAsync(int roleId);
    Task<Role?> GetByNameAsync(string roleName);
    Task<IEnumerable<Role>> GetAllAsync();
    Task<int> CreateAsync(Role role);
    Task<bool> UpdateAsync(int roleId, Role role);
    Task<bool> DeleteAsync(int roleId);
    Task<bool> RoleNameExistsAsync(string roleName, int? excludeRoleId = null);
}