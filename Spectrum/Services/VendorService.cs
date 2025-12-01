using Spectrum.DTOs;
using Spectrum.Models;
using Spectrum.Repositories;

namespace Spectrum.Services;

public class VendorService : IVendorService
{
    private readonly IVendorRepository _vendorRepository;

    public VendorService(IVendorRepository vendorRepository)
    {
        _vendorRepository = vendorRepository;
    }

    public async Task<(bool Success, string Message, VendorResponseDTO? Vendor)> GetByIdAsync(int vendorId)
    {
        var vendor = await _vendorRepository.GetByIdAsync(vendorId);
        if (vendor == null) return (false, "Vendor not found", null);
        return (true, "Vendor retrieved successfully", MapToResponseDTO(vendor));
    }

    public async Task<(bool Success, string Message, IEnumerable<VendorResponseDTO> Vendors)> GetAllAsync()
    {
        var vendors = await _vendorRepository.GetAllAsync();
        return (true, "Vendors retrieved successfully", vendors.Select(MapToResponseDTO));
    }

    public async Task<(bool Success, string Message, IEnumerable<VendorResponseDTO> Vendors)> GetActiveAsync()
    {
        var vendors = await _vendorRepository.GetActiveAsync();
        return (true, "Active vendors retrieved successfully", vendors.Select(MapToResponseDTO));
    }

    public async Task<(bool Success, string Message, int? VendorId)> CreateAsync(CreateVendorDTO createDto)
    {
        if (await _vendorRepository.VendorCodeExistsAsync(createDto.VendorCode))
            return (false, "Vendor code already exists", null);

        var vendor = new Vendor
        {
            VendorCode = createDto.VendorCode,
            VendorName = createDto.VendorName,
            VendorMobile = createDto.VendorMobile,
            IDProofType = createDto.IDProofType,
            IDProof = createDto.IDProof,
            VendorAddress = createDto.VendorAddress,
            Company = createDto.Company,
            IsActive = createDto.IsActive ?? true
        };

        var vendorId = await _vendorRepository.CreateAsync(vendor);
        return (true, "Vendor created successfully", vendorId);
    }

    public async Task<(bool Success, string Message, VendorResponseDTO? Vendor)> UpdateAsync(int vendorId, UpdateVendorDTO updateDto)
    {
        var existing = await _vendorRepository.GetByIdAsync(vendorId);
        if (existing == null) return (false, "Vendor not found", null);

        if (!string.IsNullOrWhiteSpace(updateDto.VendorCode) && await _vendorRepository.VendorCodeExistsAsync(updateDto.VendorCode, vendorId))
            return (false, "Vendor code already exists", null);

        if (!string.IsNullOrWhiteSpace(updateDto.VendorCode)) existing.VendorCode = updateDto.VendorCode;
        if (!string.IsNullOrWhiteSpace(updateDto.VendorName)) existing.VendorName = updateDto.VendorName;
        if (updateDto.VendorMobile != null) existing.VendorMobile = updateDto.VendorMobile;
        if (updateDto.IDProofType != null) existing.IDProofType = updateDto.IDProofType;
        if (updateDto.IDProof != null) existing.IDProof = updateDto.IDProof;
        if (updateDto.VendorAddress != null) existing.VendorAddress = updateDto.VendorAddress;
        if (updateDto.Company != null) existing.Company = updateDto.Company;
        if (updateDto.IsActive.HasValue) existing.IsActive = updateDto.IsActive.Value;

        var success = await _vendorRepository.UpdateAsync(vendorId, existing);
        if (!success) return (false, "Failed to update vendor", null);

        var updated = await _vendorRepository.GetByIdAsync(vendorId);
        return (true, "Vendor updated successfully", MapToResponseDTO(updated!));
    }

    public async Task<(bool Success, string Message)> DeleteAsync(int vendorId)
    {
        var existing = await _vendorRepository.GetByIdAsync(vendorId);
        if (existing == null) return (false, "Vendor not found");

        var success = await _vendorRepository.DeleteAsync(vendorId);
        if (!success) return (false, "Failed to delete vendor");

        return (true, "Vendor deleted successfully");
    }

    private static VendorResponseDTO MapToResponseDTO(Vendor vendor)
    {
        return new VendorResponseDTO
        {
            VendorID = vendor.VendorID,
            VendorCode = vendor.VendorCode,
            VendorName = vendor.VendorName,
            VendorMobile = vendor.VendorMobile,
            IDProofType = vendor.IDProofType,
            IDProof = vendor.IDProof,
            VendorAddress = vendor.VendorAddress,
            Company = vendor.Company,
            CreatedDate = vendor.CreatedDate,
            UpdatedDate = vendor.UpdatedDate,
            IsActive = vendor.IsActive
        };
    }
}
