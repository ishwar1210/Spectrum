using Spectrum.DTOs;

namespace Spectrum.Services;

public interface IVendorEmpService
{
    Task<(bool Success, string Message, VendorEmpResponseDTO? Emp)> GetByIdAsync(int id);
    Task<(bool Success, string Message, IEnumerable<VendorEmpResponseDTO> Emps)> GetAllAsync();
    Task<(bool Success, string Message, int? EmpId)> CreateAsync(CreateVendorEmpDTO createDto);
    Task<(bool Success, string Message, VendorEmpResponseDTO? Emp)> UpdateAsync(int id, UpdateVendorEmpDTO updateDto);
    Task<(bool Success, string Message)> DeleteAsync(int id);
}
