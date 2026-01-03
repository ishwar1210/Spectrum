using System.ComponentModel.DataAnnotations;

namespace Spectrum.DTOs;

public class UpdateDepartmentDTO
{
    [StringLength(100, ErrorMessage = "Department name cannot exceed 100 characters")]
    public string? DepartmentName { get; set; }
    
    public bool? IsActive { get; set; }
}