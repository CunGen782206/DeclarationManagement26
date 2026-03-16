using DeclarationManagement.Api.Data;
using DeclarationManagement.Api.DTOs;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace DeclarationManagement.Api.Services;

/// <summary>
/// AuthService 类。
/// </summary>
public class AuthService : IAuthService
{
    /// <summary>
    /// _dbContext 字段。
    /// </summary>
    private readonly AppDbContext _dbContext;
    /// <summary>
    /// _jwtOptions 字段。
    /// </summary>
    private readonly JwtOptions _jwtOptions;

    /// <summary>
    /// 构造函数。
    /// </summary>
    public AuthService(AppDbContext dbContext, IOptions<JwtOptions> jwtOptions)
    {
        _dbContext = dbContext;
        _jwtOptions = jwtOptions.Value;
    }

    /// <summary>
    /// 登录处理。
    /// </summary>
    public async Task<LoginResponseDto> LoginAsync(LoginRequestDto request, CancellationToken cancellationToken = default)
    {
        var user = await _dbContext.Users.FirstOrDefaultAsync(x => x.JobNumber == request.JobNumber && x.IsEnabled, cancellationToken)
                   ?? throw new InvalidOperationException("账号不存在或已禁用");

        if (!PasswordHasher.Verify(request.Password, user.PasswordHash, user.PasswordSalt))
        {
            throw new InvalidOperationException("账号或密码错误");
        }

        user.LastLoginAt = DateTime.UtcNow;
        await _dbContext.SaveChangesAsync(cancellationToken);

        var expiresAt = DateTime.UtcNow.AddMinutes(_jwtOptions.ExpireMinutes);
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.SecretKey));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new(JwtRegisteredClaimNames.UniqueName, user.FullName),
            new("jobNumber", user.JobNumber),
            new("isSuperAdmin", user.IsSuperAdmin.ToString())
        };

        var token = new JwtSecurityToken(
            issuer: _jwtOptions.Issuer,
            audience: _jwtOptions.Audience,
            claims: claims,
            expires: expiresAt,
            signingCredentials: creds);

        return new LoginResponseDto
        {
            AccessToken = new JwtSecurityTokenHandler().WriteToken(token),
            ExpiresAt = expiresAt
        };
    }

    /// <summary>
    /// 获取数据。
    /// </summary>
    public async Task<CurrentUserDto> GetCurrentUserAsync(long userId, CancellationToken cancellationToken = default)
    {
        var user = await _dbContext.Users.AsNoTracking().FirstOrDefaultAsync(x => x.Id == userId, cancellationToken)
                   ?? throw new InvalidOperationException("用户不存在");

        return new CurrentUserDto
        {
            UserId = user.Id,
            JobNumber = user.JobNumber,
            FullName = user.FullName,
            DepartmentId = user.DepartmentId,
            IsSuperAdmin = user.IsSuperAdmin
        };
    }

    /// <summary>
    /// 变更处理。
    /// </summary>
    public async Task ChangePasswordAsync(long userId, ChangePasswordRequestDto request, CancellationToken cancellationToken = default)
    {
        var user = await _dbContext.Users.FirstOrDefaultAsync(x => x.Id == userId, cancellationToken)
                   ?? throw new InvalidOperationException("用户不存在");

        if (!PasswordHasher.Verify(request.OldPassword, user.PasswordHash, user.PasswordSalt))
        {
            throw new InvalidOperationException("原密码错误");
        }

        if (request.NewPassword.Length is < 6 or > 8)
        {
            throw new InvalidOperationException("新密码长度必须为6-8位");
        }

        var (hash, salt) = PasswordHasher.Hash(request.NewPassword);
        user.PasswordHash = hash;
        user.PasswordSalt = salt;
        user.UpdatedAt = DateTime.UtcNow;

        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}
