namespace Spectrum.DTOs;

public class MaterialResponseDTO
{
    public int MaterialId { get; set; }
    public string? Material_Name { get; set; }
    public string? Material_Code { get; set; }
    public string? Material_Status { get; set; }
    public int? Material_VisitorId { get; set; }
    public DateTime? Material_EntryDate { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime? UpdatedDate { get; set; }
}
