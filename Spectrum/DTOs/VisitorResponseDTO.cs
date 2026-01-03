namespace Spectrum.DTOs;

public class VisitorResponseDTO
{
    public int VisitorId { get; set; }
    public string? Visitor_Name { get; set; }
    public string? Visitor_mobile { get; set; }
    public string? Visitor_Address { get; set; }
    public string? Visitor_CompanyName { get; set; }
    public string? Visitor_Idprooftype { get; set; }
    public string? Visitor_idproofno { get; set; }
    public string? Visitor_Fingerprint1 { get; set; }
    public string? Visitor_Fingerprint2 { get; set; }
    public string? Visitor_Carrymateriallist { get; set; }
    public string? Visitor_Materialbarcode { get; set; }
    public string? Visitor_OTP { get; set; }
    public int? Visitor_Onofvisit { get; set; }
    public string? Visitor_image { get; set; }
    public bool Visitor_isApproval { get; set; }
    public bool Visitor_isBlock { get; set; }
    public string? Visitor_Blockreason { get; set; }
    public string? Visitor_Unblockreason { get; set; }
    public string? Visitor_Email { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime? UpdatedDate { get; set; }
}
