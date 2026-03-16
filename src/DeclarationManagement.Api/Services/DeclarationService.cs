using AutoMapper;
using DeclarationManagement.Api.Data;
using DeclarationManagement.Api.DTOs;
using DeclarationManagement.Api.Entities;
using Microsoft.EntityFrameworkCore;

namespace DeclarationManagement.Api.Services;

/// <summary>
/// DeclarationService 类。
/// </summary>
public class DeclarationService : IDeclarationService
{
    /// <summary>
    /// 数据库上下文，用于执行申报相关持久化操作。
    /// </summary>
    private readonly AppDbContext _dbContext;
    /// <summary>
    /// 对象映射器，用于 Entity 与 DTO 的转换。
    /// </summary>
    private readonly IMapper _mapper;
    /// <summary>
    /// 文件存储服务，用于附件保存与读取。
    /// </summary>
    private readonly IFileStorageService _fileStorageService;

    /// <summary>
    /// 构造函数。
    /// </summary>
    public DeclarationService(AppDbContext dbContext, IMapper mapper, IFileStorageService fileStorageService)
    {
        _dbContext = dbContext;
        _mapper = mapper;
        _fileStorageService = fileStorageService;
    }

    /// <summary>
    /// 查询申报详情。
    /// </summary>
    public async Task<DeclarationDetailDto?> GetDetailAsync(long declarationId, long currentUserId, CancellationToken cancellationToken = default)
    {
        // declaration：数据库中的申报实体（包含任务、申报人、部门、类别等关联信息）
        var declaration = await _dbContext.Declarations
            .AsNoTracking()
            .Include(x => x.Task)
            .Include(x => x.ApplicantUser)
            .Include(x => x.Department)
            .Include(x => x.ProjectCategory)
            .FirstOrDefaultAsync(x => x.Id == declarationId, cancellationToken);

        return declaration == null ? null : _mapper.Map<DeclarationDetailDto>(declaration);
    }

