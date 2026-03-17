using DeclarationManagement.Api.Entities;

namespace DeclarationManagement.Api.DTOs;

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

public class DeclarationSubmitRequestDto
{
    public long DeclarationId { get; set; }
}

public class DeclarationResubmitRequestDto
{
    public long DeclarationId { get; set; }
}

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
    public string? ApprovalDocumentName { get; set; }
    public string? SealUnitAndDate { get; set; }
    public string? ProjectContent { get; set; }
    public string? ProjectAchievement { get; set; }
    public DeclarationStatus CurrentStatus { get; set; }
    public DeclarationNode CurrentNode { get; set; }
    public int VersionNo { get; set; }
    public DateTime? SubmittedAt { get; set; }
}

public class DeclarationPageQueryDto
{
    public int PageIndex { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public List<long>? DepartmentIds { get; set; }
    public List<long>? CategoryIds { get; set; }
    public List<DeclarationStatus>? Statuses { get; set; }
}

public class DeclarationListItemDto
{
    public long Id { get; set; }
    public string ProjectName { get; set; } = string.Empty;
    public string ProjectCategoryName { get; set; } = string.Empty;
    public string DepartmentName { get; set; } = string.Empty;
    public ProjectLevel ProjectLevel { get; set; }
    public AwardLevel AwardLevel { get; set; }
    public ParticipationType ParticipationType { get; set; }
    public string PrincipalName { get; set; } = string.Empty;
    public DeclarationStatus CurrentStatus { get; set; }
    public DateTime? SubmittedAt { get; set; }
    public string TaskName { get; set; } = string.Empty;
    public string Action { get; set; } = string.Empty;
}

public class AttachmentDto
{
    public long Id { get; set; }
    public string OriginalFileName { get; set; } = string.Empty;
    public long FileSizeBytes { get; set; }
    public DateTime UploadedAt { get; set; }
}

public class PagedResultDto<T>
{
    public int PageIndex { get; set; }
    public int PageSize { get; set; }
    public long TotalCount { get; set; }
    public List<T> Items { get; set; } = new();
}
