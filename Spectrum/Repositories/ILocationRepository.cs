using Spectrum.Models;

namespace Spectrum.Repositories;

public interface ILocationRepository
{
    Task<Location?> GetByIdAsync(int locationId);
    Task<IEnumerable<Location>> GetAllAsync();
    Task<IEnumerable<Location>> GetActiveAsync();
    Task<int> CreateAsync(Location location);
    Task<bool> UpdateAsync(int locationId, Location location);
    Task<bool> DeleteAsync(int locationId);
    Task<bool> LocationNameExistsAsync(string locationName, int? excludeLocationId = null);
}
