using System.ComponentModel.DataAnnotations;

namespace Spectrum.DTOs;

public class UpdateLocationDTO
{
    [StringLength(100, ErrorMessage = "Location name cannot exceed 100 characters")]
    public string? LocationName { get; set; }

    [StringLength(100, ErrorMessage = "Description cannot exceed 100 characters")]
    public string? Description { get; set; }

    public bool? IsActive { get; set; }
}
