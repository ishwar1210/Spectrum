using Spectrum.DTOs;

namespace Spectrum.Services;

public interface IRoomBookingService
{
    Task<(bool Success, string Message, RoomBookingResponseDTO? Booking)> GetByIdAsync(int id);
    Task<(bool Success, string Message, IEnumerable<RoomBookingResponseDTO> Bookings)> GetAllAsync();
    Task<(bool Success, string Message, int? BookingId)> CreateAsync(CreateRoomBookingDTO createDto);
    Task<(bool Success, string Message, RoomBookingResponseDTO? Booking)> UpdateAsync(int id, UpdateRoomBookingDTO updateDto);
    Task<(bool Success, string Message)> DeleteAsync(int id);
}
