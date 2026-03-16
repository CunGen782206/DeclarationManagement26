namespace DeclarationManagement.Api.DTOs;

/// <summary>
/// 登录请求 DTO。
/// </summary>
public class LoginRequestDto
{
    public string JobNumber { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}

/// <summary>
/// 登录返回 DTO。
/// </summary>
public class LoginResponseDto
{
    public string AccessToken { get; set; } = string.Empty;
    public DateTime ExpiresAt { get; set; }
}

/// <summary>
/// 当前登录用户信息 DTO。
/// </summary>
public class CurrentUserDto
{
    public long UserId { get; set; }
    public string JobNumber { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public bool IsSuperAdmin { get; set; }
}
