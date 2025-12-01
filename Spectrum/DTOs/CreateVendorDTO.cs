using System.ComponentModel.DataAnnotations;

namespace Spectrum.DTOs;

public class CreateVendorDTO
{
    [Required]
    [StringLength(10)]
    public string VendorCode { get; set; } = null!;

    [Required]
    [StringLength(100)]
    public string VendorName { get; set; } = null!;

    [StringLength(20)]
    public string? VendorMobile { get; set; }

    [StringLength(100)]
    public string? IDProofType { get; set; }

    [StringLength(100)]
    public string? IDProof { get; set; }

    [StringLength(100)]
    public string? VendorAddress { get; set; }

    [StringLength(100)]
    public string? Company { get; set; }

    public bool? IsActive { get; set; }
}
