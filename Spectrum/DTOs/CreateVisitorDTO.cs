using System.ComponentModel.DataAnnotations;

namespace Spectrum.DTOs;

public class CreateVisitorDTO
{
    [StringLength(100)]
    public string? Visitor_Name { get; set; }

    [StringLength(20)]
    public string? Visitor_mobile { get; set; }

    [StringLength(100)]
    public string? Visitor_Address { get; set; }

    [StringLength(100)]
    public string? Visitor_CompanyName { get; set; }

    [StringLength(200)]
    public string? Visitor_Purposeofvisit { get; set; }

    [StringLength(50)]
    public string? Visitor_Idprooftype { get; set; }

    [StringLength(50)]
    public string? Visitor_idproofno { get; set; }

    public string? Visitor_Fingerprint1 { get; set; }
    public string? Visitor_Fingerprint2 { get; set; }
    public string? Visitor_Carrymateriallist { get; set; }
    public string? Visitor_Materialbarcode { get; set; }
    public string? Visitor_OTP { get; set; }
    public int? Visitor_Onofvisit { get; set; }
    public string? Visitor_image { get; set; }
    public DateTime? Visitor_MeetingDate { get; set; }
    public bool? Visitor_isApproval { get; set; }
    public bool? Visitor_isBlock { get; set; }
    public string? Visitor_Blockreason { get; set; }
    public string? Visitor_Unblockreason { get; set; }
}
