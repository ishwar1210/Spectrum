using Spectrum.DTOs;
using Spectrum.Models;
using Spectrum.Repositories;

namespace Spectrum.Services;

public class RoomBookingService : IRoomBookingService
{
    private readonly IRoomBookingRepository _repo;

    public RoomBookingService(IRoomBookingRepository repo)
    {
        _repo = repo;
    }

    public async Task<(bool Success, string Message, RoomBookingResponseDTO? Booking)> GetByIdAsync(int id)
    {
        var b = await _repo.GetByIdAsync(id);
        if (b == null) return (false, "Booking not found", null);
        return (true, "Booking retrieved successfully", Map(b));
    }

    public async Task<(bool Success, string Message, IEnumerable<RoomBookingResponseDTO> Bookings)> GetAllAsync()
    {
        var items = await _repo.GetAllAsync();
        return (true, "Bookings retrieved successfully", items.Select(Map));
    }

    public async Task<(bool Success, string Message, int? BookingId)> CreateAsync(CreateRoomBookingDTO createDto)
    {
        var b = new RoomBooking
        {
            RoomBooking_MeetingroomId = createDto.RoomBooking_MeetingroomId,
            RoomBooking_UserID = createDto.RoomBooking_UserID,
            RoomBooking_VisitorID = createDto.RoomBooking_VisitorID,
            RoomBooking_MeetingDate = createDto.RoomBooking_MeetingDate,
            RoomBooking_Starttime = createDto.RoomBooking_Starttime,
            RoomBooking_Endtime = createDto.RoomBooking_Endtime
        };

        var id = await _repo.CreateAsync(b);
        return (true, "Booking created successfully", id);
    }

    public async Task<(bool Success, string Message, RoomBookingResponseDTO? Booking)> UpdateAsync(int id, UpdateRoomBookingDTO updateDto)
    {
        var existing = await _repo.GetByIdAsync(id);
        if (existing == null) return (false, "Booking not found", null);

        if (updateDto.RoomBooking_MeetingroomId.HasValue) existing.RoomBooking_MeetingroomId = updateDto.RoomBooking_MeetingroomId.Value;
        if (updateDto.RoomBooking_UserID.HasValue) existing.RoomBooking_UserID = updateDto.RoomBooking_UserID.Value;
        if (updateDto.RoomBooking_VisitorID.HasValue) existing.RoomBooking_VisitorID = updateDto.RoomBooking_VisitorID.Value;
        if (updateDto.RoomBooking_MeetingDate.HasValue) existing.RoomBooking_MeetingDate = updateDto.RoomBooking_MeetingDate.Value;
        if (updateDto.RoomBooking_Starttime.HasValue) existing.RoomBooking_Starttime = updateDto.RoomBooking_Starttime.Value;
        if (updateDto.RoomBooking_Endtime.HasValue) existing.RoomBooking_Endtime = updateDto.RoomBooking_Endtime.Value;

        var success = await _repo.UpdateAsync(id, existing);
        if (!success) return (false, "Failed to update booking", null);

        var updated = await _repo.GetByIdAsync(id);
        return (true, "Booking updated successfully", Map(updated!));
    }

    public async Task<(bool Success, string Message)> DeleteAsync(int id)
    {
        var existing = await _repo.GetByIdAsync(id);
        if (existing == null) return (false, "Booking not found");

        var success = await _repo.DeleteAsync(id);
        if (!success) return (false, "Failed to delete booking");

        return (true, "Booking deleted successfully");
    }

    private static RoomBookingResponseDTO Map(RoomBooking b)
    {
        return new RoomBookingResponseDTO
        {
            RoomBookingID = b.RoomBookingID,
            RoomBooking_MeetingroomId = b.RoomBooking_MeetingroomId,
            RoomBooking_UserID = b.RoomBooking_UserID,
            RoomBooking_VisitorID = b.RoomBooking_VisitorID,
            RoomBooking_MeetingDate = b.RoomBooking_MeetingDate,
            RoomBooking_Starttime = b.RoomBooking_Starttime,
            RoomBooking_Endtime = b.RoomBooking_Endtime,
            CreatedDate = b.CreatedDate,
            UpdatedDate = b.UpdatedDate
        };
    }
}
