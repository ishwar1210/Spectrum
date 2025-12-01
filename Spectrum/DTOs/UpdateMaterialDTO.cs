using System.ComponentModel.DataAnnotations;

namespace Spectrum.DTOs;

public class UpdateMaterialDTO
{
    [StringLength(200)]
    public string? Material_Name { get; set; }

    [StringLength(100)]
    public string? Material_Code { get; set; }

    [StringLength(50)]
    public string? Material_Status { get; set; }

    public int? Material_VisitorId { get; set; }
    public DateTime? Material_EntryDate { get; set; }
}
