using System.ComponentModel.DataAnnotations;

namespace Spectrum.DTOs;

public class UpdateAminitiesDTO
{
    [StringLength(200)]
    public string? Aminities_Name { get; set; }

    public bool? Aminities_isActive { get; set; }
}
