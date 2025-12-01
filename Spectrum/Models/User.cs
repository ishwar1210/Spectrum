namespace Spectrum.Models;

public class User
{
    public int UserId { get; set; }
    public string Username { get; set; } = string.Empty;
    public string Password_hash { get; set; } = string.Empty;
    public string U_Name { get; set; } = string.Empty;
    public string? U_Mobile { get; set; }
    public string? U_Email { get; set; }
    public string? U_Address { get; set; }
    public int? U_RoleId { get; set; }
    public int? U_DepartmentID { get; set; }
    public int? U_ReportingToId { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime? UpdatedDate { get; set; }
}
