using Spectrum.Models;

namespace Spectrum.Repositories;

public interface IDepartmentRepository
{
    Task<Department?> GetByIdAsync(int departmentId);
    Task<IEnumerable<Department>> GetAllAsync();
    Task<IEnumerable<Department>> GetActiveAsync();
    Task<int> CreateAsync(Department department);
    Task<bool> UpdateAsync(int departmentId, Department department);
    Task<bool> DepartmentNameExistsAsync(string departmentName, int? excludeDepartmentId = null);
    Task<bool> DeleteAsync(int departmentId);
}