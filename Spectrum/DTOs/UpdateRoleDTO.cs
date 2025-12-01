using System.ComponentModel.DataAnnotations;

namespace Spectrum.DTOs;

public class UpdateRoleDTO
{
    [StringLength(100, ErrorMessage = "Role name cannot exceed 100 characters")]
    public string? RoleName { get; set; }
}