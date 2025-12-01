using Spectrum.DTOs;

namespace Spectrum.Services;

public interface IMaterialService
{
    Task<(bool Success, string Message, MaterialResponseDTO? Material)> GetByIdAsync(int id);
    Task<(bool Success, string Message, IEnumerable<MaterialResponseDTO> Materials)> GetAllAsync();
    Task<(bool Success, string Message, int? MaterialId)> CreateAsync(CreateMaterialDTO createDto);
    Task<(bool Success, string Message, MaterialResponseDTO? Material)> UpdateAsync(int id, UpdateMaterialDTO updateDto);
    Task<(bool Success, string Message)> DeleteAsync(int id);
}
