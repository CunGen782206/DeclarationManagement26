using DeclarationManagement.Api.Entities;

namespace DeclarationManagement.Api.DTOs;

/// <summary>
/// SaveDeclarationRequestDto 类。
/// </summary>
public class SaveDeclarationRequestDto
{
    /// <summary>
    /// TaskId 属性。
    /// </summary>
    public long TaskId { get; set; }
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
}

/// <summary>
/// DeclarationSubmitRequestDto 类。
/// </summary>
public class DeclarationSubmitRequestDto
{
    /// <summary>
    /// DeclarationId 属性。
    /// </summary>
    public long DeclarationId { get; set; }
}

/// <summary>
/// DeclarationResubmitRequestDto 类。
/// </summary>
public class DeclarationResubmitRequestDto
{
    /// <summary>
    /// DeclarationId 属性。
    /// </summary>
    public long DeclarationId { get; set; }
}

/// <summary>
/// DeclarationDetailDto 类。
/// </summary>
public class DeclarationDetailDto
{
    /// <summary>
    /// Id 属性。
    /// </summary>
    public long Id { get; set; }
    /// <summary>
    /// TaskId 属性。
    /// </summary>
    public long TaskId { get; set; }
    /// <summary>
    /// TaskName 属性。
    /// </summary>
    public string TaskName { get; set; } = string.Empty;
    /// <summary>
    /// ApplicantName 属性。
    /// </summary>
    public string ApplicantName { get; set; } = string.Empty;
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
    /// DepartmentName 属性。
    /// </summary>
    public string DepartmentName { get; set; } = string.Empty;
    /// <summary>
    /// ProjectCategoryId 属性。
    /// </summary>
    public long ProjectCategoryId { get; set; }
    /// <summary>
    /// ProjectCategoryName 属性。
    /// </summary>
    public string ProjectCategoryName { get; set; } = string.Empty;
    /// <summary>
    /// ProjectName 属性。
    /// </summary>
    public string ProjectName { get; set; } = string.Empty;
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
    public int VersionNo { get; set; }
    /// <summary>
    /// SubmittedAt 属性。
    /// </summary>
    public DateTime? SubmittedAt { get; set; }
}

/// <summary>
/// DeclarationPageQueryDto 类。
/// </summary>
public class DeclarationPageQueryDto
{
    /// <summary>
    /// PageIndex 属性。
    /// </summary>
    public int PageIndex { get; set; } = 1;
    /// <summary>
    /// PageSize 属性。
    /// </summary>
    public int PageSize { get; set; } = 10;
    /// <summary>
    /// StartDate 属性。
    /// </summary>
    public DateTime? StartDate { get; set; }
    /// <summary>
    /// EndDate 属性。
    /// </summary>
    public DateTime? EndDate { get; set; }
    /// <summary>
    /// DepartmentIds 属性。
    /// </summary>
    public List<long>? DepartmentIds { get; set; }
    /// <summary>
    /// CategoryIds 属性。
    /// </summary>
    public List<long>? CategoryIds { get; set; }
    /// <summary>
    /// Statuses 属性。
    /// </summary>
    public List<DeclarationStatus>? Statuses { get; set; }
}

/// <summary>
/// DeclarationListItemDto 类。
/// </summary>
public class DeclarationListItemDto
{
    /// <summary>
    /// Id 属性。
    /// </summary>
    public long Id { get; set; }
    /// <summary>
    /// ProjectName 属性。
    /// </summary>
    public string ProjectName { get; set; } = string.Empty;
    /// <summary>
    /// ProjectCategoryName 属性。
    /// </summary>
    public string ProjectCategoryName { get; set; } = string.Empty;
    /// <summary>
    /// DepartmentName 属性。
    /// </summary>
    public string DepartmentName { get; set; } = string.Empty;
    /// <summary>
    /// PrincipalName 属性。
    /// </summary>
    public string PrincipalName { get; set; } = string.Empty;
    /// <summary>
    /// CurrentStatus 属性。
    /// </summary>
    public DeclarationStatus CurrentStatus { get; set; }
    /// <summary>
    /// SubmittedAt 属性。
    /// </summary>
    public DateTime? SubmittedAt { get; set; }
    /// <summary>
    /// Action 属性。
    /// </summary>
    public string Action { get; set; } = string.Empty;
}

/// <summary>
/// AttachmentDto 类。
/// </summary>
public class AttachmentDto
{
    /// <summary>
    /// Id 属性。
    /// </summary>
    public long Id { get; set; }
    /// <summary>
    /// OriginalFileName 属性。
    /// </summary>
    public string OriginalFileName { get; set; } = string.Empty;
    /// <summary>
    /// FileSizeBytes 属性。
    /// </summary>
    public long FileSizeBytes { get; set; }
    /// <summary>
    /// UploadedAt 属性。
    /// </summary>
    public DateTime UploadedAt { get; set; }
}

/// <summary>
/// PagedResultDto 类。
/// </summary>
public class PagedResultDto<T>
{
    /// <summary>
    /// PageIndex 属性。
    /// </summary>
    public int PageIndex { get; set; }
    /// <summary>
    /// PageSize 属性。
    /// </summary>
    public int PageSize { get; set; }
    /// <summary>
    /// TotalCount 属性。
    /// </summary>
    public long TotalCount { get; set; }
    /// <summary>
    /// Items 属性。
    /// </summary>
    public List<T> Items { get; set; } = new();
}
