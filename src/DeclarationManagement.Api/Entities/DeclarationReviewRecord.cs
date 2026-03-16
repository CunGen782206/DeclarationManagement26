namespace DeclarationManagement.Api.Entities;

/// <summary>
/// 审核记录实体（每次审核都新增，满足多轮累计）。
/// </summary>
public class DeclarationReviewRecord : BaseEntity
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
    /// <summary>
    /// ReviewedByUserId 属性。
    /// </summary>
    public long ReviewedByUserId { get; set; }
    /// <summary>
    /// ReviewedAt 属性。
    /// </summary>
    public DateTime ReviewedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Declaration 属性。
    /// </summary>
    public Declaration? Declaration { get; set; }
    /// <summary>
    /// ReviewedByUser 属性。
    /// </summary>
    public User? ReviewedByUser { get; set; }
}
