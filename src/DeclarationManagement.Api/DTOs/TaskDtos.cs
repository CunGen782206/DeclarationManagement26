namespace DeclarationManagement.Api.DTOs;

/// <summary>
/// 任务数据传输对象类。
/// </summary>
public class TaskDto
{
    /// <summary>
    /// ID属性。
    /// </summary>
    public long Id { get; set; }
    /// <summary>
    /// 任务名称属性。
    /// </summary>
    public string TaskName { get; set; } = string.Empty;
    /// <summary>
    /// 开始时间属性。
    /// </summary>
    public DateTime StartAt { get; set; }
    /// <summary>
    /// 结束时间属性。
    /// </summary>
    public DateTime EndAt { get; set; }
    /// <summary>
    /// 是否启用属性。
    /// </summary>
    public bool IsEnabled { get; set; }
}

/// <summary>
/// Create任务请求数据传输对象类。
/// </summary>
public class CreateTaskRequestDto
{
    /// <summary>
    /// 任务名称属性。
    /// </summary>
    public string TaskName { get; set; } = string.Empty;
    /// <summary>
    /// 开始时间属性。
    /// </summary>
    public DateTime StartAt { get; set; }
    /// <summary>
    /// 结束时间属性。
    /// </summary>
    public DateTime EndAt { get; set; }
}

/// <summary>
/// Update任务Window请求数据传输对象类。
/// </summary>
public class UpdateTaskWindowRequestDto
{
    /// <summary>
    /// 开始时间属性。
    /// </summary>
    public DateTime StartAt { get; set; }
    /// <summary>
    /// 结束时间属性。
    /// </summary>
    public DateTime EndAt { get; set; }
}

/// <summary>
/// Update任务状态请求数据传输对象类。
/// </summary>
public class UpdateTaskStatusRequestDto
{
    /// <summary>
    /// 是否启用属性。
    /// </summary>
    public bool IsEnabled { get; set; }
}
