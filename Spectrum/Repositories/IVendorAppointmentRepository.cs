using Spectrum.Models;

namespace Spectrum.Repositories;

public interface IVendorAppointmentRepository
{
    Task<VendorAppointment?> GetByIdAsync(int id);
    Task<IEnumerable<VendorAppointment>> GetAllAsync();
    Task<int> CreateAsync(VendorAppointment appointment);
    Task<bool> UpdateAsync(int id, VendorAppointment appointment);
    Task<bool> DeleteAsync(int id);
}
