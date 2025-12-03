using Spectrum.DTOs;

namespace Spectrum.Services;

public interface IMeetingRoomService
{
    Task<(bool Success, string Message, MeetingRoomResponseDTO? Room)> GetByIdAsync(int id);
    Task<(bool Success, string Message, IEnumerable<MeetingRoomResponseDTO> Rooms)> GetAllAsync();
    Task<(bool Success, string Message, int? RoomId)> CreateAsync(CreateMeetingRoomDTO createDto);
    Task<(bool Success, string Message, MeetingRoomResponseDTO? Room)> UpdateAsync(int id, UpdateMeetingRoomDTO updateDto);
    Task<(bool Success, string Message)> DeleteAsync(int id);
}
