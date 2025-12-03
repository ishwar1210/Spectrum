using Spectrum.DTOs;
using Spectrum.Models;
using Spectrum.Repositories;

namespace Spectrum.Services;

public class AminitiesService : IAminitiesService
{
    private readonly IAminitiesRepository _repo;

    public AminitiesService(IAminitiesRepository repo)
    {
        _repo = repo;
    }

    public async Task<(bool Success, string Message, AminitiesResponseDTO? Aminity)> GetByIdAsync(int id)
    {
        var a = await _repo.GetByIdAsync(id);
        if (a == null) return (false, "Amenity not found", null);
        return (true, "Amenity retrieved successfully", Map(a));
    }

    public async Task<(bool Success, string Message, IEnumerable<AminitiesResponseDTO> Aminities)> GetAllAsync()
    {
        var items = await _repo.GetAllAsync();
        return (true, "Amenities retrieved successfully", items.Select(Map));
    }

    public async Task<(bool Success, string Message, int? AminityId)> CreateAsync(CreateAminitiesDTO createDto)
    {
        var a = new Aminities
        {
            Aminities_Name = createDto.Aminities_Name,
            Aminities_isActive = createDto.Aminities_isActive ?? true
        };

        var id = await _repo.CreateAsync(a);
        return (true, "Amenity created successfully", id);
    }

    public async Task<(bool Success, string Message, AminitiesResponseDTO? Aminity)> UpdateAsync(int id, UpdateAminitiesDTO updateDto)
    {
        var existing = await _repo.GetByIdAsync(id);
        if (existing == null) return (false, "Amenity not found", null);

        if (updateDto.Aminities_Name != null) existing.Aminities_Name = updateDto.Aminities_Name;
        if (updateDto.Aminities_isActive.HasValue) existing.Aminities_isActive = updateDto.Aminities_isActive.Value;

        var success = await _repo.UpdateAsync(id, existing);
        if (!success) return (false, "Failed to update amenity", null);

        var updated = await _repo.GetByIdAsync(id);
        return (true, "Amenity updated successfully", Map(updated!));
    }

    public async Task<(bool Success, string Message)> DeleteAsync(int id)
    {
        var existing = await _repo.GetByIdAsync(id);
        if (existing == null) return (false, "Amenity not found");

        var success = await _repo.DeleteAsync(id);
        if (!success) return (false, "Failed to delete amenity");

        return (true, "Amenity deleted successfully");
    }

    private static AminitiesResponseDTO Map(Aminities a)
    {
        return new AminitiesResponseDTO
        {
            AminitiesId = a.AminitiesId,
            Aminities_Name = a.Aminities_Name,
            Aminities_isActive = a.Aminities_isActive,
            CreatedDate = a.CreatedDate,
            UpdatedDate = a.UpdatedDate
        };
    }
}
