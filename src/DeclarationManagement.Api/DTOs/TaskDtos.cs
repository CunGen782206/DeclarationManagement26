namespace DeclarationManagement.Api.DTOs;

public class TaskDto
{
    public long Id { get; set; }
    public string TaskName { get; set; } = string.Empty;
    public DateTime StartAt { get; set; }
    public DateTime EndAt { get; set; }
    public bool IsEnabled { get; set; }
}

public class CreateTaskRequestDto
{
    public string TaskName { get; set; } = string.Empty;
    public DateTime StartAt { get; set; }
    public DateTime EndAt { get; set; }
}

public class UpdateTaskWindowRequestDto
{
    public DateTime StartAt { get; set; }
    public DateTime EndAt { get; set; }
}

public class UpdateTaskStatusRequestDto
{
    public bool IsEnabled { get; set; }
}
