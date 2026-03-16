namespace DeclarationManagement.Api.Entities;

/// <summary>
/// 审核记录实体（每次审核都新增，满足多轮累计）。
/// </summary>
public class DeclarationReviewRecord : BaseEntity
{
    /// <summary>
    /// 申报ID属性。
    /// </summary>
    public long DeclarationId { get; set; }
    /// <summary>
    /// 审核Stage属性。
    /// </summary>
    public ReviewStage ReviewStage { get; set; }
    /// <summary>
    /// 审核动作属性。
    /// </summary>
    public ReviewAction ReviewAction { get; set; }
    /// <summary>
    /// 原因属性。
    /// </summary>
    public string? Reason { get; set; }
    /// <summary>
    /// 认定项目等级属性。
    /// </summary>
    public ProjectLevel? RecognizedProjectLevel { get; set; }
    /// <summary>
    /// 认定奖项等级属性。
    /// </summary>
    public AwardLevel? RecognizedAwardLevel { get; set; }
    /// <summary>
    /// 认定金额属性。
    /// </summary>
    public decimal? RecognizedAmount { get; set; }
    /// <summary>
    /// 备注属性。
    /// </summary>
    public string? Remark { get; set; }
    /// <summary>
    /// ReviewedBy用户ID属性。
    /// </summary>
    public long ReviewedByUserId { get; set; }
    /// <summary>
    /// Reviewed时间属性。
    /// </summary>
    public DateTime ReviewedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// 申报属性。
    /// </summary>
    public Declaration? Declaration { get; set; }
    /// <summary>
    /// ReviewedBy用户属性。
    /// </summary>
    public User? ReviewedByUser { get; set; }
}
