using DeclarationManagement.Api.DTOs;
using DeclarationManagement.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace DeclarationManagement.Api.Controllers;

/// <summary>
/// 申报任务控制器。
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class TasksController : ControllerBase
{
    private readonly ITaskService _taskService;

    public TasksController(ITaskService taskService)
    {
        _taskService = taskService;
    }

    [HttpGet]
    public async Task<ActionResult<ApiResponse<List<TaskDto>>>> GetList(CancellationToken cancellationToken)
    {
        var result = await _taskService.GetListAsync(cancellationToken);
        return Ok(ApiResponse<List<TaskDto>>.Ok(result));
    }

    [HttpPost]
    public async Task<ActionResult<ApiResponse<long>>> Create([FromBody] CreateTaskRequestDto request, CancellationToken cancellationToken)
    {
        const long currentUserId = 1;
        var id = await _taskService.CreateAsync(currentUserId, request, cancellationToken);
        return Ok(ApiResponse<long>.Ok(id, "创建成功"));
    }

    [HttpPut("{id:long}/window")]
    public async Task<ActionResult<ApiResponse<string>>> UpdateWindow(long id, [FromBody] UpdateTaskWindowRequestDto request, CancellationToken cancellationToken)
    {
        await _taskService.UpdateWindowAsync(id, request, cancellationToken);
        return Ok(ApiResponse<string>.Ok("OK", "修改成功"));
    }
}
