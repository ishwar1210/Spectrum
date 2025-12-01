using Spectrum.DTOs;

namespace Spectrum.Services;

public interface IVisitorEntryService
{
    Task<(bool Success, string Message, VisitorEntryResponseDTO? Entry)> GetByIdAsync(int id);
    Task<(bool Success, string Message, IEnumerable<VisitorEntryResponseDTO> Entries)> GetAllAsync();
    Task<(bool Success, string Message, int? EntryId)> CreateAsync(CreateVisitorEntryDTO createDto);
    Task<(bool Success, string Message, VisitorEntryResponseDTO? Entry)> UpdateAsync(int id, UpdateVisitorEntryDTO updateDto);
    Task<(bool Success, string Message)> DeleteAsync(int id);
}
