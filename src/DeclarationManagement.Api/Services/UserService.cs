using DeclarationManagement.Api.DTOs;
using DeclarationManagement.Api.Entities;
using DeclarationManagement.Api.Repositories;

namespace DeclarationManagement.Api.Services;

/// <summary>
/// 用户服务实现（当前为骨架实现，后续补全权限关联维护逻辑）。
/// </summary>
public class UserService : IUserService
{
    private readonly IRepository<User> _userRepository;

    public UserService(IRepository<User> userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<List<UserDto>> GetListAsync(CancellationToken cancellationToken = default)
    {
        var users = await _userRepository.GetListAsync(cancellationToken: cancellationToken);
        return users.Select(x => new UserDto
        {
            Id = x.Id,
            JobNumber = x.JobNumber,
            FullName = x.FullName,
            DepartmentId = x.DepartmentId,
            IsSuperAdmin = x.IsSuperAdmin,
            IsEnabled = x.IsEnabled
        }).ToList();
    }

    public async Task<long> CreateAsync(CreateUserRequestDto request, CancellationToken cancellationToken = default)
    {
        var user = new User
        {
            JobNumber = request.JobNumber,
            FullName = request.FullName,
            DepartmentId = request.DepartmentId,
            IsSuperAdmin = request.IsSuperAdmin,
            IsEnabled = true,
            PasswordHash = "TODO_HASH_111111",
            CreatedAt = DateTime.UtcNow
        };

        await _userRepository.AddAsync(user, cancellationToken);
        await _userRepository.SaveChangesAsync(cancellationToken);
        return user.Id;
    }

    public async Task UpdateAsync(long userId, UpdateUserRequestDto request, CancellationToken cancellationToken = default)
    {
        var user = await _userRepository.GetByIdAsync(userId, cancellationToken)
                   ?? throw new InvalidOperationException("用户不存在");

        user.FullName = request.FullName;
        user.DepartmentId = request.DepartmentId;
        user.IsEnabled = request.IsEnabled;
        user.IsSuperAdmin = request.IsSuperAdmin;
        user.UpdatedAt = DateTime.UtcNow;

        _userRepository.Update(user);
        await _userRepository.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(long userId, CancellationToken cancellationToken = default)
    {
        var user = await _userRepository.GetByIdAsync(userId, cancellationToken)
                   ?? throw new InvalidOperationException("用户不存在");

        _userRepository.Remove(user);
        await _userRepository.SaveChangesAsync(cancellationToken);
    }

    public async Task ResetPasswordAsync(long userId, CancellationToken cancellationToken = default)
    {
        var user = await _userRepository.GetByIdAsync(userId, cancellationToken)
                   ?? throw new InvalidOperationException("用户不存在");

        // TODO: 替换为真实密码哈希。
        user.PasswordHash = "TODO_HASH_111111";
        user.UpdatedAt = DateTime.UtcNow;

        _userRepository.Update(user);
        await _userRepository.SaveChangesAsync(cancellationToken);
    }
}
