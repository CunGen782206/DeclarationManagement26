using ClosedXML.Excel;
using DeclarationManagement.Api.Data;
using DeclarationManagement.Api.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace DeclarationManagement.Api.Services;

public class DatabaseInitializer
{
    private static readonly string[] DefaultProjectCategories =
    [
        "专业建设类",
        "课程建设类",
        "师资建设类",
        "教学竞赛类",
        "教材建设类",
        "教学成果类"
    ];

    private readonly AppDbContext _dbContext;
    private readonly DatabaseInitializationOptions _options;
    private readonly ILogger<DatabaseInitializer> _logger;

    public DatabaseInitializer(
        AppDbContext dbContext,
        IOptions<DatabaseInitializationOptions> options,
        ILogger<DatabaseInitializer> logger)
    {
        _dbContext = dbContext;
        _options = options.Value;
        _logger = logger;
    }

    public async Task InitializeAsync(CancellationToken cancellationToken = default)
    {
        if (_options.AutoMigrate)
        {
            await _dbContext.Database.MigrateAsync(cancellationToken);
        }

        if (!_options.SeedOnStartup)
        {
            return;
        }

        await SeedDepartmentsAsync(cancellationToken);
        await SeedProjectCategoriesAsync(cancellationToken);
        await SeedUsersAsync(cancellationToken);
    }

    private async Task SeedDepartmentsAsync(CancellationToken cancellationToken)
    {
        var departmentNames = await ReadDepartmentNamesFromExcelAsync(cancellationToken);
        if (departmentNames.Count == 0)
        {
            return;
        }

        var existingNames = await _dbContext.Departments
            .AsNoTracking()
            .Select(x => x.Name)
            .ToListAsync(cancellationToken);

        var toAdd = departmentNames
            .Except(existingNames)
            .Select((name, index) => new Department
            {
                Name = name,
                SortOrder = index + 1,
                IsEnabled = true,
                CreatedAt = DateTime.UtcNow
            })
            .ToList();

        if (toAdd.Count == 0)
        {
            return;
        }

        await _dbContext.Departments.AddRangeAsync(toAdd, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    private async Task SeedProjectCategoriesAsync(CancellationToken cancellationToken)
    {
        var existingNames = await _dbContext.ProjectCategories
            .AsNoTracking()
            .Select(x => x.Name)
            .ToListAsync(cancellationToken);

        var toAdd = DefaultProjectCategories
            .Except(existingNames)
            .Select((name, index) => new ProjectCategory
            {
                Name = name,
                SortOrder = index + 1,
                IsEnabled = true,
                CreatedAt = DateTime.UtcNow
            })
            .ToList();

        if (toAdd.Count == 0)
        {
            return;
        }

        await _dbContext.ProjectCategories.AddRangeAsync(toAdd, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    private async Task SeedUsersAsync(CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(_options.UsersExcelPath) || !File.Exists(_options.UsersExcelPath))
        {
            _logger.LogWarning("Users Excel file not found: {Path}", _options.UsersExcelPath);
            return;
        }

        var departments = await _dbContext.Departments
            .AsNoTracking()
            .ToDictionaryAsync(x => x.Name, x => x.Id, cancellationToken);

        using var workbook = new XLWorkbook(_options.UsersExcelPath);
        var worksheet = workbook.Worksheet(1);
        var lastRow = worksheet.LastRowUsed()?.RowNumber() ?? 0;

        if (lastRow < 2)
        {
            return;
        }

        var existingUsers = await _dbContext.Users
            .ToDictionaryAsync(x => x.JobNumber, cancellationToken);

        var now = DateTime.UtcNow;

        for (var row = 2; row <= lastRow; row++)
        {
            var jobNumber = worksheet.Cell(row, 1).GetFormattedString().Trim();
            var password = worksheet.Cell(row, 2).GetFormattedString().Trim();
            var departmentName = worksheet.Cell(row, 3).GetFormattedString().Trim();
            var fullName = worksheet.Cell(row, 4).GetFormattedString().Trim();

            if (string.IsNullOrWhiteSpace(jobNumber) || string.IsNullOrWhiteSpace(departmentName) || string.IsNullOrWhiteSpace(fullName))
            {
                continue;
            }

            if (!departments.TryGetValue(departmentName, out var departmentId))
            {
                _logger.LogWarning("Unknown department in users excel: {DepartmentName}", departmentName);
                continue;
            }

            var effectivePassword = string.IsNullOrWhiteSpace(password) ? _options.DefaultPassword : password;
            var isSuperAdmin = jobNumber == _options.SuperAdminJobNumber;

            if (existingUsers.TryGetValue(jobNumber, out var user))
            {
                user.FullName = fullName;
                user.DepartmentId = departmentId;
                user.IsEnabled = true;
                user.IsSuperAdmin = isSuperAdmin;
                user.UpdatedAt = now;
                continue;
            }

            var (hash, salt) = PasswordHasher.Hash(effectivePassword);
            var newUser = new User
            {
                JobNumber = jobNumber,
                FullName = fullName,
                DepartmentId = departmentId,
                PasswordHash = hash,
                PasswordSalt = salt,
                IsSuperAdmin = isSuperAdmin,
                IsEnabled = true,
                CreatedAt = now
            };

            await _dbContext.Users.AddAsync(newUser, cancellationToken);
        }

        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    private async Task<List<string>> ReadDepartmentNamesFromExcelAsync(CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(_options.UsersExcelPath) || !File.Exists(_options.UsersExcelPath))
        {
            _logger.LogWarning("Users Excel file not found: {Path}", _options.UsersExcelPath);
            return [];
        }

        await Task.Yield();

        using var workbook = new XLWorkbook(_options.UsersExcelPath);
        var worksheet = workbook.Worksheet(1);
        var lastRow = worksheet.LastRowUsed()?.RowNumber() ?? 0;
        var names = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        for (var row = 2; row <= lastRow; row++)
        {
            var departmentName = worksheet.Cell(row, 3).GetFormattedString().Trim();
            if (!string.IsNullOrWhiteSpace(departmentName))
            {
                names.Add(departmentName);
            }
        }

        return names.OrderBy(x => x).ToList();
    }
}
