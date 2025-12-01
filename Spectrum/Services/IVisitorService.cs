using Spectrum.DTOs;

namespace Spectrum.Services;

public interface IVisitorService
{
    Task<(bool Success, string Message, VisitorResponseDTO? Visitor)> GetByIdAsync(int id);
    Task<(bool Success, string Message, IEnumerable<VisitorResponseDTO> Visitors)> GetAllAsync();
    Task<(bool Success, string Message, int? VisitorId)> CreateAsync(CreateVisitorDTO createDto);
    Task<(bool Success, string Message, VisitorResponseDTO? Visitor)> UpdateAsync(int id, UpdateVisitorDTO updateDto);
    Task<(bool Success, string Message)> DeleteAsync(int id);
}
