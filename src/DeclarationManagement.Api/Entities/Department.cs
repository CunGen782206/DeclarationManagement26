namespace DeclarationManagement.Api.Entities;

/// <summary>
/// 部门字典表实体。
/// </summary>
public class Department : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public int SortOrder { get; set; }
    public bool IsEnabled { get; set; } = true;

    public ICollection<User> Users { get; set; } = new List<User>();
    public ICollection<Declaration> Declarations { get; set; } = new List<Declaration>();
    public ICollection<UserPreReviewDepartment> UserPreReviewDepartments { get; set; } = new List<UserPreReviewDepartment>();
}
