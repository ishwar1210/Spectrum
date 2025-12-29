using Spectrum.DTOs;
using Spectrum.Models;
using Spectrum.Repositories;

namespace Spectrum.Services;

public class VisitorEntryService : IVisitorEntryService
{
    private readonly IVisitorEntryRepository _repo;

    public VisitorEntryService(IVisitorEntryRepository repo)
    {
        _repo = repo;
    }

    public async Task<(bool Success, string Message, VisitorEntryResponseDTO? Entry)> GetByIdAsync(int id)
    {
        var e = await _repo.GetByIdAsync(id);
        if (e == null) return (false, "Entry not found", null);
        return (true, "Entry retrieved successfully", Map(e));
    }

    public async Task<(bool Success, string Message, IEnumerable<VisitorEntryResponseDTO> Entries)> GetAllAsync()
    {
        var items = await _repo.GetAllAsync();
        return (true, "Entries retrieved successfully", items.Select(Map));
    }

    public async Task<(bool Success, string Message, int? EntryId)> CreateAsync(CreateVisitorEntryDTO createDto)
    {
        var entry = new VisitorEntry
        {
            VisitorEntry_visitorId = createDto.VisitorEntry_visitorId,
            VisitorEntry_Gatepass = createDto.VisitorEntry_Gatepass,
            VisitorEntry_Vehicletype = createDto.VisitorEntry_Vehicletype,
            VisitorEntry_Vehicleno = createDto.VisitorEntry_Vehicleno,
            VisitorEntry_Purposeofvisit = createDto.VisitorEntry_Purposeofvisit,
            VisitorEntry_Date = createDto.VisitorEntry_Date,
            VisitorEntry_Intime = createDto.VisitorEntry_Intime,
            VisitorEntry_Outtime = createDto.VisitorEntry_Outtime,
            VisitorEntry_Userid = createDto.VisitorEntry_Userid,
            VisitorEntryAdmin_isApproval = createDto.VisitorEntryAdmin_isApproval ?? false,
            VisitorEntryuser_isApproval = createDto.VisitorEntryuser_isApproval ??  false,
            VisitorEntryUser_isReject = createDto.VisitorEntryUser_isReject ?? false,
            VisitorEntry_Remark = createDto.VisitorEntry_Remark,
            VisitorEntry_isCanteen = createDto.VisitorEntry_isCanteen ?? false,
            VisitorEntry_isStay = createDto.VisitorEntry_isStay ?? false
        };

        var id = await _repo.CreateAsync(entry);
        return (true, "Entry created successfully", id);
    }

    public async Task<(bool Success, string Message, VisitorEntryResponseDTO? Entry)> UpdateAsync(int id, UpdateVisitorEntryDTO updateDto)
    {
        var existing = await _repo.GetByIdAsync(id);
        if (existing == null) return (false, "Entry not found", null);

        if (updateDto.VisitorEntry_visitorId.HasValue) existing.VisitorEntry_visitorId = updateDto.VisitorEntry_visitorId.Value;
        if (updateDto.VisitorEntry_Gatepass != null) existing.VisitorEntry_Gatepass = updateDto.VisitorEntry_Gatepass;
        if (updateDto.VisitorEntry_Vehicletype != null) existing.VisitorEntry_Vehicletype = updateDto.VisitorEntry_Vehicletype;
        if (updateDto.VisitorEntry_Vehicleno != null) existing.VisitorEntry_Vehicleno = updateDto.VisitorEntry_Vehicleno;
        if (updateDto.VisitorEntry_Purposeofvisit != null) existing.VisitorEntry_Purposeofvisit = updateDto.VisitorEntry_Purposeofvisit;
        if (updateDto.VisitorEntry_Date.HasValue) existing.VisitorEntry_Date = updateDto.VisitorEntry_Date.Value;
        if (updateDto.VisitorEntry_Intime.HasValue) existing.VisitorEntry_Intime = updateDto.VisitorEntry_Intime.Value;
        if (updateDto.VisitorEntry_Outtime.HasValue) existing.VisitorEntry_Outtime = updateDto.VisitorEntry_Outtime.Value;
        if (updateDto.VisitorEntry_Userid.HasValue) existing.VisitorEntry_Userid = updateDto.VisitorEntry_Userid.Value;
        // Prefer explicit admin/user approval fields
        if (updateDto.VisitorEntryAdmin_isApproval.HasValue) existing.VisitorEntryAdmin_isApproval = updateDto.VisitorEntryAdmin_isApproval.Value;

        if (updateDto.VisitorEntryuser_isApproval.HasValue) existing.VisitorEntryuser_isApproval = updateDto.VisitorEntryuser_isApproval.Value;

        if (updateDto.VisitorEntryUser_isReject.HasValue) existing.VisitorEntryUser_isReject = updateDto.VisitorEntryUser_isReject.Value;

        if (updateDto.VisitorEntry_Remark != null) existing.VisitorEntry_Remark = updateDto.VisitorEntry_Remark;
        if (updateDto.VisitorEntry_isCanteen.HasValue) existing.VisitorEntry_isCanteen = updateDto.VisitorEntry_isCanteen.Value;
        if (updateDto.VisitorEntry_isStay.HasValue) existing.VisitorEntry_isStay = updateDto.VisitorEntry_isStay.Value;

        var success = await _repo.UpdateAsync(id, existing);
        if (!success) return (false, "Failed to update entry", null);

        var updated = await _repo.GetByIdAsync(id);
        return (true, "Entry updated successfully", Map(updated!));
    }

    public async Task<(bool Success, string Message)> DeleteAsync(int id)
    {
        var existing = await _repo.GetByIdAsync(id);
        if (existing == null) return (false, "Entry not found");

        var success = await _repo.DeleteAsync(id);
        if (!success) return (false, "Failed to delete entry");

        return (true, "Entry deleted successfully");
    }

    private static VisitorEntryResponseDTO Map(VisitorEntry e)
    {
        return new VisitorEntryResponseDTO
        {
            VisitorEntryID = e.VisitorEntryID,
            VisitorEntry_visitorId = e.VisitorEntry_visitorId,
            VisitorEntry_Gatepass = e.VisitorEntry_Gatepass,
            VisitorEntry_Vehicletype = e.VisitorEntry_Vehicletype,
            VisitorEntry_Vehicleno = e.VisitorEntry_Vehicleno,
            VisitorEntry_Purposeofvisit = e.VisitorEntry_Purposeofvisit,
            VisitorEntry_Date = e.VisitorEntry_Date,
            VisitorEntry_Intime = e.VisitorEntry_Intime,
            VisitorEntry_Outtime = e.VisitorEntry_Outtime,
            VisitorEntry_Userid = e.VisitorEntry_Userid,
            VisitorEntryAdmin_isApproval = e.VisitorEntryAdmin_isApproval,
            VisitorEntryuser_isApproval = e.VisitorEntryuser_isApproval,
            VisitorEntryUser_isReject = e.VisitorEntryUser_isReject,
            VisitorEntry_Remark = e.VisitorEntry_Remark,
            VisitorEntry_isCanteen = e.VisitorEntry_isCanteen,
            VisitorEntry_isStay = e.VisitorEntry_isStay,
            CreatedDate = e.CreatedDate,
            UpdatedDate = e.UpdatedDate
        };
    }
}
