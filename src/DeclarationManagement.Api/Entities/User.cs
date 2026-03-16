namespace DeclarationManagement.Api.Entities;

/// <summary>
/// 用户实体。
/// </summary>
public class User : BaseEntity
{
    /// <summary>
    /// 工号编号属性。
    /// </summary>
    public string JobNumber { get; set; } = string.Empty;
    /// <summary>
    /// 姓名属性。
    /// </summary>
    public string FullName { get; set; } = string.Empty;
    /// <summary>
    /// 部门ID属性。
    /// </summary>
    public long DepartmentId { get; set; }
    /// <summary>
    /// 密码哈希值属性。
    /// </summary>
    public string PasswordHash { get; set; } = string.Empty;
    /// <summary>
    /// 密码盐值属性。
    /// </summary>
    public string? PasswordSalt { get; set; }
    /// <summary>
    /// 是否超级管理员属性。
    /// </summary>
    public bool IsSuperAdmin { get; set; }
    /// <summary>
    /// 是否启用属性。
    /// </summary>
    public bool IsEnabled { get; set; } = true;
    /// <summary>
    /// 最后登录时间属性。
    /// </summary>
    public DateTime? LastLoginAt { get; set; }
    /// <summary>
    /// 更新时间时间属性。
    /// </summary>
    public DateTime? UpdatedAt { get; set; }

    /// <summary>
    /// 部门属性。
    /// </summary>
    public Department? Department { get; set; }
    /// <summary>
    /// 用户预审核Departments属性。
    /// </summary>
    public ICollection<UserPreReviewDepartment> UserPreReviewDepartments { get; set; } = new List<UserPreReviewDepartment>();
    /// <summary>
    /// 用户初审核Categories属性。
    /// </summary>
    public ICollection<UserInitialReviewCategory> UserInitialReviewCategories { get; set; } = new List<UserInitialReviewCategory>();
}
