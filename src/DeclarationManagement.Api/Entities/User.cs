namespace DeclarationManagement.Api.Entities;

/// <summary>
/// 用户实体。
/// </summary>
public class User : BaseEntity
{
    public string JobNumber { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public long DepartmentId { get; set; }
    public string PasswordHash { get; set; } = string.Empty;
    public string? PasswordSalt { get; set; }
    public bool IsSuperAdmin { get; set; }
    public bool IsEnabled { get; set; } = true;
    public DateTime? LastLoginAt { get; set; }
    public DateTime? UpdatedAt { get; set; }

    public Department? Department { get; set; }
    public ICollection<UserPreReviewDepartment> UserPreReviewDepartments { get; set; } = new List<UserPreReviewDepartment>();
    public ICollection<UserInitialReviewCategory> UserInitialReviewCategories { get; set; } = new List<UserInitialReviewCategory>();
}
