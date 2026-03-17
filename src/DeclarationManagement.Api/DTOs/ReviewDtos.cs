using DeclarationManagement.Api.Entities;

namespace DeclarationManagement.Api.DTOs;

public class ReviewActionRequestDto
{
    public long DeclarationId { get; set; }
    public ReviewStage ReviewStage { get; set; }
    public ReviewAction ReviewAction { get; set; }
    public string? Reason { get; set; }
    public ProjectLevel? RecognizedProjectLevel { get; set; }
    public AwardLevel? RecognizedAwardLevel { get; set; }
    public decimal? RecognizedAmount { get; set; }
    public string? Remark { get; set; }
}

public class ReviewRecordDto
{
    public long Id { get; set; }
    public long DeclarationId { get; set; }
    public ReviewStage ReviewStage { get; set; }
    public ReviewAction ReviewAction { get; set; }
    public string? Reason { get; set; }
    public ProjectLevel? RecognizedProjectLevel { get; set; }
    public AwardLevel? RecognizedAwardLevel { get; set; }
    public decimal? RecognizedAmount { get; set; }
    public string? Remark { get; set; }
    public long ReviewedByUserId { get; set; }
    public DateTime ReviewedAt { get; set; }
}

public class FlowLogDto
{
    public long Id { get; set; }
    public long DeclarationId { get; set; }
    public DeclarationStatus? FromStatus { get; set; }
    public DeclarationStatus ToStatus { get; set; }
    public FlowActionType ActionType { get; set; }
    public long OperatorUserId { get; set; }
    public string? Note { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class PendingReviewQueryDto
{
    public int PageIndex { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public string? ProjectName { get; set; }
    public List<long>? DepartmentIds { get; set; }
    public List<long>? CategoryIds { get; set; }
    public List<DeclarationStatus>? Statuses { get; set; }
}

public class PendingReviewItemDto
{
    public long DeclarationId { get; set; }
    public string ProjectName { get; set; } = string.Empty;
    public string ProjectCategoryName { get; set; } = string.Empty;
    public string DepartmentName { get; set; } = string.Empty;
    public DeclarationStatus CurrentStatus { get; set; }
    public ReviewStage CurrentReviewStage { get; set; }
    public DateTime? SubmittedAt { get; set; }
}
