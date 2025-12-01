namespace Spectrum.DTOs;

public class DepartmentResponseDTO
{
    public int DepartmentId { get; set; }
    public string Department { get; set; } = string.Empty;
    public DateTime CreatedDate { get; set; }
    public DateTime? UpdatedDate { get; set; }
    public bool IsActive { get; set; }
}