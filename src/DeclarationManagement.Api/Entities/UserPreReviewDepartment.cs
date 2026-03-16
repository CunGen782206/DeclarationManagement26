namespace DeclarationManagement.Api.Entities;

/// <summary>
/// 用户-预审部门权限关联实体（多对多）。
/// </summary>
public class UserPreReviewDepartment
{
    /// <summary>
    /// UserId 属性。
    /// </summary>
    public long UserId { get; set; }
    /// <summary>
    /// DepartmentId 属性。
    /// </summary>
    public long DepartmentId { get; set; }
    /// <summary>
    /// CreatedAt 属性。
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// User 属性。
    /// </summary>
    public User? User { get; set; }
    /// <summary>
    /// Department 属性。
    /// </summary>
    public Department? Department { get; set; }
}
