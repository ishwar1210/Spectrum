using Spectrum.Models;

namespace Spectrum.Repositories;

public interface IMeetingRoomRepository
{
    Task<MeetingRoom?> GetByIdAsync(int id);
    Task<IEnumerable<MeetingRoom>> GetAllAsync();
    Task<int> CreateAsync(MeetingRoom m);
    Task<bool> UpdateAsync(int id, MeetingRoom m);
    Task<bool> DeleteAsync(int id);
}
