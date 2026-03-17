using DeclarationManagement.Api.Entities;

namespace DeclarationManagement.Api.DTOs;

public class StatisticsQueryDto
{
    public long? TaskId { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public List<long>? DepartmentIds { get; set; }
    public List<long>? CategoryIds { get; set; }
    public List<DeclarationStatus>? Statuses { get; set; }
}

public class StatisticsItemDto
{
    public long DeclarationId { get; set; }
    public string ProjectName { get; set; } = string.Empty;
    public string ProjectCategoryName { get; set; } = string.Empty;
    public string DepartmentName { get; set; } = string.Empty;
    public string ApplicantName { get; set; } = string.Empty;
    public ProjectLevel ProjectLevel { get; set; }
    public AwardLevel AwardLevel { get; set; }
    public ParticipationType ParticipationType { get; set; }
    public string ContactPhone { get; set; } = string.Empty;
    public string? SealUnitAndDate { get; set; }
    public string FinalReviewDepartmentName { get; set; } = string.Empty;
    public string StatusName { get; set; } = string.Empty;
    public string? ReviewReason { get; set; }
    public ProjectLevel? RecognizedProjectLevel { get; set; }
    public AwardLevel? RecognizedAwardLevel { get; set; }
    public decimal? RecognizedAmount { get; set; }
    public string? Remark { get; set; }
    public DeclarationStatus Status { get; set; }
    public DateTime? SubmittedAt { get; set; }
}

public class ExportFileDto
{
    public string FileName { get; set; } = string.Empty;
    public string ContentType { get; set; } = string.Empty;
    public byte[] Content { get; set; } = [];
}
