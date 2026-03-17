namespace DeclarationManagement.Api.DTOs;

public class UserDto
{
    public long Id { get; set; }
    public string JobNumber { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public long DepartmentId { get; set; }
    public string DepartmentName { get; set; } = string.Empty;
    public bool IsSuperAdmin { get; set; }
    public bool IsEnabled { get; set; }
    public List<long> PreReviewDepartmentIds { get; set; } = new();
    public List<long> InitialReviewCategoryIds { get; set; } = new();
}

public class UserQueryDto
{
    public string? JobNumber { get; set; }
    public string? FullName { get; set; }
    public List<long>? DepartmentIds { get; set; }
}

public class CreateUserRequestDto
{
    public string JobNumber { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public long DepartmentId { get; set; }
    public bool IsSuperAdmin { get; set; }
    public List<long> PreReviewDepartmentIds { get; set; } = new();
    public List<long> InitialReviewCategoryIds { get; set; } = new();
}

public class UpdateUserRequestDto
{
    public string FullName { get; set; } = string.Empty;
    public long DepartmentId { get; set; }
    public bool IsEnabled { get; set; }
    public bool IsSuperAdmin { get; set; }
    public List<long> PreReviewDepartmentIds { get; set; } = new();
    public List<long> InitialReviewCategoryIds { get; set; } = new();
}
