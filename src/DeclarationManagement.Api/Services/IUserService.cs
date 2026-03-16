using DeclarationManagement.Api.DTOs;

namespace DeclarationManagement.Api.Services;

/// <summary>
/// 用户服务接口。
/// </summary>
public interface IUserService
{
    Task<List<UserDto>> GetListAsync(CancellationToken cancellationToken = default);
    Task<long> CreateAsync(CreateUserRequestDto request, CancellationToken cancellationToken = default);
    Task UpdateAsync(long userId, UpdateUserRequestDto request, CancellationToken cancellationToken = default);
    Task DeleteAsync(long userId, CancellationToken cancellationToken = default);
    Task ResetPasswordAsync(long userId, CancellationToken cancellationToken = default);
}
