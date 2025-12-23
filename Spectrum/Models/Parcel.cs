namespace Spectrum.Models;

public class Parcel
{
    public int ParcelId { get; set; }
    public string ParcelBarcode { get; set; } = null!;
    public string ParcelCompanyName { get; set; } = null!;
    public int UserId { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime? UpdatedDate { get; set; }
    public bool IsActive { get; set; }
    public bool? ParcelHandover { get; set; }
    public string Parcel_type { get; set; } = "Personal";
}
