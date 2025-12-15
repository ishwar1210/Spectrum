using Spectrum.DTOs;

namespace Spectrum.Services;

public interface IParcelService
{
    Task<(bool Success, string Message, int? Id)> CreateAsync(ParcelCreateDTO dto);
    Task<(bool Success, string Message, ParcelResponseDTO? Parcel)> GetByIdAsync(int id);
    Task<(bool Success, string Message, IEnumerable<ParcelResponseDTO> Parcels)> GetAllAsync();
    Task<(bool Success, string Message, ParcelResponseDTO? Parcel)> UpdateAsync(int id, ParcelUpdateDTO dto);
    Task<(bool Success, string Message)> DeleteAsync(int id);
}
