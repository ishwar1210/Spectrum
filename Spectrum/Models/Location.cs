namespace Spectrum.Models;

public class Location
{
    public int LocationId { get; set; }
    public string LocationName { get; set; } = null!;
    public string? Description { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime? UpdatedDate { get; set; }
    public bool IsActive { get; set; }
    public int? CreatedBy { get; set; }
}
