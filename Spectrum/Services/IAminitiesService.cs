using Spectrum.DTOs;

namespace Spectrum.Services;

public interface IAminitiesService
{
    Task<(bool Success, string Message, AminitiesResponseDTO? Aminity)> GetByIdAsync(int id);
    Task<(bool Success, string Message, IEnumerable<AminitiesResponseDTO> Aminities)> GetAllAsync();
    Task<(bool Success, string Message, int? AminityId)> CreateAsync(CreateAminitiesDTO createDto);
    Task<(bool Success, string Message, AminitiesResponseDTO? Aminity)> UpdateAsync(int id, UpdateAminitiesDTO updateDto);
    Task<(bool Success, string Message)> DeleteAsync(int id);
}
