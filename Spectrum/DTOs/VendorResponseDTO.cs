namespace Spectrum.DTOs;

public class VendorResponseDTO
{
    public int VendorID { get; set; }
    public string VendorCode { get; set; } = null!;
    public string VendorName { get; set; } = null!;
    public string? VendorMobile { get; set; }
    public string? IDProofType { get; set; }
    public string? IDProof { get; set; }
    public string? VendorAddress { get; set; }
    public string? Company { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime? UpdatedDate { get; set; }
    public bool IsActive { get; set; }
}
