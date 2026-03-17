using DeclarationManagement.Api.DTOs;

namespace DeclarationManagement.Api.Services;

/// <summary>
/// IAuth服务接口。
/// </summary>
public interface IAuthService
{
    Task<LoginResponseDto> LoginAsync(LoginRequestDto request, CancellationToken cancellationToken = default);
    Task<CurrentUserDto> GetCurrentUserAsync(long userId, CancellationToken cancellationToken = default);
    Task ChangePasswordAsync(long userId, ChangePasswordRequestDto request, CancellationToken cancellationToken = default);
}
