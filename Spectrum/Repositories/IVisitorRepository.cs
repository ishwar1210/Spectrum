using Spectrum.Models;

namespace Spectrum.Repositories;

public interface IVisitorRepository
{
    Task<Visitor?> GetByIdAsync(int id);
    Task<IEnumerable<Visitor>> GetAllAsync();
    Task<int> CreateAsync(Visitor visitor);
    Task<bool> UpdateAsync(int id, Visitor visitor);
    Task<bool> DeleteAsync(int id);
}
