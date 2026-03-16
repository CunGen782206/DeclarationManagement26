namespace DeclarationManagement.Api.Entities;

/// <summary>
/// 审核记录实体（每次审核都新增，满足多轮累计）。
/// </summary>
public class DeclarationReviewRecord : BaseEntity
{
    public long DeclarationId { get; set; }
    public ReviewStage ReviewStage { get; set; }
    public ReviewAction ReviewAction { get; set; }
    public string? Reason { get; set; }
    public ProjectLevel? RecognizedProjectLevel { get; set; }
    public AwardLevel? RecognizedAwardLevel { get; set; }
    public decimal? RecognizedAmount { get; set; }
    public string? Remark { get; set; }
    public long ReviewedByUserId { get; set; }
    public DateTime ReviewedAt { get; set; } = DateTime.UtcNow;

    public Declaration? Declaration { get; set; }
    public User? ReviewedByUser { get; set; }
}
