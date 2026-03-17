using DeclarationManagement.Api.Entities;

namespace DeclarationManagement.Api.DTOs;

/// <summary>
/// 审核动作请求数据传输对象类。
/// </summary>
public class ReviewActionRequestDto
{
    /// <summary>
    /// 申报ID属性。
    /// </summary>
    public long DeclarationId { get; set; }
    /// <summary>
    /// 审核Stage属性。
    /// </summary>
    public ReviewStage ReviewStage { get; set; }
    /// <summary>
    /// 审核动作属性。
    /// </summary>
    public ReviewAction ReviewAction { get; set; }
    /// <summary>
    /// 原因属性。
    /// </summary>
    public string? Reason { get; set; }
    /// <summary>
    /// 认定项目等级属性。
    /// </summary>
    public ProjectLevel? RecognizedProjectLevel { get; set; }
    /// <summary>
    /// 认定奖项等级属性。
    /// </summary>
    public AwardLevel? RecognizedAwardLevel { get; set; }
    /// <summary>
    /// 认定金额属性。
    /// </summary>
    public decimal? RecognizedAmount { get; set; }
    /// <summary>
    /// 备注属性。
    /// </summary>
    public string? Remark { get; set; }
}

/// <summary>
/// 审核记录数据传输对象类。
/// </summary>
public class ReviewRecordDto
{
    /// <summary>
    /// ID属性。
    /// </summary>
    public long Id { get; set; }
    /// <summary>
    /// 申报ID属性。
    /// </summary>
    public long DeclarationId { get; set; }
    /// <summary>
    /// 审核Stage属性。
    /// </summary>
    public ReviewStage ReviewStage { get; set; }
    /// <summary>
    /// 审核动作属性。
    /// </summary>
    public ReviewAction ReviewAction { get; set; }
    /// <summary>
    /// 原因属性。
    /// </summary>
    public string? Reason { get; set; }
    /// <summary>
    /// 认定项目等级属性。
    /// </summary>
    public ProjectLevel? RecognizedProjectLevel { get; set; }
    /// <summary>
    /// 认定奖项等级属性。
    /// </summary>
    public AwardLevel? RecognizedAwardLevel { get; set; }
    /// <summary>
    /// 认定金额属性。
    /// </summary>
    public decimal? RecognizedAmount { get; set; }
    /// <summary>
    /// 备注属性。
    /// </summary>
    public string? Remark { get; set; }
    /// <summary>
    /// ReviewedBy用户ID属性。
    /// </summary>
    public long ReviewedByUserId { get; set; }
    /// <summary>
    /// Reviewed时间属性。
    /// </summary>
    public DateTime ReviewedAt { get; set; }
}

/// <summary>
/// 待处理审核查询数据传输对象类。
/// </summary>
public class PendingReviewQueryDto
{
    /// <summary>
    /// 分页页码属性。
    /// </summary>
    public int PageIndex { get; set; } = 1;
    /// <summary>
    /// 分页大小属性。
    /// </summary>
    public int PageSize { get; set; } = 10;
    /// <summary>
    /// 开始日期属性。
    /// </summary>
    public DateTime? StartDate { get; set; }
    /// <summary>
    /// 结束日期属性。
    /// </summary>
    public DateTime? EndDate { get; set; }
}

/// <summary>
/// 待处理审核Item数据传输对象类。
/// </summary>
public class PendingReviewItemDto
{
    /// <summary>
    /// 申报ID属性。
    /// </summary>
    public long DeclarationId { get; set; }
    /// <summary>
    /// 项目名称属性。
    /// </summary>
    public string ProjectName { get; set; } = string.Empty;
    /// <summary>
    /// 部门名称属性。
    /// </summary>
    public string DepartmentName { get; set; } = string.Empty;
    /// <summary>
    /// 当前审核Stage属性。
    /// </summary>
    public ReviewStage CurrentReviewStage { get; set; }
    /// <summary>
    /// 提交时间属性。
    /// </summary>
    public DateTime? SubmittedAt { get; set; }
}
