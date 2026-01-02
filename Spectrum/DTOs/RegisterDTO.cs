using System.ComponentModel.DataAnnotations;

namespace Spectrum.DTOs;

public class RegisterDTO
{
    [Required]
    [StringLength(100)]
    public string Username { get; set; } = string.Empty;

    [Required]
    [StringLength(100, MinimumLength = 6)]
    public string Password { get; set; } = string.Empty;

    [Required]
    [StringLength(100)]
    public string U_Name { get; set; } = string.Empty;

    [StringLength(20)]
    public string? U_Mobile { get; set; }

    [EmailAddress]
    [StringLength(200)]
    public string? U_Email { get; set; }

    [StringLength(400)]
    public string? U_Address { get; set; }

    public int? U_RoleId { get; set; }
    public int? U_DepartmentID { get; set; }
    public int? U_ReportingToId { get; set; }
}
