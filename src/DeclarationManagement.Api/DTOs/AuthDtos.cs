namespace DeclarationManagement.Api.DTOs;

/// <summary>
/// LoginRequestDto 类。
/// </summary>
public class LoginRequestDto
{
    /// <summary>
    /// JobNumber 属性。
    /// </summary>
    public string JobNumber { get; set; } = string.Empty;
    /// <summary>
    /// Password 属性。
    /// </summary>
    public string Password { get; set; } = string.Empty;
}

/// <summary>
/// LoginResponseDto 类。
/// </summary>
public class LoginResponseDto
{
    /// <summary>
    /// AccessToken 属性。
    /// </summary>
    public string AccessToken { get; set; } = string.Empty;
    /// <summary>
    /// ExpiresAt 属性。
    /// </summary>
    public DateTime ExpiresAt { get; set; }
}

/// <summary>
/// CurrentUserDto 类。
/// </summary>
public class CurrentUserDto
{
    /// <summary>
    /// UserId 属性。
    /// </summary>
    public long UserId { get; set; }
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
}

/// <summary>
/// ChangePasswordRequestDto 类。
/// </summary>
public class ChangePasswordRequestDto
{
    /// <summary>
    /// OldPassword 属性。
    /// </summary>
    public string OldPassword { get; set; } = string.Empty;
    /// <summary>
    /// NewPassword 属性。
    /// </summary>
    public string NewPassword { get; set; } = string.Empty;
}
