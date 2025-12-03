using Spectrum.Models;

namespace Spectrum.Repositories;

public interface IAminitiesRepository
{
    Task<Aminities?> GetByIdAsync(int id);
    Task<IEnumerable<Aminities>> GetAllAsync();
    Task<int> CreateAsync(Aminities a);
    Task<bool> UpdateAsync(int id, Aminities a);
    Task<bool> DeleteAsync(int id);
}
