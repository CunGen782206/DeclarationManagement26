using DeclarationManagement.Api.Entities;

namespace DeclarationManagement.Api.DTOs;

/// <summary>
/// StatisticsQueryDto 类。
/// </summary>
public class StatisticsQueryDto
{
    /// <summary>
    /// TaskId 属性。
    /// </summary>
    public long? TaskId { get; set; }
    /// <summary>
    /// StartDate 属性。
    /// </summary>
    public DateTime? StartDate { get; set; }
    /// <summary>
    /// EndDate 属性。
    /// </summary>
    public DateTime? EndDate { get; set; }
    /// <summary>
    /// DepartmentIds 属性。
    /// </summary>
    public List<long>? DepartmentIds { get; set; }
    /// <summary>
    /// CategoryIds 属性。
    /// </summary>
    public List<long>? CategoryIds { get; set; }
    /// <summary>
    /// Statuses 属性。
    /// </summary>
    public List<DeclarationStatus>? Statuses { get; set; }
}

/// <summary>
/// StatisticsItemDto 类。
/// </summary>
public class StatisticsItemDto
{
    /// <summary>
    /// DeclarationId 属性。
    /// </summary>
    public long DeclarationId { get; set; }
    /// <summary>
    /// ProjectName 属性。
    /// </summary>
    public string ProjectName { get; set; } = string.Empty;
    /// <summary>
    /// DepartmentName 属性。
    /// </summary>
    public string DepartmentName { get; set; } = string.Empty;
    /// <summary>
    /// ApplicantName 属性。
    /// </summary>
    public string ApplicantName { get; set; } = string.Empty;
    /// <summary>
    /// ContactPhone 属性。
    /// </summary>
    public string ContactPhone { get; set; } = string.Empty;
    /// <summary>
    /// Status 属性。
    /// </summary>
    public DeclarationStatus Status { get; set; }
    /// <summary>
    /// SubmittedAt 属性。
    /// </summary>
    public DateTime? SubmittedAt { get; set; }
}

/// <summary>
/// ExportFileDto 类。
/// </summary>
public class ExportFileDto
{
    /// <summary>
    /// FileName 属性。
    /// </summary>
    public string FileName { get; set; } = string.Empty;
    /// <summary>
    /// ContentType 属性。
    /// </summary>
    public string ContentType { get; set; } = string.Empty;
    /// <summary>
    /// Content 属性。
    /// </summary>
    public byte[] Content { get; set; } = [];
}
