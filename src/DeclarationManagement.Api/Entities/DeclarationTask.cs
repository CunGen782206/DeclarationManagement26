namespace DeclarationManagement.Api.Entities;

/// <summary>
/// 申报任务实体（含时间窗口）。
/// </summary>
public class DeclarationTask : BaseEntity
{
    /// <summary>
    /// 任务名称属性。
    /// </summary>
    public string TaskName { get; set; } = string.Empty;
    /// <summary>
    /// 开始时间属性。
    /// </summary>
    public DateTime StartAt { get; set; }
    /// <summary>
    /// 结束时间属性。
    /// </summary>
    public DateTime EndAt { get; set; }
    /// <summary>
    /// 是否启用属性。
    /// </summary>
    public bool IsEnabled { get; set; } = true;
    /// <summary>
    /// 创建时间By用户ID属性。
    /// </summary>
    public long CreatedByUserId { get; set; }
    /// <summary>
    /// 更新时间时间属性。
    /// </summary>
    public DateTime? UpdatedAt { get; set; }

    /// <summary>
    /// 创建时间By用户属性。
    /// </summary>
    public User? CreatedByUser { get; set; }
    /// <summary>
    /// Declarations属性。
    /// </summary>
    public ICollection<Declaration> Declarations { get; set; } = new List<Declaration>();
}
