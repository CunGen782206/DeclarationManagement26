using DeclarationManagement.Api.DTOs;

namespace DeclarationManagement.Api.Services;

/// <summary>
/// 认证服务接口。
/// </summary>
public interface IAuthService
{
    Task<LoginResponseDto> LoginAsync(LoginRequestDto request, CancellationToken cancellationToken = default);
    Task<CurrentUserDto> GetCurrentUserAsync(long userId, CancellationToken cancellationToken = default);
}
