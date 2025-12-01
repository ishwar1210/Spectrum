using Spectrum.Models;

namespace Spectrum.Repositories;

public interface IVisitorEntryRepository
{
    Task<VisitorEntry?> GetByIdAsync(int id);
    Task<IEnumerable<VisitorEntry>> GetAllAsync();
    Task<int> CreateAsync(VisitorEntry entry);
    Task<bool> UpdateAsync(int id, VisitorEntry entry);
    Task<bool> DeleteAsync(int id);
}
