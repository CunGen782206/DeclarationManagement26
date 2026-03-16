namespace DeclarationManagement.Api.Entities;

/// <summary>
/// 申报任务实体（含时间窗口）。
/// </summary>
public class DeclarationTask : BaseEntity
{
    /// <summary>
    /// TaskName 属性。
    /// </summary>
    public string TaskName { get; set; } = string.Empty;
    /// <summary>
    /// StartAt 属性。
    /// </summary>
    public DateTime StartAt { get; set; }
    /// <summary>
    /// EndAt 属性。
    /// </summary>
    public DateTime EndAt { get; set; }
    /// <summary>
    /// IsEnabled 属性。
    /// </summary>
    public bool IsEnabled { get; set; } = true;
    /// <summary>
    /// CreatedByUserId 属性。
    /// </summary>
    public long CreatedByUserId { get; set; }
    /// <summary>
    /// UpdatedAt 属性。
    /// </summary>
    public DateTime? UpdatedAt { get; set; }

    /// <summary>
    /// CreatedByUser 属性。
    /// </summary>
    public User? CreatedByUser { get; set; }
    /// <summary>
    /// Declarations 属性。
    /// </summary>
    public ICollection<Declaration> Declarations { get; set; } = new List<Declaration>();
}
