using System.ComponentModel.DataAnnotations;

namespace Spectrum.DTOs;

public class UpdateVendorAppointmentDTO
{
    public int? VendorA_VendorID { get; set; }
    [StringLength(100)]
    public string? VendorA_Getpass { get; set; }
    public DateTime? VendorA_FromDate { get; set; }
    public DateTime? VendorA_ToDate { get; set; }
    [StringLength(50)]
    public string? VendorA_VehicleNO { get; set; }
    [StringLength(50)]
    public string? VendorA_IdProofType { get; set; }
    [StringLength(50)]
    public string? VendorA_IdProofNo { get; set; }
    public int? VendorA_UserId { get; set; }
}
