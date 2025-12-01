using Spectrum.DTOs;

namespace Spectrum.Services;

public interface IVendorAppointmentService
{
    Task<(bool Success, string Message, VendorAppointmentResponseDTO? Appointment)> GetByIdAsync(int id);
    Task<(bool Success, string Message, IEnumerable<VendorAppointmentResponseDTO> Appointments)> GetAllAsync();
    Task<(bool Success, string Message, int? AppointmentId)> CreateAsync(CreateVendorAppointmentDTO createDto);
    Task<(bool Success, string Message, VendorAppointmentResponseDTO? Appointment)> UpdateAsync(int id, UpdateVendorAppointmentDTO updateDto);
    Task<(bool Success, string Message)> DeleteAsync(int id);
}
