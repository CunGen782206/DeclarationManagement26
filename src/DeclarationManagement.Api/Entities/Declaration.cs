namespace DeclarationManagement.Api.Entities;

/// <summary>
/// 项目申报主表实体。
/// </summary>
public class Declaration : BaseEntity
{
    public long TaskId { get; set; }
    public long ApplicantUserId { get; set; }
    public string PrincipalName { get; set; } = string.Empty;
    public string ContactPhone { get; set; } = string.Empty;
    public long DepartmentId { get; set; }
    public string ProjectName { get; set; } = string.Empty;
    public long ProjectCategoryId { get; set; }
    public ProjectLevel ProjectLevel { get; set; }
    public AwardLevel AwardLevel { get; set; }
    public ParticipationType ParticipationType { get; set; }
    public string? ApprovalDocumentName { get; set; }
    public string? SealUnitAndDate { get; set; }
    public string? ProjectContent { get; set; }
    public string? ProjectAchievement { get; set; }
    public DeclarationStatus CurrentStatus { get; set; }
    public DeclarationNode CurrentNode { get; set; }
    public int VersionNo { get; set; } = 1;
    public DateTime? SubmittedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }

    public DeclarationTask? Task { get; set; }
    public User? ApplicantUser { get; set; }
    public Department? Department { get; set; }
    public ProjectCategory? ProjectCategory { get; set; }
    public ICollection<DeclarationAttachment> Attachments { get; set; } = new List<DeclarationAttachment>();
    public ICollection<DeclarationReviewRecord> ReviewRecords { get; set; } = new List<DeclarationReviewRecord>();
    public ICollection<DeclarationFlowLog> FlowLogs { get; set; } = new List<DeclarationFlowLog>();
}
