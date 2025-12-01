using Spectrum.DTOs;
using Spectrum.Models;
using Spectrum.Repositories;

namespace Spectrum.Services;

public class VendorEmpService : IVendorEmpService
{
    private readonly IVendorEmpRepository _repo;

    public VendorEmpService(IVendorEmpRepository repo)
    {
        _repo = repo;
    }

    public async Task<(bool Success, string Message, VendorEmpResponseDTO? Emp)> GetByIdAsync(int id)
    {
        var emp = await _repo.GetByIdAsync(id);
        if (emp == null) return (false, "Employee not found", null);
        return (true, "Employee retrieved successfully", Map(emp));
    }

    public async Task<(bool Success, string Message, IEnumerable<VendorEmpResponseDTO> Emps)> GetAllAsync()
    {
        var items = await _repo.GetAllAsync();
        return (true, "Employees retrieved successfully", items.Select(Map));
    }

    public async Task<(bool Success, string Message, int? EmpId)> CreateAsync(CreateVendorEmpDTO createDto)
    {
        var emp = new VendorEmp
        {
            VendorEmp_VendorID = createDto.VendorEmp_VendorID,
            VendorEmp_Name = createDto.VendorEmp_Name,
            VendorEmp_IDProofType = createDto.VendorEmp_IDProofType,
            VendorEmp_IDProofNo = createDto.VendorEmp_IDProofNo,
            VendorEmp_mobile = createDto.VendorEmp_mobile,
            VendorEmp_VenderAID = createDto.VendorEmp_VenderAID
        };

        var id = await _repo.CreateAsync(emp);
        return (true, "Employee created successfully", id);
    }

    public async Task<(bool Success, string Message, VendorEmpResponseDTO? Emp)> UpdateAsync(int id, UpdateVendorEmpDTO updateDto)
    {
        var existing = await _repo.GetByIdAsync(id);
        if (existing == null) return (false, "Employee not found", null);

        if (updateDto.VendorEmp_VendorID.HasValue) existing.VendorEmp_VendorID = updateDto.VendorEmp_VendorID.Value;
        if (updateDto.VendorEmp_Name != null) existing.VendorEmp_Name = updateDto.VendorEmp_Name;
        if (updateDto.VendorEmp_IDProofType != null) existing.VendorEmp_IDProofType = updateDto.VendorEmp_IDProofType;
        if (updateDto.VendorEmp_IDProofNo != null) existing.VendorEmp_IDProofNo = updateDto.VendorEmp_IDProofNo;
        if (updateDto.VendorEmp_mobile != null) existing.VendorEmp_mobile = updateDto.VendorEmp_mobile;
        if (updateDto.VendorEmp_VenderAID.HasValue) existing.VendorEmp_VenderAID = updateDto.VendorEmp_VenderAID.Value;

        var success = await _repo.UpdateAsync(id, existing);
        if (!success) return (false, "Failed to update employee", null);

        var updated = await _repo.GetByIdAsync(id);
        return (true, "Employee updated successfully", Map(updated!));
    }

    public async Task<(bool Success, string Message)> DeleteAsync(int id)
    {
        var existing = await _repo.GetByIdAsync(id);
        if (existing == null) return (false, "Employee not found");

        var success = await _repo.DeleteAsync(id);
        if (!success) return (false, "Failed to delete employee");

        return (true, "Employee deleted successfully");
    }

    private static VendorEmpResponseDTO Map(VendorEmp e)
    {
        return new VendorEmpResponseDTO
        {
            VendorEmpId = e.VendorEmpId,
            VendorEmp_VendorID = e.VendorEmp_VendorID,
            VendorEmp_Name = e.VendorEmp_Name,
            VendorEmp_IDProofType = e.VendorEmp_IDProofType,
            VendorEmp_IDProofNo = e.VendorEmp_IDProofNo,
            VendorEmp_mobile = e.VendorEmp_mobile,
            VendorEmp_VenderAID = e.VendorEmp_VenderAID,
            CreatedDate = e.CreatedDate,
            UpdatedDate = e.UpdatedDate
        };
    }
}
