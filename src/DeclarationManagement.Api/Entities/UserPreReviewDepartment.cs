namespace DeclarationManagement.Api.Entities;

/// <summary>
/// 用户-预审部门权限关联实体（多对多）。
/// </summary>
public class UserPreReviewDepartment
{
    public long UserId { get; set; }
    public long DepartmentId { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public User? User { get; set; }
    public Department? Department { get; set; }
}
