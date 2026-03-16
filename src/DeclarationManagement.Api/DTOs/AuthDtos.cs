namespace DeclarationManagement.Api.DTOs;

/// <summary>
/// 登录请求数据传输对象类。
/// </summary>
public class LoginRequestDto
{
    /// <summary>
    /// 工号编号属性。
    /// </summary>
    public string JobNumber { get; set; } = string.Empty;
    /// <summary>
    /// 密码属性。
    /// </summary>
    public string Password { get; set; } = string.Empty;
}

/// <summary>
/// 登录响应数据传输对象类。
/// </summary>
public class LoginResponseDto
{
    /// <summary>
    /// 访问令牌属性。
    /// </summary>
    public string AccessToken { get; set; } = string.Empty;
    /// <summary>
    /// 过期时间属性。
    /// </summary>
    public DateTime ExpiresAt { get; set; }
}

/// <summary>
/// 当前用户数据传输对象类。
/// </summary>
public class CurrentUserDto
{
    /// <summary>
    /// 用户ID属性。
    /// </summary>
    public long UserId { get; set; }
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
}

/// <summary>
/// Change密码请求数据传输对象类。
/// </summary>
public class ChangePasswordRequestDto
{
    /// <summary>
    /// 旧密码属性。
    /// </summary>
    public string OldPassword { get; set; } = string.Empty;
    /// <summary>
    /// 新密码属性。
    /// </summary>
    public string NewPassword { get; set; } = string.Empty;
}
