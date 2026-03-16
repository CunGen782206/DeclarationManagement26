namespace DeclarationManagement.Api.Entities;

/// <summary>
/// 用户-初审项目类别权限关联实体（多对多）。
/// </summary>
public class UserInitialReviewCategory
{
    /// <summary>
    /// 用户ID属性。
    /// </summary>
    public long UserId { get; set; }
    /// <summary>
    /// 项目类别ID属性。
    /// </summary>
    public long ProjectCategoryId { get; set; }
    /// <summary>
    /// 创建时间时间属性。
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// 用户属性。
    /// </summary>
    public User? User { get; set; }
    /// <summary>
    /// 项目类别属性。
    /// </summary>
    public ProjectCategory? ProjectCategory { get; set; }
}
