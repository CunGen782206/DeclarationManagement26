using DeclarationManagement.Api.Entities;

namespace DeclarationManagement.Api.DTOs;

/// <summary>
/// ReviewActionRequestDto 类。
/// </summary>
public class ReviewActionRequestDto
{
    /// <summary>
    /// DeclarationId 属性。
    /// </summary>
    public long DeclarationId { get; set; }
    /// <summary>
    /// ReviewStage 属性。
    /// </summary>
    public ReviewStage ReviewStage { get; set; }
    /// <summary>
    /// ReviewAction 属性。
    /// </summary>
    public ReviewAction ReviewAction { get; set; }
    /// <summary>
    /// Reason 属性。
    /// </summary>
    public string? Reason { get; set; }
    /// <summary>
    /// RecognizedProjectLevel 属性。
    /// </summary>
    public ProjectLevel? RecognizedProjectLevel { get; set; }
    /// <summary>
    /// RecognizedAwardLevel 属性。
    /// </summary>
    public AwardLevel? RecognizedAwardLevel { get; set; }
    /// <summary>
    /// RecognizedAmount 属性。
    /// </summary>
    public decimal? RecognizedAmount { get; set; }
    /// <summary>
    /// Remark 属性。
    /// </summary>
    public string? Remark { get; set; }
}

/// <summary>
/// ReviewRecordDto 类。
/// </summary>
public class ReviewRecordDto
{
    /// <summary>
    /// Id 属性。
    /// </summary>
    public long Id { get; set; }
    /// <summary>
    /// DeclarationId 属性。
    /// </summary>
    public long DeclarationId { get; set; }
    /// <summary>
    /// ReviewStage 属性。
    /// </summary>
    public ReviewStage ReviewStage { get; set; }
    /// <summary>
    /// ReviewAction 属性。
    /// </summary>
    public ReviewAction ReviewAction { get; set; }
    /// <summary>
    /// Reason 属性。
    /// </summary>
    public string? Reason { get; set; }
    /// <summary>
    /// RecognizedProjectLevel 属性。
    /// </summary>
    public ProjectLevel? RecognizedProjectLevel { get; set; }
    /// <summary>
    /// RecognizedAwardLevel 属性。
    /// </summary>
    public AwardLevel? RecognizedAwardLevel { get; set; }
    /// <summary>
    /// RecognizedAmount 属性。
    /// </summary>
    public decimal? RecognizedAmount { get; set; }
    /// <summary>
    /// Remark 属性。
    /// </summary>
    public string? Remark { get; set; }
    /// <summary>
    /// ReviewedByUserId 属性。
    /// </summary>
    public long ReviewedByUserId { get; set; }
    /// <summary>
    /// ReviewedAt 属性。
    /// </summary>
    public DateTime ReviewedAt { get; set; }
}

/// <summary>
/// PendingReviewQueryDto 类。
/// </summary>
public class PendingReviewQueryDto
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
}

/// <summary>
/// PendingReviewItemDto 类。
/// </summary>
public class PendingReviewItemDto
{
    /// <summary>
    /// DeclarationId 属性。
    /// </summary>
    public long DeclarationId { get; set; }
    /// <summary>
    /// ProjectName 属性。
    /// </summary>
    public string ProjectName { get; set; } = string.Empty;
    /// <summary>
    /// DepartmentName 属性。
    /// </summary>
    public string DepartmentName { get; set; } = string.Empty;
    /// <summary>
    /// CurrentReviewStage 属性。
    /// </summary>
    public ReviewStage CurrentReviewStage { get; set; }
    /// <summary>
    /// SubmittedAt 属性。
    /// </summary>
    public DateTime? SubmittedAt { get; set; }
}
