using Spectrum.Models;

namespace Spectrum.Repositories;

public interface IParcelRepository
{
    Task<int> CreateAsync(Parcel parcel);
    Task<Parcel?> GetByIdAsync(int id);
    Task<IEnumerable<Parcel>> GetAllAsync();
    Task<bool> UpdateAsync(int id, Parcel parcel);
    Task<bool> DeleteAsync(int id);
}
