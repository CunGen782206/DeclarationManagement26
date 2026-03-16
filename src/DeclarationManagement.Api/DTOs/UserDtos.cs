namespace DeclarationManagement.Api.DTOs;

/// <summary>
/// 用户数据传输对象类。
/// </summary>
public class UserDto
{
    /// <summary>
    /// ID属性。
    /// </summary>
    public long Id { get; set; }
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
    /// 部门名称属性。
    /// </summary>
    public string DepartmentName { get; set; } = string.Empty;
    /// <summary>
    /// 是否超级管理员属性。
    /// </summary>
    public bool IsSuperAdmin { get; set; }
    /// <summary>
    /// 是否启用属性。
    /// </summary>
    public bool IsEnabled { get; set; }
    /// <summary>
    /// 预审核部门Ids属性。
    /// </summary>
    public List<long> PreReviewDepartmentIds { get; set; } = new();
    /// <summary>
    /// 初审核类别Ids属性。
    /// </summary>
    public List<long> InitialReviewCategoryIds { get; set; } = new();
}

/// <summary>
/// Create用户请求数据传输对象类。
/// </summary>
public class CreateUserRequestDto
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
    /// 是否超级管理员属性。
    /// </summary>
    public bool IsSuperAdmin { get; set; }
    /// <summary>
    /// 预审核部门Ids属性。
    /// </summary>
    public List<long> PreReviewDepartmentIds { get; set; } = new();
    /// <summary>
    /// 初审核类别Ids属性。
    /// </summary>
    public List<long> InitialReviewCategoryIds { get; set; } = new();
}

/// <summary>
/// Update用户请求数据传输对象类。
/// </summary>
public class UpdateUserRequestDto
{
    /// <summary>
    /// 姓名属性。
    /// </summary>
    public string FullName { get; set; } = string.Empty;
    /// <summary>
    /// 部门ID属性。
    /// </summary>
    public long DepartmentId { get; set; }
    /// <summary>
    /// 是否启用属性。
    /// </summary>
    public bool IsEnabled { get; set; }
    /// <summary>
    /// 是否超级管理员属性。
    /// </summary>
    public bool IsSuperAdmin { get; set; }
    /// <summary>
    /// 预审核部门Ids属性。
    /// </summary>
    public List<long> PreReviewDepartmentIds { get; set; } = new();
    /// <summary>
    /// 初审核类别Ids属性。
    /// </summary>
    public List<long> InitialReviewCategoryIds { get; set; } = new();
}
