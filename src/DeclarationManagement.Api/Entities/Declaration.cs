namespace DeclarationManagement.Api.Entities;

/// <summary>
/// 项目申报主表实体。
/// </summary>
public class Declaration : BaseEntity
{
    /// <summary>
    /// 任务ID属性。
    /// </summary>
    public long TaskId { get; set; }
    /// <summary>
    /// 申报人用户ID属性。
    /// </summary>
    public long ApplicantUserId { get; set; }
    /// <summary>
    /// 主体名称属性。
    /// </summary>
    public string PrincipalName { get; set; } = string.Empty;
    /// <summary>
    /// 联系方式电话属性。
    /// </summary>
    public string ContactPhone { get; set; } = string.Empty;
    /// <summary>
    /// 部门ID属性。
    /// </summary>
    public long DepartmentId { get; set; }
    /// <summary>
    /// 项目名称属性。
    /// </summary>
    public string ProjectName { get; set; } = string.Empty;
    /// <summary>
    /// 项目类别ID属性。
    /// </summary>
    public long ProjectCategoryId { get; set; }
    /// <summary>
    /// 项目等级属性。
    /// </summary>
    public ProjectLevel ProjectLevel { get; set; }
    /// <summary>
    /// 奖项等级属性。
    /// </summary>
    public AwardLevel AwardLevel { get; set; }
    /// <summary>
    /// 参与类型属性。
    /// </summary>
    public ParticipationType ParticipationType { get; set; }
    /// <summary>
    /// 认定批文文档名称属性。
    /// </summary>
    public string? ApprovalDocumentName { get; set; }
    /// <summary>
    /// 盖章单位And日期属性。
    /// </summary>
    public string? SealUnitAndDate { get; set; }
    /// <summary>
    /// 项目内容属性。
    /// </summary>
    public string? ProjectContent { get; set; }
    /// <summary>
    /// 项目成果属性。
    /// </summary>
    public string? ProjectAchievement { get; set; }
    /// <summary>
    /// 当前状态属性。
    /// </summary>
    public DeclarationStatus CurrentStatus { get; set; }
    /// <summary>
    /// 当前节点属性。
    /// </summary>
    public DeclarationNode CurrentNode { get; set; }
    /// <summary>
    /// 版本No属性。
    /// </summary>
    public int VersionNo { get; set; } = 1;
    /// <summary>
    /// 提交时间属性。
    /// </summary>
    public DateTime? SubmittedAt { get; set; }
    /// <summary>
    /// 更新时间时间属性。
    /// </summary>
    public DateTime? UpdatedAt { get; set; }

    /// <summary>
    /// 任务属性。
    /// </summary>
    public DeclarationTask? Task { get; set; }
    /// <summary>
    /// 申报人用户属性。
    /// </summary>
    public User? ApplicantUser { get; set; }
    /// <summary>
    /// 部门属性。
    /// </summary>
    public Department? Department { get; set; }
    /// <summary>
    /// 项目类别属性。
    /// </summary>
    public ProjectCategory? ProjectCategory { get; set; }
    /// <summary>
    /// Attachments属性。
    /// </summary>
    public ICollection<DeclarationAttachment> Attachments { get; set; } = new List<DeclarationAttachment>();
    /// <summary>
    /// 审核Records属性。
    /// </summary>
    public ICollection<DeclarationReviewRecord> ReviewRecords { get; set; } = new List<DeclarationReviewRecord>();
    /// <summary>
    /// 流转Logs属性。
    /// </summary>
    public ICollection<DeclarationFlowLog> FlowLogs { get; set; } = new List<DeclarationFlowLog>();
}
