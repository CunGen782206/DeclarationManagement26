namespace DeclarationManagement.Api.Entities;

/// <summary>
/// 申报任务实体（含时间窗口）。
/// </summary>
public class DeclarationTask : BaseEntity
{
    public string TaskName { get; set; } = string.Empty;
    public DateTime StartAt { get; set; }
    public DateTime EndAt { get; set; }
    public bool IsEnabled { get; set; } = true;
    public long CreatedByUserId { get; set; }
    public DateTime? UpdatedAt { get; set; }

    public User? CreatedByUser { get; set; }
    public ICollection<Declaration> Declarations { get; set; } = new List<Declaration>();
}
