using DeclarationManagement.Api.Entities;

namespace DeclarationManagement.Api.DTOs;

/// <summary>
/// 新建/修改申报单 DTO。
/// </summary>
public class SaveDeclarationRequestDto
{
    public long TaskId { get; set; }
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
}

/// <summary>
/// 申报单详情 DTO（返回给前端）。
/// </summary>
public class DeclarationDetailDto
{
    public long Id { get; set; }
    public long TaskId { get; set; }
    public string TaskName { get; set; } = string.Empty;
    public string ApplicantName { get; set; } = string.Empty;
    public string PrincipalName { get; set; } = string.Empty;
    public string ContactPhone { get; set; } = string.Empty;
    public long DepartmentId { get; set; }
    public string DepartmentName { get; set; } = string.Empty;
    public long ProjectCategoryId { get; set; }
    public string ProjectCategoryName { get; set; } = string.Empty;
    public string ProjectName { get; set; } = string.Empty;
    public ProjectLevel ProjectLevel { get; set; }
    public AwardLevel AwardLevel { get; set; }
    public ParticipationType ParticipationType { get; set; }
    public DeclarationStatus CurrentStatus { get; set; }
    public DeclarationNode CurrentNode { get; set; }
    public int VersionNo { get; set; }
    public DateTime? SubmittedAt { get; set; }
}
