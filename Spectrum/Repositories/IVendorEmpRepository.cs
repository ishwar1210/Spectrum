using Spectrum.Models;

namespace Spectrum.Repositories;

public interface IVendorEmpRepository
{
    Task<VendorEmp?> GetByIdAsync(int id);
    Task<IEnumerable<VendorEmp>> GetAllAsync();
    Task<int> CreateAsync(VendorEmp emp);
    Task<bool> UpdateAsync(int id, VendorEmp emp);
    Task<bool> DeleteAsync(int id);
}
