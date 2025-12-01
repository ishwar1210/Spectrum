namespace Spectrum.DTOs;

public class VendorEmpResponseDTO
{
    public int VendorEmpId { get; set; }
    public int? VendorEmp_VendorID { get; set; }
    public string? VendorEmp_Name { get; set; }
    public string? VendorEmp_IDProofType { get; set; }
    public string? VendorEmp_IDProofNo { get; set; }
    public string? VendorEmp_mobile { get; set; }
    public int? VendorEmp_VenderAID { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime? UpdatedDate { get; set; }
}
