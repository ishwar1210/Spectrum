namespace Spectrum.Models;

public class ApprovalStatus
{
    public int ApprovalStatusId { get; set; }
    public string? ApprovalStatus_Gatepass { get; set; }
    public int? ApprovalStatus_VisitorEntryId { get; set; }
    public DateTime? ApprovalStatus_TransactionDate { get; set; }
    public DateTime? ApprovalStatus_ApprovalDate { get; set; }
    public bool ApprovalStatus_ApprovalStatus { get; set; }
    public string? ApprovalStatus_Remark { get; set; }
    public int? ApprovalStatus_ApprovalPersonRoleID { get; set; }
    public int? ApprovalStatus_UserId { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime? UpdatedDate { get; set; }
}
