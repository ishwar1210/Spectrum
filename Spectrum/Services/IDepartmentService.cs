using Spectrum.DTOs;

namespace Spectrum.Services;

public interface IDepartmentService
{
    Task<(bool Success, string Message, DepartmentResponseDTO? Department)> GetByIdAsync(int departmentId);
    Task<(bool Success, string Message, IEnumerable<DepartmentResponseDTO> Departments)> GetAllAsync();
    Task<(bool Success, string Message, IEnumerable<DepartmentResponseDTO> Departments)> GetActiveAsync();
    Task<(bool Success, string Message, int? DepartmentId)> CreateAsync(CreateDepartmentDTO createDto);
    Task<(bool Success, string Message, DepartmentResponseDTO? Department)> UpdateAsync(int departmentId, UpdateDepartmentDTO updateDto);
    Task<(bool Success, string Message)> DeleteAsync(int departmentId);
}