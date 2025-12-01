namespace Spectrum.DTOs;

public class RoleResponseDTO
{
    public int RoleId { get; set; }
    public string RoleName { get; set; } = null!;
    public DateTime CreatedDate { get; set; }
    public DateTime? UpdatedDate { get; set; }
}   