using Spectrum.DTOs;

namespace Spectrum.Services;

public interface IVendorService
{
    Task<(bool Success, string Message, VendorResponseDTO? Vendor)> GetByIdAsync(int vendorId);
    Task<(bool Success, string Message, IEnumerable<VendorResponseDTO> Vendors)> GetAllAsync();
    Task<(bool Success, string Message, IEnumerable<VendorResponseDTO> Vendors)> GetActiveAsync();
    Task<(bool Success, string Message, int? VendorId)> CreateAsync(CreateVendorDTO createDto);
    Task<(bool Success, string Message, VendorResponseDTO? Vendor)> UpdateAsync(int vendorId, UpdateVendorDTO updateDto);
    Task<(bool Success, string Message)> DeleteAsync(int vendorId);
}
