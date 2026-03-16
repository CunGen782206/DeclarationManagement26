using DeclarationManagement.Api.Data;
using DeclarationManagement.Api.DTOs;
using DeclarationManagement.Api.Entities;
using Microsoft.EntityFrameworkCore;

namespace DeclarationManagement.Api.Services;

public class TaskService : ITaskService
{
    private readonly AppDbContext _dbContext;

    public TaskService(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<TaskDto>> GetListAsync(CancellationToken cancellationToken = default)
    {
        return await _dbContext.DeclarationTasks
            .AsNoTracking()
            .OrderByDescending(x => x.CreatedAt)
            .Select(x => new TaskDto
            {
                Id = x.Id,
                TaskName = x.TaskName,
                StartAt = x.StartAt,
                EndAt = x.EndAt,
                IsEnabled = x.IsEnabled
            }).ToListAsync(cancellationToken);
    }

    public async Task<long> CreateAsync(long operatorUserId, CreateTaskRequestDto request, CancellationToken cancellationToken = default)
    {
        if (request.EndAt <= request.StartAt)
        {
            throw new InvalidOperationException("结束时间必须晚于开始时间");
        }

        var entity = new DeclarationTask
        {
            TaskName = request.TaskName,
            StartAt = request.StartAt,
            EndAt = request.EndAt,
            CreatedByUserId = operatorUserId,
            CreatedAt = DateTime.UtcNow,
            IsEnabled = true
        };

        await _dbContext.DeclarationTasks.AddAsync(entity, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return entity.Id;
    }

    public async Task UpdateWindowAsync(long taskId, UpdateTaskWindowRequestDto request, CancellationToken cancellationToken = default)
    {
        if (request.EndAt <= request.StartAt)
        {
            throw new InvalidOperationException("结束时间必须晚于开始时间");
        }

        var task = await _dbContext.DeclarationTasks.FirstOrDefaultAsync(x => x.Id == taskId, cancellationToken)
                   ?? throw new InvalidOperationException("申报任务不存在");

        task.StartAt = request.StartAt;
        task.EndAt = request.EndAt;
        task.UpdatedAt = DateTime.UtcNow;

        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateStatusAsync(long taskId, UpdateTaskStatusRequestDto request, CancellationToken cancellationToken = default)
    {
        var task = await _dbContext.DeclarationTasks.FirstOrDefaultAsync(x => x.Id == taskId, cancellationToken)
                   ?? throw new InvalidOperationException("申报任务不存在");
        task.IsEnabled = request.IsEnabled;
        task.UpdatedAt = DateTime.UtcNow;
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}
