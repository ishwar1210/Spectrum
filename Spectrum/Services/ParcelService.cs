using Spectrum.DTOs;
using Spectrum.Models;
using Spectrum.Repositories;

namespace Spectrum.Services;

public class ParcelService : IParcelService
{
    private readonly IParcelRepository _repo;

    public ParcelService(IParcelRepository repo)
    {
        _repo = repo;
    }

    public async Task<(bool Success, string Message, int? Id)> CreateAsync(ParcelCreateDTO dto)
    {
        var parcel = new Parcel
        {
            ParcelBarcode = dto.ParcelBarcode,
            ParcelCompanyName = dto.ParcelCompanyName,
            UserId = dto.UserId,
            IsActive = dto.IsActive ?? true,
            ParcelHandover = dto.ParcelHandover,
            Parcel_type = string.IsNullOrWhiteSpace(dto.Parcel_type) ? "Personal" : dto.Parcel_type
        };

        var id = await _repo.CreateAsync(parcel);
        return (true, "Parcel created successfully", id);
    }

    public async Task<(bool Success, string Message, ParcelResponseDTO? Parcel)> GetByIdAsync(int id)
    {
        var p = await _repo.GetByIdAsync(id);
        if (p == null) return (false, "Parcel not found", null);
        return (true, "Parcel retrieved successfully", Map(p));
    }

    public async Task<(bool Success, string Message, IEnumerable<ParcelResponseDTO> Parcels)> GetAllAsync()
    {
        var items = await _repo.GetAllAsync();
        return (true, "Parcels retrieved successfully", items.Select(Map));
    }

    public async Task<(bool Success, string Message, ParcelResponseDTO? Parcel)> UpdateAsync(int id, ParcelUpdateDTO dto)
    {
        var existing = await _repo.GetByIdAsync(id);
        if (existing == null) return (false, "Parcel not found", null);

        existing.ParcelBarcode = dto.ParcelBarcode;
        existing.ParcelCompanyName = dto.ParcelCompanyName;
        existing.UserId = dto.UserId;
        existing.IsActive = dto.IsActive ?? existing.IsActive;
        existing.ParcelHandover = dto.ParcelHandover ?? existing.ParcelHandover;
        existing.Parcel_type = string.IsNullOrWhiteSpace(dto.Parcel_type) ? existing.Parcel_type : dto.Parcel_type;

        var success = await _repo.UpdateAsync(id, existing);
        if (!success) return (false, "Failed to update parcel", null);

        var updated = await _repo.GetByIdAsync(id);
        return (true, "Parcel updated successfully", Map(updated!));
    }

    public async Task<(bool Success, string Message)> DeleteAsync(int id)
    {
        var existing = await _repo.GetByIdAsync(id);
        if (existing == null) return (false, "Parcel not found");

        var success = await _repo.DeleteAsync(id);
        if (!success) return (false, "Failed to delete parcel");

        return (true, "Parcel deleted successfully");
    }

    private static ParcelResponseDTO Map(Parcel p) => new ParcelResponseDTO
    {
        ParcelId = p.ParcelId,
        ParcelBarcode = p.ParcelBarcode,
        ParcelCompanyName = p.ParcelCompanyName,
        UserId = p.UserId,
        CreatedDate = p.CreatedDate,
        UpdatedDate = p.UpdatedDate,
        IsActive = p.IsActive,
        ParcelHandover = p.ParcelHandover,
        Parcel_type = p.Parcel_type
    };
}
