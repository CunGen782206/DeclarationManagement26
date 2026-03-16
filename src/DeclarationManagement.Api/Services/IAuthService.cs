using DeclarationManagement.Api.DTOs;

namespace DeclarationManagement.Api.Services;

/// <summary>
/// IAuthService 接口。
/// </summary>
public interface IAuthService
{
    Task<LoginResponseDto> LoginAsync(LoginRequestDto request, CancellationToken cancellationToken = default);
    Task<CurrentUserDto> GetCurrentUserAsync(long userId, CancellationToken cancellationToken = default);
    Task ChangePasswordAsync(long userId, ChangePasswordRequestDto request, CancellationToken cancellationToken = default);
}
