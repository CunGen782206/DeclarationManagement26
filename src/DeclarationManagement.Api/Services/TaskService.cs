using DeclarationManagement.Api.DTOs;
using DeclarationManagement.Api.Entities;
using DeclarationManagement.Api.Repositories;

namespace DeclarationManagement.Api.Services;

/// <summary>
/// 申报任务服务实现。
/// </summary>
public class TaskService : ITaskService
{
    private readonly IRepository<DeclarationTask> _taskRepository;

    public TaskService(IRepository<DeclarationTask> taskRepository)
    {
        _taskRepository = taskRepository;
    }

    public async Task<List<TaskDto>> GetListAsync(CancellationToken cancellationToken = default)
    {
        var tasks = await _taskRepository.GetListAsync(cancellationToken: cancellationToken);
        return tasks.Select(x => new TaskDto
        {
            Id = x.Id,
            TaskName = x.TaskName,
            StartAt = x.StartAt,
            EndAt = x.EndAt,
            IsEnabled = x.IsEnabled
        }).ToList();
    }

    public async Task<long> CreateAsync(long operatorUserId, CreateTaskRequestDto request, CancellationToken cancellationToken = default)
    {
        var entity = new DeclarationTask
        {
            TaskName = request.TaskName,
            StartAt = request.StartAt,
            EndAt = request.EndAt,
            CreatedByUserId = operatorUserId,
            CreatedAt = DateTime.UtcNow,
            IsEnabled = true
        };

        await _taskRepository.AddAsync(entity, cancellationToken);
        await _taskRepository.SaveChangesAsync(cancellationToken);
        return entity.Id;
    }

    public async Task UpdateWindowAsync(long taskId, UpdateTaskWindowRequestDto request, CancellationToken cancellationToken = default)
    {
        var task = await _taskRepository.GetByIdAsync(taskId, cancellationToken)
                   ?? throw new InvalidOperationException("申报任务不存在");

        task.StartAt = request.StartAt;
        task.EndAt = request.EndAt;
        task.UpdatedAt = DateTime.UtcNow;

        _taskRepository.Update(task);
        await _taskRepository.SaveChangesAsync(cancellationToken);
    }
}
