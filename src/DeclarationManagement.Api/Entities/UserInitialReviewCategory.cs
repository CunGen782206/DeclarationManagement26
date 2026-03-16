namespace DeclarationManagement.Api.Entities;

/// <summary>
/// 用户-初审项目类别权限关联实体（多对多）。
/// </summary>
public class UserInitialReviewCategory
{
    public long UserId { get; set; }
    public long ProjectCategoryId { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public User? User { get; set; }
    public ProjectCategory? ProjectCategory { get; set; }
}
