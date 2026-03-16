using DeclarationManagement.Api.Entities;

namespace DeclarationManagement.Api.DTOs;

/// <summary>
/// 审核动作请求 DTO。
/// </summary>
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

/// <summary>
/// 审核记录展示 DTO。
/// </summary>
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
