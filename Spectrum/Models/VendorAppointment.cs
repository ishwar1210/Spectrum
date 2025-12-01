namespace Spectrum.Models;

public class VendorAppointment
{
    public int VendorA_Id { get; set; }
    public int? VendorA_VendorID { get; set; }
    public string? VendorA_Getpass { get; set; }
    public DateTime? VendorA_FromDate { get; set; }
    public DateTime? VendorA_ToDate { get; set; }
    public string? VendorA_VehicleNO { get; set; }
    public string? VendorA_IdProofType { get; set; }
    public string? VendorA_IdProofNo { get; set; }
    public int? VendorA_UserId { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime? UpdatedDate { get; set; }
}
