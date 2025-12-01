using Spectrum.DTOs;
using Spectrum.Models;
using Spectrum.Repositories;

namespace Spectrum.Services;

public class MaterialService : IMaterialService
{
    private readonly IMaterialRepository _repo;

    public MaterialService(IMaterialRepository repo)
    {
        _repo = repo;
    }

    public async Task<(bool Success, string Message, MaterialResponseDTO? Material)> GetByIdAsync(int id)
    {
        var m = await _repo.GetByIdAsync(id);
        if (m == null) return (false, "Material not found", null);
        return (true, "Material retrieved successfully", Map(m));
    }

    public async Task<(bool Success, string Message, IEnumerable<MaterialResponseDTO> Materials)> GetAllAsync()
    {
        var items = await _repo.GetAllAsync();
        return (true, "Materials retrieved successfully", items.Select(Map));
    }

    public async Task<(bool Success, string Message, int? MaterialId)> CreateAsync(CreateMaterialDTO createDto)
    {
        var m = new Material
        {
            Material_Name = createDto.Material_Name,
            Material_Code = createDto.Material_Code,
            Material_Status = createDto.Material_Status,
            Material_VisitorId = createDto.Material_VisitorId,
            Material_EntryDate = createDto.Material_EntryDate
        };

        var id = await _repo.CreateAsync(m);
        return (true, "Material created successfully", id);
    }

    public async Task<(bool Success, string Message, MaterialResponseDTO? Material)> UpdateAsync(int id, UpdateMaterialDTO updateDto)
    {
        var existing = await _repo.GetByIdAsync(id);
        if (existing == null) return (false, "Material not found", null);

        if (updateDto.Material_Name != null) existing.Material_Name = updateDto.Material_Name;
        if (updateDto.Material_Code != null) existing.Material_Code = updateDto.Material_Code;
        if (updateDto.Material_Status != null) existing.Material_Status = updateDto.Material_Status;
        if (updateDto.Material_VisitorId.HasValue) existing.Material_VisitorId = updateDto.Material_VisitorId.Value;
        if (updateDto.Material_EntryDate.HasValue) existing.Material_EntryDate = updateDto.Material_EntryDate.Value;

        var success = await _repo.UpdateAsync(id, existing);
        if (!success) return (false, "Failed to update material", null);

        var updated = await _repo.GetByIdAsync(id);
        return (true, "Material updated successfully", Map(updated!));
    }

    public async Task<(bool Success, string Message)> DeleteAsync(int id)
    {
        var existing = await _repo.GetByIdAsync(id);
        if (existing == null) return (false, "Material not found");

        var success = await _repo.DeleteAsync(id);
        if (!success) return (false, "Failed to delete material");

        return (true, "Material deleted successfully");
    }

    private static MaterialResponseDTO Map(Material m)
    {
        return new MaterialResponseDTO
        {
            MaterialId = m.MaterialId,
            Material_Name = m.Material_Name,
            Material_Code = m.Material_Code,
            Material_Status = m.Material_Status,
            Material_VisitorId = m.Material_VisitorId,
            Material_EntryDate = m.Material_EntryDate,
            CreatedDate = m.CreatedDate,
            UpdatedDate = m.UpdatedDate
        };
    }
}
