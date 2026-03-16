namespace DeclarationManagement.Api.DTOs;

/// <summary>
/// UserDto 类。
/// </summary>
public class UserDto
{
    /// <summary>
    /// Id 属性。
    /// </summary>
    public long Id { get; set; }
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
    /// DepartmentName 属性。
    /// </summary>
    public string DepartmentName { get; set; } = string.Empty;
    /// <summary>
    /// IsSuperAdmin 属性。
    /// </summary>
    public bool IsSuperAdmin { get; set; }
    /// <summary>
    /// IsEnabled 属性。
    /// </summary>
    public bool IsEnabled { get; set; }
    /// <summary>
    /// PreReviewDepartmentIds 属性。
    /// </summary>
    public List<long> PreReviewDepartmentIds { get; set; } = new();
    /// <summary>
    /// InitialReviewCategoryIds 属性。
    /// </summary>
    public List<long> InitialReviewCategoryIds { get; set; } = new();
}

/// <summary>
/// CreateUserRequestDto 类。
/// </summary>
public class CreateUserRequestDto
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
    /// IsSuperAdmin 属性。
    /// </summary>
    public bool IsSuperAdmin { get; set; }
    /// <summary>
    /// PreReviewDepartmentIds 属性。
    /// </summary>
    public List<long> PreReviewDepartmentIds { get; set; } = new();
    /// <summary>
    /// InitialReviewCategoryIds 属性。
    /// </summary>
    public List<long> InitialReviewCategoryIds { get; set; } = new();
}

/// <summary>
/// UpdateUserRequestDto 类。
/// </summary>
public class UpdateUserRequestDto
{
    /// <summary>
    /// FullName 属性。
    /// </summary>
    public string FullName { get; set; } = string.Empty;
    /// <summary>
    /// DepartmentId 属性。
    /// </summary>
    public long DepartmentId { get; set; }
    /// <summary>
    /// IsEnabled 属性。
    /// </summary>
    public bool IsEnabled { get; set; }
    /// <summary>
    /// IsSuperAdmin 属性。
    /// </summary>
    public bool IsSuperAdmin { get; set; }
    /// <summary>
    /// PreReviewDepartmentIds 属性。
    /// </summary>
    public List<long> PreReviewDepartmentIds { get; set; } = new();
    /// <summary>
    /// InitialReviewCategoryIds 属性。
    /// </summary>
    public List<long> InitialReviewCategoryIds { get; set; } = new();
}
