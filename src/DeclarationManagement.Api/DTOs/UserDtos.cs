namespace DeclarationManagement.Api.DTOs;

/// <summary>
/// 用户列表/详情 DTO。
/// </summary>
public class UserDto
{
    public long Id { get; set; }
    public string JobNumber { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public long DepartmentId { get; set; }
    public string DepartmentName { get; set; } = string.Empty;
    public bool IsSuperAdmin { get; set; }
    public bool IsEnabled { get; set; }
    public List<long> PreReviewDepartmentIds { get; set; } = new();
    public List<long> InitialReviewCategoryIds { get; set; } = new();
}

/// <summary>
/// 新建用户请求 DTO。
/// </summary>
public class CreateUserRequestDto
{
    public string JobNumber { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public long DepartmentId { get; set; }
    public bool IsSuperAdmin { get; set; }
    public List<long> PreReviewDepartmentIds { get; set; } = new();
    public List<long> InitialReviewCategoryIds { get; set; } = new();
}

/// <summary>
/// 更新用户请求 DTO。
/// </summary>
public class UpdateUserRequestDto
{
    public string FullName { get; set; } = string.Empty;
    public long DepartmentId { get; set; }
    public bool IsEnabled { get; set; }
    public bool IsSuperAdmin { get; set; }
    public List<long> PreReviewDepartmentIds { get; set; } = new();
    public List<long> InitialReviewCategoryIds { get; set; } = new();
}
