using Spectrum.Models;

namespace Spectrum.Repositories;

public interface IVendorRepository
{
    Task<Vendor?> GetByIdAsync(int vendorId);
    Task<IEnumerable<Vendor>> GetAllAsync();
    Task<IEnumerable<Vendor>> GetActiveAsync();
    Task<int> CreateAsync(Vendor vendor);
    Task<bool> UpdateAsync(int vendorId, Vendor vendor);
    Task<bool> DeleteAsync(int vendorId);
    Task<bool> VendorCodeExistsAsync(string vendorCode, int? excludeVendorId = null);
}
