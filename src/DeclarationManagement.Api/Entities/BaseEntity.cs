namespace DeclarationManagement.Api.Entities;

/// <summary>
/// 实体基类：统一主键与创建时间字段。
/// </summary>
public abstract class BaseEntity
{
    /// <summary>
    /// ID属性。
    /// </summary>
    public long Id { get; set; }
    /// <summary>
    /// 创建时间时间属性。
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
