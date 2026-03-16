namespace DeclarationManagement.Api.Entities;

/// <summary>
/// 实体基类：统一主键与创建时间字段。
/// </summary>
public abstract class BaseEntity
{
    /// <summary>
    /// Id 属性。
    /// </summary>
    public long Id { get; set; }
    /// <summary>
    /// CreatedAt 属性。
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
