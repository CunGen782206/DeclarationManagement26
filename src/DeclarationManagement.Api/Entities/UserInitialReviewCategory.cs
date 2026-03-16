namespace DeclarationManagement.Api.Entities;

/// <summary>
/// 用户-初审项目类别权限关联实体（多对多）。
/// </summary>
public class UserInitialReviewCategory
{
    /// <summary>
    /// UserId 属性。
    /// </summary>
    public long UserId { get; set; }
    /// <summary>
    /// ProjectCategoryId 属性。
    /// </summary>
    public long ProjectCategoryId { get; set; }
    /// <summary>
    /// CreatedAt 属性。
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// User 属性。
    /// </summary>
    public User? User { get; set; }
    /// <summary>
    /// ProjectCategory 属性。
    /// </summary>
    public ProjectCategory? ProjectCategory { get; set; }
}
