namespace DeclarationManagement.Api.Entities;

/// <summary>
/// 用户实体。
/// </summary>
public class User : BaseEntity
{
    /// <summary>
    /// JobNumber 属性。
    /// </summary>
    public string JobNumber { get; set; } = string.Empty;
    /// <summary>
    /// FullName 属性。
    /// </summary>
    public string FullName { get; set; } = string.Empty;
    /// <summary>
    /// DepartmentId 属性。
    /// </summary>
    public long DepartmentId { get; set; }
    /// <summary>
    /// PasswordHash 属性。
    /// </summary>
    public string PasswordHash { get; set; } = string.Empty;
    /// <summary>
    /// PasswordSalt 属性。
    /// </summary>
    public string? PasswordSalt { get; set; }
    /// <summary>
    /// IsSuperAdmin 属性。
    /// </summary>
    public bool IsSuperAdmin { get; set; }
    /// <summary>
    /// IsEnabled 属性。
    /// </summary>
    public bool IsEnabled { get; set; } = true;
    /// <summary>
    /// LastLoginAt 属性。
    /// </summary>
    public DateTime? LastLoginAt { get; set; }
    /// <summary>
    /// UpdatedAt 属性。
    /// </summary>
    public DateTime? UpdatedAt { get; set; }

    /// <summary>
    /// Department 属性。
    /// </summary>
    public Department? Department { get; set; }
    /// <summary>
    /// UserPreReviewDepartments 属性。
    /// </summary>
    public ICollection<UserPreReviewDepartment> UserPreReviewDepartments { get; set; } = new List<UserPreReviewDepartment>();
    /// <summary>
    /// UserInitialReviewCategories 属性。
    /// </summary>
    public ICollection<UserInitialReviewCategory> UserInitialReviewCategories { get; set; } = new List<UserInitialReviewCategory>();
}
