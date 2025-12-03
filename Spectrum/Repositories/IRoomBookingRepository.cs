using Spectrum.Models;

namespace Spectrum.Repositories;

public interface IRoomBookingRepository
{
    Task<RoomBooking?> GetByIdAsync(int id);
    Task<IEnumerable<RoomBooking>> GetAllAsync();
    Task<int> CreateAsync(RoomBooking rb);
    Task<bool> UpdateAsync(int id, RoomBooking rb);
    Task<bool> DeleteAsync(int id);
}
