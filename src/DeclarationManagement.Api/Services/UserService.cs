using DeclarationManagement.Api.Data;
using DeclarationManagement.Api.DTOs;
using DeclarationManagement.Api.Entities;
using Microsoft.EntityFrameworkCore;

namespace DeclarationManagement.Api.Services;

public class UserService : IUserService
{
    private readonly AppDbContext _dbContext;

    public UserService(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<UserDto>> GetListAsync(UserQueryDto query, CancellationToken cancellationToken = default)
    {
        var usersQuery = _dbContext.Users
            .AsNoTracking()
            .Include(x => x.Department)
            .Include(x => x.UserPreReviewDepartments)
            .Include(x => x.UserInitialReviewCategories)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(query.JobNumber))
        {
            usersQuery = usersQuery.Where(x => x.JobNumber.Contains(query.JobNumber));
        }

        if (!string.IsNullOrWhiteSpace(query.FullName))
        {
            usersQuery = usersQuery.Where(x => x.FullName.Contains(query.FullName));
        }

        if (query.DepartmentIds is { Count: > 0 })
        {
            usersQuery = usersQuery.Where(x => query.DepartmentIds.Contains(x.DepartmentId));
        }

        var users = await usersQuery
            .OrderBy(x => x.JobNumber)
            .ToListAsync(cancellationToken);

        return users.Select(x => new UserDto
        {
            Id = x.Id,
            JobNumber = x.JobNumber,
            FullName = x.FullName,
            DepartmentId = x.DepartmentId,
            DepartmentName = x.Department?.Name ?? string.Empty,
            IsSuperAdmin = x.IsSuperAdmin,
            IsEnabled = x.IsEnabled,
            PreReviewDepartmentIds = x.UserPreReviewDepartments.Select(i => i.DepartmentId).ToList(),
            InitialReviewCategoryIds = x.UserInitialReviewCategories.Select(i => i.ProjectCategoryId).ToList()
        }).ToList();
    }

    public async Task<long> CreateAsync(CreateUserRequestDto request, CancellationToken cancellationToken = default)
    {
        var existed = await _dbContext.Users.AnyAsync(x => x.JobNumber == request.JobNumber, cancellationToken);
        if (existed)
        {
            throw new InvalidOperationException("工号已存在");
        }

        var (hash, salt) = PasswordHasher.Hash("111111");
        var user = new User
        {
            JobNumber = request.JobNumber,
            FullName = request.FullName,
            DepartmentId = request.DepartmentId,
            IsSuperAdmin = request.IsSuperAdmin,
            IsEnabled = true,
            PasswordHash = hash,
            PasswordSalt = salt,
            CreatedAt = DateTime.UtcNow
        };

        await _dbContext.Users.AddAsync(user, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);

        await ReplacePermissionsAsync(user.Id, request.PreReviewDepartmentIds, request.InitialReviewCategoryIds, cancellationToken);
        return user.Id;
    }

    public async Task UpdateAsync(long userId, UpdateUserRequestDto request, CancellationToken cancellationToken = default)
    {
        var user = await _dbContext.Users.FirstOrDefaultAsync(x => x.Id == userId, cancellationToken)
            ?? throw new InvalidOperationException("用户不存在");

        user.FullName = request.FullName;
        user.DepartmentId = request.DepartmentId;
        user.IsEnabled = request.IsEnabled;
        user.IsSuperAdmin = request.IsSuperAdmin;
        user.UpdatedAt = DateTime.UtcNow;

        await _dbContext.SaveChangesAsync(cancellationToken);
        await ReplacePermissionsAsync(user.Id, request.PreReviewDepartmentIds, request.InitialReviewCategoryIds, cancellationToken);
    }

    public async Task DeleteAsync(long userId, CancellationToken cancellationToken = default)
    {
        var user = await _dbContext.Users.FirstOrDefaultAsync(x => x.Id == userId, cancellationToken)
            ?? throw new InvalidOperationException("用户不存在");

        _dbContext.Users.Remove(user);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task ResetPasswordAsync(long userId, CancellationToken cancellationToken = default)
    {
        var user = await _dbContext.Users.FirstOrDefaultAsync(x => x.Id == userId, cancellationToken)
            ?? throw new InvalidOperationException("用户不存在");

        var (hash, salt) = PasswordHasher.Hash("111111");
        user.PasswordHash = hash;
        user.PasswordSalt = salt;
        user.UpdatedAt = DateTime.UtcNow;
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    private async Task ReplacePermissionsAsync(long userId, List<long> preReviewDepartmentIds, List<long> initialReviewCategoryIds, CancellationToken cancellationToken)
    {
        var preRecords = await _dbContext.UserPreReviewDepartments.Where(x => x.UserId == userId).ToListAsync(cancellationToken);
        _dbContext.UserPreReviewDepartments.RemoveRange(preRecords);

        var initRecords = await _dbContext.UserInitialReviewCategories.Where(x => x.UserId == userId).ToListAsync(cancellationToken);
        _dbContext.UserInitialReviewCategories.RemoveRange(initRecords);

        if (preReviewDepartmentIds.Count > 0)
        {
            await _dbContext.UserPreReviewDepartments.AddRangeAsync(preReviewDepartmentIds.Distinct().Select(x => new UserPreReviewDepartment
            {
                UserId = userId,
                DepartmentId = x,
                CreatedAt = DateTime.UtcNow
            }), cancellationToken);
        }

        if (initialReviewCategoryIds.Count > 0)
        {
            await _dbContext.UserInitialReviewCategories.AddRangeAsync(initialReviewCategoryIds.Distinct().Select(x => new UserInitialReviewCategory
            {
                UserId = userId,
                ProjectCategoryId = x,
                CreatedAt = DateTime.UtcNow
            }), cancellationToken);
        }

        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}
