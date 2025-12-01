namespace Spectrum.DTOs;

public class VisitorEntryResponseDTO
{
    public int VisitorEntryID { get; set; }
    public int? VisitorEntry_visitorId { get; set; }
    public string? VisitorEntry_Gatepass { get; set; }
    public string? VisitorEntry_Vehicletype { get; set; }
    public string? VisitorEntry_Vehicleno { get; set; }
    public DateTime? VisitorEntry_Date { get; set; }
    public DateTime? VisitorEntry_Intime { get; set; }
    public DateTime? VisitorEntry_Outtime { get; set; }
    public int? VisitorEntry_Userid { get; set; }
    public bool VisitorEntry_isApproval { get; set; }
    public string? VisitorEntry_Remark { get; set; }
    public bool VisitorEntry_isCanteen { get; set; }
    public bool VisitorEntry_isStay { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime? UpdatedDate { get; set; }
}
