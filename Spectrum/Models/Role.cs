namespace Spectrum.Models;

public class Role
{
    public int RoleId { get; set; }
    public string RoleName { get; set; } = null!;
    public DateTime CreatedDate { get; set; }
    public DateTime? UpdatedDate { get; set; }
}