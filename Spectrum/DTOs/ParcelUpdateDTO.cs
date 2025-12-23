using System.ComponentModel.DataAnnotations;

namespace Spectrum.DTOs;

public class ParcelUpdateDTO
{
    [Required]
    [StringLength(100)]
    public string ParcelBarcode { get; set; } = null!;

    [Required]
    [StringLength(150)]
    public string ParcelCompanyName { get; set; } = null!;

    [Required]
    public int UserId { get; set; }

    public bool? IsActive { get; set; }

    public bool? ParcelHandover { get; set; }

    [StringLength(20)]
    public string? Parcel_type { get; set; }
}
