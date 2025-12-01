using Spectrum.DTOs;
using Spectrum.Models;
using Spectrum.Repositories;

namespace Spectrum.Services;

public class VendorAppointmentService : IVendorAppointmentService
{
    private readonly IVendorAppointmentRepository _repo;

    public VendorAppointmentService(IVendorAppointmentRepository repo)
    {
        _repo = repo;
    }

    public async Task<(bool Success, string Message, VendorAppointmentResponseDTO? Appointment)> GetByIdAsync(int id)
    {
        var appt = await _repo.GetByIdAsync(id);
        if (appt == null) return (false, "Appointment not found", null);
        return (true, "Appointment retrieved successfully", Map(appt));
    }

    public async Task<(bool Success, string Message, IEnumerable<VendorAppointmentResponseDTO> Appointments)> GetAllAsync()
    {
        var items = await _repo.GetAllAsync();
        return (true, "Appointments retrieved successfully", items.Select(Map));
    }

    public async Task<(bool Success, string Message, int? AppointmentId)> CreateAsync(CreateVendorAppointmentDTO createDto)
    {
        var appt = new VendorAppointment
        {
            VendorA_VendorID = createDto.VendorA_VendorID,
            VendorA_Getpass = createDto.VendorA_Getpass,
            VendorA_FromDate = createDto.VendorA_FromDate,
            VendorA_ToDate = createDto.VendorA_ToDate,
            VendorA_VehicleNO = createDto.VendorA_VehicleNO,
            VendorA_IdProofType = createDto.VendorA_IdProofType,
            VendorA_IdProofNo = createDto.VendorA_IdProofNo,
            VendorA_UserId = createDto.VendorA_UserId
        };

        var id = await _repo.CreateAsync(appt);
        return (true, "Appointment created successfully", id);
    }

    public async Task<(bool Success, string Message, VendorAppointmentResponseDTO? Appointment)> UpdateAsync(int id, UpdateVendorAppointmentDTO updateDto)
    {
        var existing = await _repo.GetByIdAsync(id);
        if (existing == null) return (false, "Appointment not found", null);

        if (updateDto.VendorA_VendorID.HasValue) existing.VendorA_VendorID = updateDto.VendorA_VendorID.Value;
        if (updateDto.VendorA_Getpass != null) existing.VendorA_Getpass = updateDto.VendorA_Getpass;
        if (updateDto.VendorA_FromDate.HasValue) existing.VendorA_FromDate = updateDto.VendorA_FromDate;
        if (updateDto.VendorA_ToDate.HasValue) existing.VendorA_ToDate = updateDto.VendorA_ToDate;
        if (updateDto.VendorA_VehicleNO != null) existing.VendorA_VehicleNO = updateDto.VendorA_VehicleNO;
        if (updateDto.VendorA_IdProofType != null) existing.VendorA_IdProofType = updateDto.VendorA_IdProofType;
        if (updateDto.VendorA_IdProofNo != null) existing.VendorA_IdProofNo = updateDto.VendorA_IdProofNo;
        if (updateDto.VendorA_UserId.HasValue) existing.VendorA_UserId = updateDto.VendorA_UserId.Value;

        var success = await _repo.UpdateAsync(id, existing);
        if (!success) return (false, "Failed to update appointment", null);

        var updated = await _repo.GetByIdAsync(id);
        return (true, "Appointment updated successfully", Map(updated!));
    }

    public async Task<(bool Success, string Message)> DeleteAsync(int id)
    {
        var existing = await _repo.GetByIdAsync(id);
        if (existing == null) return (false, "Appointment not found");

        var success = await _repo.DeleteAsync(id);
        if (!success) return (false, "Failed to delete appointment");

        return (true, "Appointment deleted successfully");
    }

    private static VendorAppointmentResponseDTO Map(VendorAppointment a)
    {
        return new VendorAppointmentResponseDTO
        {
            VendorA_Id = a.VendorA_Id,
            VendorA_VendorID = a.VendorA_VendorID,
            VendorA_Getpass = a.VendorA_Getpass,
            VendorA_FromDate = a.VendorA_FromDate,
            VendorA_ToDate = a.VendorA_ToDate,
            VendorA_VehicleNO = a.VendorA_VehicleNO,
            VendorA_IdProofType = a.VendorA_IdProofType,
            VendorA_IdProofNo = a.VendorA_IdProofNo,
            VendorA_UserId = a.VendorA_UserId,
            CreatedDate = a.CreatedDate,
            UpdatedDate = a.UpdatedDate
        };
    }
}
