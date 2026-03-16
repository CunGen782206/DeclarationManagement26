namespace DeclarationManagement.Api.Entities;

/// <summary>
/// 用户-预审部门权限关联实体（多对多）。
/// </summary>
public class UserPreReviewDepartment
{
    /// <summary>
    /// 用户ID属性。
    /// </summary>
    public long UserId { get; set; }
    /// <summary>
    /// 部门ID属性。
    /// </summary>
    public long DepartmentId { get; set; }
    /// <summary>
    /// 创建时间时间属性。
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// 用户属性。
    /// </summary>
    public User? User { get; set; }
    /// <summary>
    /// 部门属性。
    /// </summary>
    public Department? Department { get; set; }
}
