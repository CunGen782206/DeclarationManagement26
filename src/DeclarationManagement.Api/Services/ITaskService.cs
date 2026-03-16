using DeclarationManagement.Api.DTOs;

namespace DeclarationManagement.Api.Services;

/// <summary>
/// ITask服务接口。
/// </summary>
public interface ITaskService
{
    Task<List<TaskDto>> GetListAsync(CancellationToken cancellationToken = default);
    Task<long> CreateAsync(long operatorUserId, CreateTaskRequestDto request, CancellationToken cancellationToken = default);
    Task UpdateWindowAsync(long taskId, UpdateTaskWindowRequestDto request, CancellationToken cancellationToken = default);
    Task UpdateStatusAsync(long taskId, UpdateTaskStatusRequestDto request, CancellationToken cancellationToken = default);
}
