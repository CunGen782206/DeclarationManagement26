using DeclarationManagement.Api.Common;
using DeclarationManagement.Api.DTOs;
using DeclarationManagement.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DeclarationManagement.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
/// <summary>
/// TasksController类。
/// </summary>
public class TasksController : ControllerBase
{
    /// <summary>
    /// 任务服务字段。
    /// </summary>
    private readonly ITaskService _taskService;

    /// <summary>
    /// 构造函数。
    /// </summary>
    public TasksController(ITaskService taskService)
    {
        _taskService = taskService;
    }

    [HttpGet]
    /// <summary>
    /// 获取数据。
    /// </summary>
    public async Task<ActionResult<ApiResponse<List<TaskDto>>>> GetList(CancellationToken cancellationToken)
    {
        var result = await _taskService.GetListAsync(cancellationToken); // result：结果
        return Ok(ApiResponse<List<TaskDto>>.Ok(result));
    }

    [HttpPost]
    /// <summary>
    /// 创建数据。
    /// </summary>
    public async Task<ActionResult<ApiResponse<long>>> Create([FromBody] CreateTaskRequestDto request, CancellationToken cancellationToken)
    {
        var id = await _taskService.CreateAsync(User.GetUserId(), request, cancellationToken); // id：ID
        return Ok(ApiResponse<long>.Ok(id, "创建成功"));
    }

    [HttpPut("{id:long}/window")]
    /// <summary>
    /// 更新数据。
    /// </summary>
    public async Task<ActionResult<ApiResponse<string>>> UpdateWindow(long id, [FromBody] UpdateTaskWindowRequestDto request, CancellationToken cancellationToken)
    {
        await _taskService.UpdateWindowAsync(id, request, cancellationToken);
        return Ok(ApiResponse<string>.Ok("OK", "修改成功"));
    }

    [HttpPut("{id:long}/status")]
    /// <summary>
    /// 更新数据。
    /// </summary>
    public async Task<ActionResult<ApiResponse<string>>> UpdateStatus(long id, [FromBody] UpdateTaskStatusRequestDto request, CancellationToken cancellationToken)
    {
        await _taskService.UpdateStatusAsync(id, request, cancellationToken);
        return Ok(ApiResponse<string>.Ok("OK", "修改成功"));
    }
}
