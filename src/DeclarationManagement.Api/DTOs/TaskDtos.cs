namespace DeclarationManagement.Api.DTOs;

/// <summary>
/// 申报任务 DTO。
/// </summary>
public class TaskDto
{
    public long Id { get; set; }
    public string TaskName { get; set; } = string.Empty;
    public DateTime StartAt { get; set; }
    public DateTime EndAt { get; set; }
    public bool IsEnabled { get; set; }
}

/// <summary>
/// 新建申报任务请求 DTO。
/// </summary>
public class CreateTaskRequestDto
{
    public string TaskName { get; set; } = string.Empty;
    public DateTime StartAt { get; set; }
    public DateTime EndAt { get; set; }
}

/// <summary>
/// 更新时间窗口请求 DTO。
/// </summary>
public class UpdateTaskWindowRequestDto
{
    public DateTime StartAt { get; set; }
    public DateTime EndAt { get; set; }
}
