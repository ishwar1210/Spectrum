using System.ComponentModel.DataAnnotations;

namespace Spectrum.DTOs;

public class UpdateVendorEmpDTO
{
    public int? VendorEmp_VendorID { get; set; }
    [StringLength(100)]
    public string? VendorEmp_Name { get; set; }
    [StringLength(50)]
    public string? VendorEmp_IDProofType { get; set; }
    [StringLength(50)]
    public string? VendorEmp_IDProofNo { get; set; }
    [StringLength(20)]
    public string? VendorEmp_mobile { get; set; }
    public int? VendorEmp_VenderAID { get; set; }
}
