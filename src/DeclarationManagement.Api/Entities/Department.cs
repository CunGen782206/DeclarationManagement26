namespace DeclarationManagement.Api.Entities;

/// <summary>
/// 部门字典表实体。
/// </summary>
public class Department : BaseEntity
{
    /// <summary>
    /// Name 属性。
    /// </summary>
    public string Name { get; set; } = string.Empty;
    /// <summary>
    /// SortOrder 属性。
    /// </summary>
    public int SortOrder { get; set; }
    /// <summary>
    /// IsEnabled 属性。
    /// </summary>
    public bool IsEnabled { get; set; } = true;

    /// <summary>
    /// Users 属性。
    /// </summary>
    public ICollection<User> Users { get; set; } = new List<User>();
    /// <summary>
    /// Declarations 属性。
    /// </summary>
    public ICollection<Declaration> Declarations { get; set; } = new List<Declaration>();
    /// <summary>
    /// UserPreReviewDepartments 属性。
    /// </summary>
    public ICollection<UserPreReviewDepartment> UserPreReviewDepartments { get; set; } = new List<UserPreReviewDepartment>();
}
