namespace Spectrum.Models;

public class Department
{
    public int DepartmentId { get; set; }
    public string DepartmentName { get; set; } = null!;
    public DateTime CreatedDate { get; set; }
    public DateTime? UpdatedDate { get; set; }
    public bool IsActive { get; set; }
}