using DeclarationManagement.Api.Entities;

namespace DeclarationManagement.Api.DTOs;

/// <summary>
/// 统计查询数据传输对象类。
/// </summary>
public class StatisticsQueryDto
{
    /// <summary>
    /// 任务ID属性。
    /// </summary>
    public long? TaskId { get; set; }
    /// <summary>
    /// 开始日期属性。
    /// </summary>
    public DateTime? StartDate { get; set; }
    /// <summary>
    /// 结束日期属性。
    /// </summary>
    public DateTime? EndDate { get; set; }
    /// <summary>
    /// 部门Ids属性。
    /// </summary>
    public List<long>? DepartmentIds { get; set; }
    /// <summary>
    /// 类别Ids属性。
    /// </summary>
    public List<long>? CategoryIds { get; set; }
    /// <summary>
    /// Statuses属性。
    /// </summary>
    public List<DeclarationStatus>? Statuses { get; set; }
}

/// <summary>
/// 统计Item数据传输对象类。
/// </summary>
public class StatisticsItemDto
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
    /// 申报人名称属性。
    /// </summary>
    public string ApplicantName { get; set; } = string.Empty;
    /// <summary>
    /// 联系方式电话属性。
    /// </summary>
    public string ContactPhone { get; set; } = string.Empty;
    /// <summary>
    /// 状态属性。
    /// </summary>
    public DeclarationStatus Status { get; set; }
    /// <summary>
    /// 提交时间属性。
    /// </summary>
    public DateTime? SubmittedAt { get; set; }
}

/// <summary>
/// 导出文件数据传输对象类。
/// </summary>
public class ExportFileDto
{
    /// <summary>
    /// 文件名称属性。
    /// </summary>
    public string FileName { get; set; } = string.Empty;
    /// <summary>
    /// 内容类型属性。
    /// </summary>
    public string ContentType { get; set; } = string.Empty;
    /// <summary>
    /// 内容属性。
    /// </summary>
    public byte[] Content { get; set; } = [];
}
