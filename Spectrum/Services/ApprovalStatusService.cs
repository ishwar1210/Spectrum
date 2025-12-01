using Spectrum.DTOs;
using Spectrum.Models;
using Spectrum.Repositories;

namespace Spectrum.Services;

public class ApprovalStatusService : IApprovalStatusService
{
    private readonly IApprovalStatusRepository _repo;

    public ApprovalStatusService(IApprovalStatusRepository repo)
    {
        _repo = repo;
    }

    public async Task<(bool Success, string Message, ApprovalStatusResponseDTO? Approval)> GetByIdAsync(int id)
    {
        var a = await _repo.GetByIdAsync(id);
        if (a == null) return (false, "Approval not found", null);
        return (true, "Approval retrieved successfully", Map(a));
    }

    public async Task<(bool Success, string Message, IEnumerable<ApprovalStatusResponseDTO> Approvals)> GetAllAsync()
    {
        var items = await _repo.GetAllAsync();
        return (true, "Approvals retrieved successfully", items.Select(Map));
    }

    public async Task<(bool Success, string Message, int? ApprovalId)> CreateAsync(CreateApprovalStatusDTO createDto)
    {
        var a = new ApprovalStatus
        {
            ApprovalStatus_Gatepass = createDto.ApprovalStatus_Gatepass,
            ApprovalStatus_VisitorEntryId = createDto.ApprovalStatus_VisitorEntryId,
            ApprovalStatus_TransactionDate = createDto.ApprovalStatus_TransactionDate,
            ApprovalStatus_ApprovalDate = createDto.ApprovalStatus_ApprovalDate,
            ApprovalStatus_ApprovalStatus = createDto.ApprovalStatus_ApprovalStatus ?? false,
            ApprovalStatus_Remark = createDto.ApprovalStatus_Remark,
            ApprovalStatus_ApprovalPersonRoleID = createDto.ApprovalStatus_ApprovalPersonRoleID,
            ApprovalStatus_UserId = createDto.ApprovalStatus_UserId
        };

        var id = await _repo.CreateAsync(a);
        return (true, "Approval created successfully", id);
    }

    public async Task<(bool Success, string Message, ApprovalStatusResponseDTO? Approval)> UpdateAsync(int id, UpdateApprovalStatusDTO updateDto)
    {
        var existing = await _repo.GetByIdAsync(id);
        if (existing == null) return (false, "Approval not found", null);

        if (updateDto.ApprovalStatus_Gatepass != null) existing.ApprovalStatus_Gatepass = updateDto.ApprovalStatus_Gatepass;
        if (updateDto.ApprovalStatus_VisitorEntryId.HasValue) existing.ApprovalStatus_VisitorEntryId = updateDto.ApprovalStatus_VisitorEntryId.Value;
        if (updateDto.ApprovalStatus_TransactionDate.HasValue) existing.ApprovalStatus_TransactionDate = updateDto.ApprovalStatus_TransactionDate;
        if (updateDto.ApprovalStatus_ApprovalDate.HasValue) existing.ApprovalStatus_ApprovalDate = updateDto.ApprovalStatus_ApprovalDate;
        if (updateDto.ApprovalStatus_ApprovalStatus.HasValue) existing.ApprovalStatus_ApprovalStatus = updateDto.ApprovalStatus_ApprovalStatus.Value;
        if (updateDto.ApprovalStatus_Remark != null) existing.ApprovalStatus_Remark = updateDto.ApprovalStatus_Remark;
        if (updateDto.ApprovalStatus_ApprovalPersonRoleID.HasValue) existing.ApprovalStatus_ApprovalPersonRoleID = updateDto.ApprovalStatus_ApprovalPersonRoleID.Value;
        if (updateDto.ApprovalStatus_UserId.HasValue) existing.ApprovalStatus_UserId = updateDto.ApprovalStatus_UserId.Value;

        var success = await _repo.UpdateAsync(id, existing);
        if (!success) return (false, "Failed to update approval", null);

        var updated = await _repo.GetByIdAsync(id);
        return (true, "Approval updated successfully", Map(updated!));
    }

    public async Task<(bool Success, string Message)> DeleteAsync(int id)
    {
        var existing = await _repo.GetByIdAsync(id);
        if (existing == null) return (false, "Approval not found");

        var success = await _repo.DeleteAsync(id);
        if (!success) return (false, "Failed to delete approval");

        return (true, "Approval deleted successfully");
    }

    private static ApprovalStatusResponseDTO Map(ApprovalStatus a)
    {
        return new ApprovalStatusResponseDTO
        {
            ApprovalStatusId = a.ApprovalStatusId,
            ApprovalStatus_Gatepass = a.ApprovalStatus_Gatepass,
            ApprovalStatus_VisitorEntryId = a.ApprovalStatus_VisitorEntryId,
            ApprovalStatus_TransactionDate = a.ApprovalStatus_TransactionDate,
            ApprovalStatus_ApprovalDate = a.ApprovalStatus_ApprovalDate,
            ApprovalStatus_ApprovalStatus = a.ApprovalStatus_ApprovalStatus,
            ApprovalStatus_Remark = a.ApprovalStatus_Remark,
            ApprovalStatus_ApprovalPersonRoleID = a.ApprovalStatus_ApprovalPersonRoleID,
            ApprovalStatus_UserId = a.ApprovalStatus_UserId,
            CreatedDate = a.CreatedDate,
            UpdatedDate = a.UpdatedDate
        };
    }
}
