using Spectrum.DTOs;

namespace Spectrum.Services;

public interface ILocationService
{
    Task<(bool Success, string Message, LocationResponseDTO? Location)> GetByIdAsync(int locationId);
    Task<(bool Success, string Message, IEnumerable<LocationResponseDTO> Locations)> GetAllAsync();
    Task<(bool Success, string Message, IEnumerable<LocationResponseDTO> Locations)> GetActiveAsync();
    Task<(bool Success, string Message, int? LocationId)> CreateAsync(CreateLocationDTO createDto);
    Task<(bool Success, string Message, LocationResponseDTO? Location)> UpdateAsync(int locationId, UpdateLocationDTO updateDto);
    Task<(bool Success, string Message)> DeleteAsync(int locationId);
}
