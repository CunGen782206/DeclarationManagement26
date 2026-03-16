using DeclarationManagement.Api.Entities;

namespace DeclarationManagement.Api.DTOs;

/// <summary>
/// Save申报请求数据传输对象类。
/// </summary>
public class SaveDeclarationRequestDto
{
    /// <summary>
    /// 任务ID属性。
    /// </summary>
    public long TaskId { get; set; }
    /// <summary>
    /// 主体名称属性。
    /// </summary>
    public string PrincipalName { get; set; } = string.Empty;
    /// <summary>
    /// 联系方式电话属性。
    /// </summary>
    public string ContactPhone { get; set; } = string.Empty;
    /// <summary>
    /// 部门ID属性。
    /// </summary>
    public long DepartmentId { get; set; }
    /// <summary>
    /// 项目名称属性。
    /// </summary>
    public string ProjectName { get; set; } = string.Empty;
    /// <summary>
    /// 项目类别ID属性。
    /// </summary>
    public long ProjectCategoryId { get; set; }
    /// <summary>
    /// 项目等级属性。
    /// </summary>
    public ProjectLevel ProjectLevel { get; set; }
    /// <summary>
    /// 奖项等级属性。
    /// </summary>
    public AwardLevel AwardLevel { get; set; }
    /// <summary>
    /// 参与类型属性。
    /// </summary>
    public ParticipationType ParticipationType { get; set; }
    /// <summary>
    /// 认定批文文档名称属性。
    /// </summary>
    public string? ApprovalDocumentName { get; set; }
    /// <summary>
    /// 盖章单位And日期属性。
    /// </summary>
    public string? SealUnitAndDate { get; set; }
    /// <summary>
    /// 项目内容属性。
    /// </summary>
    public string? ProjectContent { get; set; }
    /// <summary>
    /// 项目成果属性。
    /// </summary>
    public string? ProjectAchievement { get; set; }
}

/// <summary>
/// 申报Submit请求数据传输对象类。
/// </summary>
public class DeclarationSubmitRequestDto
{
    /// <summary>
    /// 申报ID属性。
    /// </summary>
    public long DeclarationId { get; set; }
}

/// <summary>
/// 申报Resubmit请求数据传输对象类。
/// </summary>
public class DeclarationResubmitRequestDto
{
    /// <summary>
    /// 申报ID属性。
    /// </summary>
    public long DeclarationId { get; set; }
}

/// <summary>
/// 申报Detail数据传输对象类。
/// </summary>
public class DeclarationDetailDto
{
    /// <summary>
    /// ID属性。
    /// </summary>
    public long Id { get; set; }
    /// <summary>
    /// 任务ID属性。
    /// </summary>
    public long TaskId { get; set; }
    /// <summary>
    /// 任务名称属性。
    /// </summary>
    public string TaskName { get; set; } = string.Empty;
    /// <summary>
    /// 申报人名称属性。
    /// </summary>
    public string ApplicantName { get; set; } = string.Empty;
    /// <summary>
    /// 主体名称属性。
    /// </summary>
    public string PrincipalName { get; set; } = string.Empty;
    /// <summary>
    /// 联系方式电话属性。
    /// </summary>
    public string ContactPhone { get; set; } = string.Empty;
    /// <summary>
    /// 部门ID属性。
    /// </summary>
    public long DepartmentId { get; set; }
    /// <summary>
    /// 部门名称属性。
    /// </summary>
    public string DepartmentName { get; set; } = string.Empty;
    /// <summary>
    /// 项目类别ID属性。
    /// </summary>
    public long ProjectCategoryId { get; set; }
    /// <summary>
    /// 项目类别名称属性。
    /// </summary>
    public string ProjectCategoryName { get; set; } = string.Empty;
    /// <summary>
    /// 项目名称属性。
    /// </summary>
    public string ProjectName { get; set; } = string.Empty;
    /// <summary>
    /// 项目等级属性。
    /// </summary>
    public ProjectLevel ProjectLevel { get; set; }
    /// <summary>
    /// 奖项等级属性。
    /// </summary>
    public AwardLevel AwardLevel { get; set; }
    /// <summary>
    /// 参与类型属性。
    /// </summary>
    public ParticipationType ParticipationType { get; set; }
    /// <summary>
    /// 当前状态属性。
    /// </summary>
    public DeclarationStatus CurrentStatus { get; set; }
    /// <summary>
    /// 当前节点属性。
    /// </summary>
    public DeclarationNode CurrentNode { get; set; }
    /// <summary>
    /// 版本No属性。
    /// </summary>
    public int VersionNo { get; set; }
    /// <summary>
    /// 提交时间属性。
    /// </summary>
    public DateTime? SubmittedAt { get; set; }
}

/// <summary>
/// 申报分页查询数据传输对象类。
/// </summary>
public class DeclarationPageQueryDto
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
/// 申报列表Item数据传输对象类。
/// </summary>
public class DeclarationListItemDto
{
    /// <summary>
    /// ID属性。
    /// </summary>
    public long Id { get; set; }
    /// <summary>
    /// 项目名称属性。
    /// </summary>
    public string ProjectName { get; set; } = string.Empty;
    /// <summary>
    /// 项目类别名称属性。
    /// </summary>
    public string ProjectCategoryName { get; set; } = string.Empty;
    /// <summary>
    /// 部门名称属性。
    /// </summary>
    public string DepartmentName { get; set; } = string.Empty;
    /// <summary>
    /// 主体名称属性。
    /// </summary>
    public string PrincipalName { get; set; } = string.Empty;
    /// <summary>
    /// 当前状态属性。
    /// </summary>
    public DeclarationStatus CurrentStatus { get; set; }
    /// <summary>
    /// 提交时间属性。
    /// </summary>
    public DateTime? SubmittedAt { get; set; }
    /// <summary>
    /// 动作属性。
    /// </summary>
    public string Action { get; set; } = string.Empty;
}

/// <summary>
/// 附件数据传输对象类。
/// </summary>
public class AttachmentDto
{
    /// <summary>
    /// ID属性。
    /// </summary>
    public long Id { get; set; }
    /// <summary>
    /// 原始文件名称属性。
    /// </summary>
    public string OriginalFileName { get; set; } = string.Empty;
    /// <summary>
    /// 文件大小字节属性。
    /// </summary>
    public long FileSizeBytes { get; set; }
    /// <summary>
    /// Uploaded时间属性。
    /// </summary>
    public DateTime UploadedAt { get; set; }
}

/// <summary>
/// Paged结果数据传输对象类。
/// </summary>
public class PagedResultDto<T>
{
    /// <summary>
    /// 分页页码属性。
    /// </summary>
    public int PageIndex { get; set; }
    /// <summary>
    /// 分页大小属性。
    /// </summary>
    public int PageSize { get; set; }
    /// <summary>
    /// 总数Count属性。
    /// </summary>
    public long TotalCount { get; set; }
    /// <summary>
    /// 项属性。
    /// </summary>
    public List<T> Items { get; set; } = new();
}