    /// <summary>
    /// 创建申报草稿。
    /// </summary>
    public async Task<long> CreateAsync(long applicantUserId, SaveDeclarationRequestDto request, CancellationToken cancellationToken = default)
    {
        // entity：待持久化的申报主表实体
        var entity = _mapper.Map<Declaration>(request);
        entity.ApplicantUserId = applicantUserId;
        entity.CurrentStatus = DeclarationStatus.Draft;
        entity.CurrentNode = DeclarationNode.Declaration;
        entity.CreatedAt = DateTime.UtcNow;

        await _dbContext.Declarations.AddAsync(entity, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }

    /// <summary>
    /// 修改申报草稿或被驳回后的申报单。
    /// </summary>
    public async Task UpdateAsync(long declarationId, long applicantUserId, SaveDeclarationRequestDto request, CancellationToken cancellationToken = default)
    {
        // entity：当前用户可操作的目标申报单
        var entity = await _dbContext.Declarations.FirstOrDefaultAsync(x => x.Id == declarationId && x.ApplicantUserId == applicantUserId, cancellationToken)
                    ?? throw new InvalidOperationException("申报单不存在");

        if (entity.CurrentStatus is not (DeclarationStatus.Draft or DeclarationStatus.PreReviewRejected or DeclarationStatus.InitialReviewRejected))
        {
            throw new InvalidOperationException("当前状态不可修改");
        }

        entity.TaskId = request.TaskId;
        entity.PrincipalName = request.PrincipalName;
        entity.ContactPhone = request.ContactPhone;
        entity.DepartmentId = request.DepartmentId;
        entity.ProjectName = request.ProjectName;
        entity.ProjectCategoryId = request.ProjectCategoryId;
        entity.ProjectLevel = request.ProjectLevel;
        entity.AwardLevel = request.AwardLevel;
        entity.ParticipationType = request.ParticipationType;
        entity.ApprovalDocumentName = request.ApprovalDocumentName;
        entity.SealUnitAndDate = request.SealUnitAndDate;
        entity.ProjectContent = request.ProjectContent;
        entity.ProjectAchievement = request.ProjectAchievement;
        entity.VersionNo += 1;
        entity.UpdatedAt = DateTime.UtcNow;

        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    /// <summary>
    /// 提交申报单进入“待预审核”流程。
    /// </summary>
    public async Task SubmitAsync(long applicantUserId, DeclarationSubmitRequestDto request, CancellationToken cancellationToken = default)
    {
        // entity：当前提交的申报单
        var entity = await _dbContext.Declarations.FirstOrDefaultAsync(x => x.Id == request.DeclarationId && x.ApplicantUserId == applicantUserId, cancellationToken)
                    ?? throw new InvalidOperationException("申报单不存在");

        if (entity.CurrentStatus is not (DeclarationStatus.Draft or DeclarationStatus.PreReviewRejected or DeclarationStatus.InitialReviewRejected))
        {
            throw new InvalidOperationException("当前状态不可提交");
        }

        // task：申报任务（包含时间窗与启用状态）
        var task = await _dbContext.DeclarationTasks.FirstOrDefaultAsync(x => x.Id == entity.TaskId, cancellationToken)
                   ?? throw new InvalidOperationException("申报任务不存在");

        // now：统一时间戳，保证状态与日志时间一致
        var now = DateTime.UtcNow;
        if (!task.IsEnabled || now < task.StartAt || now > task.EndAt)
        {
            throw new InvalidOperationException($"不在申报时间窗内：{task.StartAt:yyyy-MM-dd HH:mm:ss} ~ {task.EndAt:yyyy-MM-dd HH:mm:ss}");
        }

        // fromStatus：记录提交前状态，用于流程日志追踪
        var fromStatus = entity.CurrentStatus;
        entity.CurrentStatus = DeclarationStatus.PendingPreReview;
        entity.CurrentNode = DeclarationNode.PreReview;
        entity.SubmittedAt = now;
        entity.UpdatedAt = now;

        await _dbContext.DeclarationFlowLogs.AddAsync(new DeclarationFlowLog
        {
            DeclarationId = entity.Id,
            FromStatus = fromStatus,
            ToStatus = DeclarationStatus.PendingPreReview,
            ActionType = fromStatus == DeclarationStatus.Draft ? FlowActionType.Submit : FlowActionType.Resubmit,
            OperatorUserId = applicantUserId,
            CreatedAt = now
        }, cancellationToken);

        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    /// <summary>
    /// 驳回后重提，本质复用提交逻辑。
    /// </summary>
    public Task ResubmitAsync(long applicantUserId, DeclarationResubmitRequestDto request, CancellationToken cancellationToken = default)
    {
        return SubmitAsync(applicantUserId, new DeclarationSubmitRequestDto { DeclarationId = request.DeclarationId }, cancellationToken);
    }

    /// <summary>
    /// 分页查询当前申报人的历史申报列表。
    /// </summary>
    public async Task<PagedResultDto<DeclarationListItemDto>> GetMyDeclarationsAsync(long applicantUserId, DeclarationPageQueryDto query, CancellationToken cancellationToken = default)
    {
        // q：可叠加筛选条件的查询对象
        var q = _dbContext.Declarations
            .AsNoTracking()
            .Include(x => x.Department)
            .Include(x => x.ProjectCategory)
            .Where(x => x.ApplicantUserId == applicantUserId);

        if (query.StartDate.HasValue)
        {
            q = q.Where(x => x.SubmittedAt >= query.StartDate.Value);
        }

        if (query.EndDate.HasValue)
        {
            q = q.Where(x => x.SubmittedAt <= query.EndDate.Value);
        }

        if (query.DepartmentIds is { Count: > 0 })
        {
            q = q.Where(x => query.DepartmentIds.Contains(x.DepartmentId));
        }

        if (query.CategoryIds is { Count: > 0 })
        {
            q = q.Where(x => query.CategoryIds.Contains(x.ProjectCategoryId));
        }

        if (query.Statuses is { Count: > 0 })
        {
            q = q.Where(x => query.Statuses.Contains(x.CurrentStatus));
        }

        // total：筛选后的总记录数，用于分页展示
        var total = await q.LongCountAsync(cancellationToken);
        // list：当前页数据
        var list = await q.OrderByDescending(x => x.SubmittedAt ?? x.CreatedAt)
            .Skip((query.PageIndex - 1) * query.PageSize)
            .Take(query.PageSize)
            .ToListAsync(cancellationToken);

        // items：返回前端的列表 DTO（含“查看/修改”操作文案）
        var items = list.Select(x => new DeclarationListItemDto
        {
            Id = x.Id,
            ProjectName = x.ProjectName,
            ProjectCategoryName = x.ProjectCategory?.Name ?? string.Empty,
            DepartmentName = x.Department?.Name ?? string.Empty,
            PrincipalName = x.PrincipalName,
            CurrentStatus = x.CurrentStatus,
            SubmittedAt = x.SubmittedAt,
            Action = x.CurrentStatus is DeclarationStatus.PreReviewRejected or DeclarationStatus.InitialReviewRejected ? "修改" : "查看"
        }).ToList();

        return new PagedResultDto<DeclarationListItemDto>
        {
            PageIndex = query.PageIndex,
            PageSize = query.PageSize,
            TotalCount = total,
            Items = items
        };
    }

    /// <summary>
    /// 上传附件到本地存储并写入附件记录。
    /// </summary>
    public async Task<long> UploadAttachmentAsync(long declarationId, long uploaderId, IFormFile file, CancellationToken cancellationToken = default)
    {
        // declaration：附件归属的申报单
        var declaration = await _dbContext.Declarations.FirstOrDefaultAsync(x => x.Id == declarationId, cancellationToken)
                          ?? throw new InvalidOperationException("申报单不存在");

        if (declaration.ApplicantUserId != uploaderId)
        {
            throw new InvalidOperationException("仅申报人可上传附件");
        }

        // saved：文件系统落盘结果（存储路径/名称/大小）
        var saved = await _fileStorageService.SaveAsync(file, $"declarations/{declarationId}", cancellationToken);
        // attachment：附件表记录
        var attachment = new DeclarationAttachment
        {
            DeclarationId = declarationId,
            OriginalFileName = file.FileName,
            StorageFileName = saved.StorageFileName,
            StoragePath = saved.StoragePath,
            FileSizeBytes = saved.Size,
            ContentType = file.ContentType,
            UploadedByUserId = uploaderId,
            UploadedAt = DateTime.UtcNow,
            CreatedAt = DateTime.UtcNow,
            IsDeleted = false
        };

        await _dbContext.DeclarationAttachments.AddAsync(attachment, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return attachment.Id;
    }

    /// <summary>
    /// 下载附件文件内容。
    /// </summary>
    public async Task<(byte[] Content, string FileName, string ContentType)> DownloadAttachmentAsync(long attachmentId, long currentUserId, CancellationToken cancellationToken = default)
    {
        // attachment：附件元数据（文件名、路径、类型）
        var attachment = await _dbContext.DeclarationAttachments
            .AsNoTracking()
            .Include(x => x.Declaration)
            .FirstOrDefaultAsync(x => x.Id == attachmentId && !x.IsDeleted, cancellationToken)
            ?? throw new InvalidOperationException("附件不存在");

        // bytes：附件二进制内容
        var bytes = await _fileStorageService.ReadAsync(attachment.StoragePath, cancellationToken);
        return (bytes, attachment.OriginalFileName, attachment.ContentType ?? "application/octet-stream");
    }

    /// <summary>
    /// 查询申报单下的附件列表。
    /// </summary>
    public async Task<List<AttachmentDto>> GetAttachmentsAsync(long declarationId, long currentUserId, CancellationToken cancellationToken = default)
    {
        return await _dbContext.DeclarationAttachments
            .AsNoTracking()
            .Where(x => x.DeclarationId == declarationId && !x.IsDeleted)
            .OrderByDescending(x => x.UploadedAt)
            .Select(x => new AttachmentDto
            {
                Id = x.Id,
                OriginalFileName = x.OriginalFileName,
                FileSizeBytes = x.FileSizeBytes,
                UploadedAt = x.UploadedAt
            }).ToListAsync(cancellationToken);
    }
}
