using DeclarationManagement.Api.DTOs;

namespace DeclarationManagement.Api.Services;

/// <summary>
/// 认证服务实现（当前为骨架实现，后续接入 JWT 与密码校验）。
/// </summary>
public class AuthService : IAuthService
{
    public Task<LoginResponseDto> LoginAsync(LoginRequestDto request, CancellationToken cancellationToken = default)
    {
        // TODO: 接入真实账号校验与 JWT 签发。
        return Task.FromResult(new LoginResponseDto
        {
            AccessToken = "mock-token",
            ExpiresAt = DateTime.UtcNow.AddHours(2)
        });
    }

    public Task<CurrentUserDto> GetCurrentUserAsync(long userId, CancellationToken cancellationToken = default)
    {
        // TODO: 从数据库读取用户信息。
        return Task.FromResult(new CurrentUserDto
        {
            UserId = userId,
            JobNumber = "mock",
            FullName = "Mock User",
            IsSuperAdmin = false
        });
    }
}
