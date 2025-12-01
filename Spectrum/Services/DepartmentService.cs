using Spectrum.DTOs;
using Spectrum.Models;
using Spectrum.Repositories;

namespace Spectrum.Services;

public class DepartmentService : IDepartmentService
{
    private readonly IDepartmentRepository _departmentRepository;

    public DepartmentService(IDepartmentRepository departmentRepository)
    {
        _departmentRepository = departmentRepository;
    }

    public async Task<(bool Success, string Message, DepartmentResponseDTO? Department)> GetByIdAsync(int departmentId)
    {
        var department = await _departmentRepository.GetByIdAsync(departmentId);

        if (department == null)
        {
            return (false, "Department not found", null);
        }

        var response = MapToResponseDTO(department);
        return (true, "Department retrieved successfully", response);
    }

    public async Task<(bool Success, string Message, IEnumerable<DepartmentResponseDTO> Departments)> GetAllAsync()
    {
        var departments = await _departmentRepository.GetAllAsync();
        var response = departments.Select(MapToResponseDTO);
        return (true, "Departments retrieved successfully", response);
    }

    public async Task<(bool Success, string Message, IEnumerable<DepartmentResponseDTO> Departments)> GetActiveAsync()
    {
        var departments = await _departmentRepository.GetActiveAsync();
        var response = departments.Select(MapToResponseDTO);
        return (true, "Active departments retrieved successfully", response);
    }

    public async Task<(bool Success, string Message, int? DepartmentId)> CreateAsync(CreateDepartmentDTO createDto)
    {
        // Check if department name already exists
        if (await _departmentRepository.DepartmentNameExistsAsync(createDto.DepartmentName))
        {
            return (false, "Department name already exists", null);
        }

        var department = new Department
        {
            DepartmentName = createDto.DepartmentName
        };

        var departmentId = await _departmentRepository.CreateAsync(department);
        return (true, "Department created successfully", departmentId);
    }

    public async Task<(bool Success, string Message, DepartmentResponseDTO? Department)> UpdateAsync(int departmentId, UpdateDepartmentDTO updateDto)
    {
        var existingDepartment = await _departmentRepository.GetByIdAsync(departmentId);

        if (existingDepartment == null)
        {
            return (false, "Department not found", null);
        }

        // Check if new department name already exists (excluding current department)
        if (!string.IsNullOrWhiteSpace(updateDto.Department) && 
            await _departmentRepository.DepartmentNameExistsAsync(updateDto.Department, departmentId))
        {
            return (false, "Department name already exists", null);
        }

        // Update only provided fields
        if (!string.IsNullOrWhiteSpace(updateDto.Department))
        {
            existingDepartment.DepartmentName = updateDto.Department;
        }

        if (updateDto.IsActive.HasValue)
        {
            existingDepartment.IsActive = updateDto.IsActive.Value;
        }

        var success = await _departmentRepository.UpdateAsync(departmentId, existingDepartment);

        if (!success)
        {
            return (false, "Failed to update department", null);
        }

        var updatedDepartment = await _departmentRepository.GetByIdAsync(departmentId);
        var response = MapToResponseDTO(updatedDepartment!);
        return (true, "Department updated successfully", response);
    }

    public async Task<(bool Success, string Message)> DeleteAsync(int departmentId)
    {
        var existingDepartment = await _departmentRepository.GetByIdAsync(departmentId);

        if (existingDepartment == null)
        {
            return (false, "Department not found");
        }

        var success = await _departmentRepository.DeleteAsync(departmentId);

        if (!success)
        {
            return (false, "Failed to delete department");
        }

        return (true, "Department deleted successfully");
    }

    private static DepartmentResponseDTO MapToResponseDTO(Department department)
    {
        return new DepartmentResponseDTO
        {
            DepartmentId = department.DepartmentId,
            Department = department.DepartmentName,
            CreatedDate = department.CreatedDate,
            UpdatedDate = department.UpdatedDate,
            IsActive = department.IsActive
        };
    }
}