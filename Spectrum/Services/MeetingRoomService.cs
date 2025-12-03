using Spectrum.DTOs;
using Spectrum.Models;
using Spectrum.Repositories;

namespace Spectrum.Services;

public class MeetingRoomService : IMeetingRoomService
{
    private readonly IMeetingRoomRepository _repo;

    public MeetingRoomService(IMeetingRoomRepository repo)
    {
        _repo = repo;
    }

    public async Task<(bool Success, string Message, MeetingRoomResponseDTO? Room)> GetByIdAsync(int id)
    {
        var r = await _repo.GetByIdAsync(id);
        if (r == null) return (false, "Meeting room not found", null);
        return (true, "Meeting room retrieved successfully", Map(r));
    }

    public async Task<(bool Success, string Message, IEnumerable<MeetingRoomResponseDTO> Rooms)> GetAllAsync()
    {
        var items = await _repo.GetAllAsync();
        return (true, "Meeting rooms retrieved successfully", items.Select(Map));
    }

    public async Task<(bool Success, string Message, int? RoomId)> CreateAsync(CreateMeetingRoomDTO createDto)
    {
        var r = new MeetingRoom
        {
            MeetingRoom_Name = createDto.MeetingRoom_Name,
            MeetingRoom_Floor = createDto.MeetingRoom_Floor,
            MeetingRoom_Capacity = createDto.MeetingRoom_Capacity,
            MeetingRoom_AminitiesId = createDto.MeetingRoom_AminitiesId
        };

        var id = await _repo.CreateAsync(r);
        return (true, "Meeting room created successfully", id);
    }

    public async Task<(bool Success, string Message, MeetingRoomResponseDTO? Room)> UpdateAsync(int id, UpdateMeetingRoomDTO updateDto)
    {
        var existing = await _repo.GetByIdAsync(id);
        if (existing == null) return (false, "Meeting room not found", null);

        if (updateDto.MeetingRoom_Name != null) existing.MeetingRoom_Name = updateDto.MeetingRoom_Name;
        if (updateDto.MeetingRoom_Floor.HasValue) existing.MeetingRoom_Floor = updateDto.MeetingRoom_Floor.Value;
        if (updateDto.MeetingRoom_Capacity.HasValue) existing.MeetingRoom_Capacity = updateDto.MeetingRoom_Capacity.Value;
        if (updateDto.MeetingRoom_AminitiesId.HasValue) existing.MeetingRoom_AminitiesId = updateDto.MeetingRoom_AminitiesId.Value;

        var success = await _repo.UpdateAsync(id, existing);
        if (!success) return (false, "Failed to update meeting room", null);

        var updated = await _repo.GetByIdAsync(id);
        return (true, "Meeting room updated successfully", Map(updated!));
    }

    public async Task<(bool Success, string Message)> DeleteAsync(int id)
    {
        var existing = await _repo.GetByIdAsync(id);
        if (existing == null) return (false, "Meeting room not found");

        var success = await _repo.DeleteAsync(id);
        if (!success) return (false, "Failed to delete meeting room");

        return (true, "Meeting room deleted successfully");
    }

    private static MeetingRoomResponseDTO Map(MeetingRoom r)
    {
        return new MeetingRoomResponseDTO
        {
            MeetingRoomId = r.MeetingRoomId,
            MeetingRoom_Name = r.MeetingRoom_Name,
            MeetingRoom_Floor = r.MeetingRoom_Floor,
            MeetingRoom_Capacity = r.MeetingRoom_Capacity,
            MeetingRoom_AminitiesId = r.MeetingRoom_AminitiesId,
            CreatedDate = r.CreatedDate,
            UpdatedDate = r.UpdatedDate
        };
    }
}
