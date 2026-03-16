namespace DeclarationManagement.Api.DTOs;

/// <summary>
/// TaskDto 类。
/// </summary>
public class TaskDto
{
    /// <summary>
    /// Id 属性。
    /// </summary>
    public long Id { get; set; }
    /// <summary>
    /// TaskName 属性。
    /// </summary>
    public string TaskName { get; set; } = string.Empty;
    /// <summary>
    /// StartAt 属性。
    /// </summary>
    public DateTime StartAt { get; set; }
    /// <summary>
    /// EndAt 属性。
    /// </summary>
    public DateTime EndAt { get; set; }
    /// <summary>
    /// IsEnabled 属性。
    /// </summary>
    public bool IsEnabled { get; set; }
}

/// <summary>
/// CreateTaskRequestDto 类。
/// </summary>
public class CreateTaskRequestDto
{
    /// <summary>
    /// TaskName 属性。
    /// </summary>
    public string TaskName { get; set; } = string.Empty;
    /// <summary>
    /// StartAt 属性。
    /// </summary>
    public DateTime StartAt { get; set; }
    /// <summary>
    /// EndAt 属性。
    /// </summary>
    public DateTime EndAt { get; set; }
}

/// <summary>
/// UpdateTaskWindowRequestDto 类。
/// </summary>
public class UpdateTaskWindowRequestDto
{
    /// <summary>
    /// StartAt 属性。
    /// </summary>
    public DateTime StartAt { get; set; }
    /// <summary>
    /// EndAt 属性。
    /// </summary>
    public DateTime EndAt { get; set; }
}

/// <summary>
/// UpdateTaskStatusRequestDto 类。
/// </summary>
public class UpdateTaskStatusRequestDto
{
    /// <summary>
    /// IsEnabled 属性。
    /// </summary>
    public bool IsEnabled { get; set; }
}
