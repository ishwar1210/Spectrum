using Spectrum.Models;

namespace Spectrum.Repositories;

public interface IMaterialRepository
{
    Task<Material?> GetByIdAsync(int id);
    Task<IEnumerable<Material>> GetAllAsync();
    Task<int> CreateAsync(Material m);
    Task<bool> UpdateAsync(int id, Material m);
    Task<bool> DeleteAsync(int id);
}
