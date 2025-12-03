namespace Spectrum.DTOs;

public class AminitiesResponseDTO
{
    public int AminitiesId { get; set; }
    public string? Aminities_Name { get; set; }
    public bool Aminities_isActive { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime? UpdatedDate { get; set; }
}
