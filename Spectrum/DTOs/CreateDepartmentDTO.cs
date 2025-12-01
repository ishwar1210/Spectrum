using System.ComponentModel.DataAnnotations;

namespace Spectrum.DTOs;

public class CreateDepartmentDTO
{
    [Required(ErrorMessage = "Department name is required")]
    [StringLength(100, ErrorMessage = "Department name cannot exceed 100 characters")]
    public string DepartmentName { get; set; } = string.Empty;
}