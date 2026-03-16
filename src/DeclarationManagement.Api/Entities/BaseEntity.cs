namespace DeclarationManagement.Api.Entities;

/// <summary>
/// 实体基类：统一主键与创建时间字段。
/// </summary>
public abstract class BaseEntity
{
    public long Id { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
