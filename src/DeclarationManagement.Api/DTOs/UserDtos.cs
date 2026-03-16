namespace DeclarationManagement.Api.DTOs;

/// <summary>
/// 用户列表/详情 DTO。
/// </summary>
public class UserDto
{
    public long Id { get; set; }
    public string JobNumber { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public long DepartmentId { get; set; }
    public string DepartmentName { get; set; } = string.Empty;
    public bool IsSuperAdmin { get; set; }
    public bool IsEnabled { get; set; }
}
