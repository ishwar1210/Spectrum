using Spectrum.DTOs;
using Spectrum.Models;
using Spectrum.Repositories;

namespace Spectrum.Services;

public class VisitorService : IVisitorService
{
    private readonly IVisitorRepository _repo;

    public VisitorService(IVisitorRepository repo)
    {
        _repo = repo;
    }

    public async Task<(bool Success, string Message, VisitorResponseDTO? Visitor)> GetByIdAsync(int id)
    {
        var v = await _repo.GetByIdAsync(id);
        if (v == null) return (false, "Visitor not found", null);
        return (true, "Visitor retrieved successfully", Map(v));
    }

    public async Task<(bool Success, string Message, IEnumerable<VisitorResponseDTO> Visitors)> GetAllAsync()
    {
        var items = await _repo.GetAllAsync();
        return (true, "Visitors retrieved successfully", items.Select(Map));
    }

    public async Task<(bool Success, string Message, int? VisitorId)> CreateAsync(CreateVisitorDTO createDto)
    {
        var visitor = new Visitor
        {
            Visitor_Name = createDto.Visitor_Name,
            Visitor_mobile = createDto.Visitor_mobile,
            Visitor_Address = createDto.Visitor_Address,
            Visitor_CompanyName = createDto.Visitor_CompanyName,
            Visitor_Purposeofvisit = createDto.Visitor_Purposeofvisit,
            Visitor_Idprooftype = createDto.Visitor_Idprooftype,
            Visitor_idproofno = createDto.Visitor_idproofno,
            Visitor_Fingerprint1 = createDto.Visitor_Fingerprint1,
            Visitor_Fingerprint2 = createDto.Visitor_Fingerprint2,
            Visitor_Carrymateriallist = createDto.Visitor_Carrymateriallist,
            Visitor_Materialbarcode = createDto.Visitor_Materialbarcode,
            Visitor_OTP = createDto.Visitor_OTP,
            Visitor_Onofvisit = createDto.Visitor_Onofvisit,
            Visitor_image = createDto.Visitor_image,
            Visitor_MeetingDate = createDto.Visitor_MeetingDate,
            Visitor_isApproval = createDto.Visitor_isApproval ?? true,
            Visitor_isBlock = createDto.Visitor_isBlock ?? false,
            Visitor_Blockreason = createDto.Visitor_Blockreason,
            Visitor_Unblockreason = createDto.Visitor_Unblockreason
        };

        var id = await _repo.CreateAsync(visitor);
        return (true, "Visitor created successfully", id);
    }

    public async Task<(bool Success, string Message, VisitorResponseDTO? Visitor)> UpdateAsync(int id, UpdateVisitorDTO updateDto)
    {
        var existing = await _repo.GetByIdAsync(id);
        if (existing == null) return (false, "Visitor not found", null);

        if (updateDto.Visitor_Name != null) existing.Visitor_Name = updateDto.Visitor_Name;
        if (updateDto.Visitor_mobile != null) existing.Visitor_mobile = updateDto.Visitor_mobile;
        if (updateDto.Visitor_Address != null) existing.Visitor_Address = updateDto.Visitor_Address;
        if (updateDto.Visitor_CompanyName != null) existing.Visitor_CompanyName = updateDto.Visitor_CompanyName;
        if (updateDto.Visitor_Purposeofvisit != null) existing.Visitor_Purposeofvisit = updateDto.Visitor_Purposeofvisit;
        if (updateDto.Visitor_Idprooftype != null) existing.Visitor_Idprooftype = updateDto.Visitor_Idprooftype;
        if (updateDto.Visitor_idproofno != null) existing.Visitor_idproofno = updateDto.Visitor_idproofno;
        if (updateDto.Visitor_Fingerprint1 != null) existing.Visitor_Fingerprint1 = updateDto.Visitor_Fingerprint1;
        if (updateDto.Visitor_Fingerprint2 != null) existing.Visitor_Fingerprint2 = updateDto.Visitor_Fingerprint2;
        if (updateDto.Visitor_Carrymateriallist != null) existing.Visitor_Carrymateriallist = updateDto.Visitor_Carrymateriallist;
        if (updateDto.Visitor_Materialbarcode != null) existing.Visitor_Materialbarcode = updateDto.Visitor_Materialbarcode;
        if (updateDto.Visitor_OTP != null) existing.Visitor_OTP = updateDto.Visitor_OTP;
        if (updateDto.Visitor_Onofvisit.HasValue) existing.Visitor_Onofvisit = updateDto.Visitor_Onofvisit.Value;
        if (updateDto.Visitor_image != null) existing.Visitor_image = updateDto.Visitor_image;
        if (updateDto.Visitor_MeetingDate.HasValue) existing.Visitor_MeetingDate = updateDto.Visitor_MeetingDate.Value;
        if (updateDto.Visitor_isApproval.HasValue) existing.Visitor_isApproval = updateDto.Visitor_isApproval.Value;
        if (updateDto.Visitor_isBlock.HasValue) existing.Visitor_isBlock = updateDto.Visitor_isBlock.Value;
        if (updateDto.Visitor_Blockreason != null) existing.Visitor_Blockreason = updateDto.Visitor_Blockreason;
        if (updateDto.Visitor_Unblockreason != null) existing.Visitor_Unblockreason = updateDto.Visitor_Unblockreason;

        var success = await _repo.UpdateAsync(id, existing);
        if (!success) return (false, "Failed to update visitor", null);

        var updated = await _repo.GetByIdAsync(id);
        return (true, "Visitor updated successfully", Map(updated!));
    }

    public async Task<(bool Success, string Message)> DeleteAsync(int id)
    {
        var existing = await _repo.GetByIdAsync(id);
        if (existing == null) return (false, "Visitor not found");

        var success = await _repo.DeleteAsync(id);
        if (!success) return (false, "Failed to delete visitor");

        return (true, "Visitor deleted successfully");
    }

    private static VisitorResponseDTO Map(Visitor v)
    {
        return new VisitorResponseDTO
        {
            VisitorId = v.VisitorId,
            Visitor_Name = v.Visitor_Name,
            Visitor_mobile = v.Visitor_mobile,
            Visitor_Address = v.Visitor_Address,
            Visitor_CompanyName = v.Visitor_CompanyName,
            Visitor_Purposeofvisit = v.Visitor_Purposeofvisit,
            Visitor_Idprooftype = v.Visitor_Idprooftype,
            Visitor_idproofno = v.Visitor_idproofno,
            Visitor_Fingerprint1 = v.Visitor_Fingerprint1,
            Visitor_Fingerprint2 = v.Visitor_Fingerprint2,
            Visitor_Carrymateriallist = v.Visitor_Carrymateriallist,
            Visitor_Materialbarcode = v.Visitor_Materialbarcode,
            Visitor_OTP = v.Visitor_OTP,
            Visitor_Onofvisit = v.Visitor_Onofvisit,
            Visitor_image = v.Visitor_image,
            Visitor_MeetingDate = v.Visitor_MeetingDate,
            Visitor_isApproval = v.Visitor_isApproval,
            Visitor_isBlock = v.Visitor_isBlock,
            Visitor_Blockreason = v.Visitor_Blockreason,
            Visitor_Unblockreason = v.Visitor_Unblockreason,
            CreatedDate = v.CreatedDate,
            UpdatedDate = v.UpdatedDate
        };
    }
}
