namespace Spectrum.DTOs;

public class LoginResponseDTO
{
    public string Token { get; set; } = string.Empty;
    public int UserId { get; set; }
    public string Username { get; set; } = string.Empty;
    public string U_Name { get; set; } = string.Empty;
    public string? U_Email { get; set; }
    public int? U_RoleId { get; set; }
    public string? U_RoleName { get; set; }
    public int? U_DepartmentID { get; set; }
}
