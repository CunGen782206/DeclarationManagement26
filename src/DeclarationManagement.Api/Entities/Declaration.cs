namespace DeclarationManagement.Api.Entities;

/// <summary>
/// 项目申报主表实体。
/// </summary>
public class Declaration : BaseEntity
{
    /// <summary>
    /// TaskId 属性。
    /// </summary>
    public long TaskId { get; set; }
    /// <summary>
    /// ApplicantUserId 属性。
    /// </summary>
    public long ApplicantUserId { get; set; }
    /// <summary>
    /// PrincipalName 属性。
    /// </summary>
    public string PrincipalName { get; set; } = string.Empty;
    /// <summary>
    /// ContactPhone 属性。
    /// </summary>
    public string ContactPhone { get; set; } = string.Empty;
    /// <summary>
    /// DepartmentId 属性。
    /// </summary>
    public long DepartmentId { get; set; }
    /// <summary>
    /// ProjectName 属性。
    /// </summary>
    public string ProjectName { get; set; } = string.Empty;
    /// <summary>
    /// ProjectCategoryId 属性。
    /// </summary>
    public long ProjectCategoryId { get; set; }
    /// <summary>
    /// ProjectLevel 属性。
    /// </summary>
    public ProjectLevel ProjectLevel { get; set; }
    /// <summary>
    /// AwardLevel 属性。
    /// </summary>
    public AwardLevel AwardLevel { get; set; }
    /// <summary>
    /// ParticipationType 属性。
    /// </summary>
    public ParticipationType ParticipationType { get; set; }
    /// <summary>
    /// ApprovalDocumentName 属性。
    /// </summary>
    public string? ApprovalDocumentName { get; set; }
    /// <summary>
    /// SealUnitAndDate 属性。
    /// </summary>
    public string? SealUnitAndDate { get; set; }
    /// <summary>
    /// ProjectContent 属性。
    /// </summary>
    public string? ProjectContent { get; set; }
    /// <summary>
    /// ProjectAchievement 属性。
    /// </summary>
    public string? ProjectAchievement { get; set; }
    /// <summary>
    /// CurrentStatus 属性。
    /// </summary>
    public DeclarationStatus CurrentStatus { get; set; }
    /// <summary>
    /// CurrentNode 属性。
    /// </summary>
    public DeclarationNode CurrentNode { get; set; }
    /// <summary>
    /// VersionNo 属性。
    /// </summary>
    public int VersionNo { get; set; } = 1;
    /// <summary>
    /// SubmittedAt 属性。
    /// </summary>
    public DateTime? SubmittedAt { get; set; }
    /// <summary>
    /// UpdatedAt 属性。
    /// </summary>
    public DateTime? UpdatedAt { get; set; }

    /// <summary>
    /// Task 属性。
    /// </summary>
    public DeclarationTask? Task { get; set; }
    /// <summary>
    /// ApplicantUser 属性。
    /// </summary>
    public User? ApplicantUser { get; set; }
    /// <summary>
    /// Department 属性。
    /// </summary>
    public Department? Department { get; set; }
    /// <summary>
    /// ProjectCategory 属性。
    /// </summary>
    public ProjectCategory? ProjectCategory { get; set; }
    /// <summary>
    /// Attachments 属性。
    /// </summary>
    public ICollection<DeclarationAttachment> Attachments { get; set; } = new List<DeclarationAttachment>();
    /// <summary>
    /// ReviewRecords 属性。
    /// </summary>
    public ICollection<DeclarationReviewRecord> ReviewRecords { get; set; } = new List<DeclarationReviewRecord>();
    /// <summary>
    /// FlowLogs 属性。
    /// </summary>
    public ICollection<DeclarationFlowLog> FlowLogs { get; set; } = new List<DeclarationFlowLog>();
}
