using Spectrum.Models;

namespace Spectrum.Repositories;

public interface IApprovalStatusRepository
{
    Task<ApprovalStatus?> GetByIdAsync(int id);
    Task<IEnumerable<ApprovalStatus>> GetAllAsync();
    Task<int> CreateAsync(ApprovalStatus a);
    Task<bool> UpdateAsync(int id, ApprovalStatus a);
    Task<bool> DeleteAsync(int id);
}
