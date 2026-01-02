using System.ComponentModel.DataAnnotations;

namespace Spectrum.DTOs;

public class UpdateVisitorEntryDTO
{
    public int? VisitorEntry_visitorId { get; set; }
    [StringLength(40)]
    public string? VisitorEntry_Gatepass { get; set; }
    [StringLength(50)]
    public string? VisitorEntry_Vehicletype { get; set; }
    [StringLength(50)]
    public string? VisitorEntry_Vehicleno { get; set; }
    [StringLength(100)]
    public string? VisitorEntry_Purposeofvisit { get; set; }
    public DateTime? VisitorEntry_Date { get; set; }
    public DateTime? VisitorEntry_Intime { get; set; }
    public DateTime? VisitorEntry_Outtime { get; set; }
    public TimeSpan? MeetingTimeFrom { get; set; }
    public TimeSpan? MeetingTimeTo { get; set; }
    public int? VisitorEntry_Userid { get; set; }
    public bool? VisitorEntryAdmin_isApproval { get; set; }
    public bool? VisitorEntryuser_isApproval { get; set; }
    public bool? VisitorEntryUser_isReject { get; set; }
    public string? VisitorEntry_Remark { get; set; }
    public bool? VisitorEntry_isCanteen { get; set; }
    public bool? VisitorEntry_isStay { get; set; }
}
