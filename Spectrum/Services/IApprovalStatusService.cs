using Spectrum.DTOs;

namespace Spectrum.Services;

public interface IApprovalStatusService
{
    Task<(bool Success, string Message, ApprovalStatusResponseDTO? Approval)> GetByIdAsync(int id);
    Task<(bool Success, string Message, IEnumerable<ApprovalStatusResponseDTO> Approvals)> GetAllAsync();
    Task<(bool Success, string Message, int? ApprovalId)> CreateAsync(CreateApprovalStatusDTO createDto);
    Task<(bool Success, string Message, ApprovalStatusResponseDTO? Approval)> UpdateAsync(int id, UpdateApprovalStatusDTO updateDto);
    Task<(bool Success, string Message)> DeleteAsync(int id);
}
