using System.ComponentModel.DataAnnotations;

namespace Spectrum.DTOs;

public class CreateApprovalStatusDTO
{
    [StringLength(50)]
    public string? ApprovalStatus_Gatepass { get; set; }
    public int? ApprovalStatus_VisitorEntryId { get; set; }
    public DateTime? ApprovalStatus_TransactionDate { get; set; }
    public DateTime? ApprovalStatus_ApprovalDate { get; set; }
    public bool? ApprovalStatus_ApprovalStatus { get; set; }
    [StringLength(100)]
    public string? ApprovalStatus_Remark { get; set; }
    public int? ApprovalStatus_ApprovalPersonRoleID { get; set; }
    public int? ApprovalStatus_UserId { get; set; }
}
