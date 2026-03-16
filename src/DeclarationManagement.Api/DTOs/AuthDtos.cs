namespace DeclarationManagement.Api.DTOs;

public class LoginRequestDto
{
    public string JobNumber { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}

public class LoginResponseDto
{
    public string AccessToken { get; set; } = string.Empty;
    public DateTime ExpiresAt { get; set; }
}

public class CurrentUserDto
{
    public long UserId { get; set; }
    public string JobNumber { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public long DepartmentId { get; set; }
    public bool IsSuperAdmin { get; set; }
}

public class ChangePasswordRequestDto
{
    public string OldPassword { get; set; } = string.Empty;
    public string NewPassword { get; set; } = string.Empty;
}
