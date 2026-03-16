using AutoMapper;
using DeclarationManagement.Api.Data;
using DeclarationManagement.Api.DTOs;
using DeclarationManagement.Api.Entities;
using Microsoft.EntityFrameworkCore;

namespace DeclarationManagement.Api.Services;

public class DeclarationService : IDeclarationService
{
    private readonly AppDbContext _dbContext;
    private readonly IMapper _mapper;
    private readonly IFileStorageService _fileStorageService;

    public DeclarationService(AppDbContext dbContext, IMapper mapper, IFileStorageService fileStorageService)
    {
        _dbContext = dbContext;
        _mapper = mapper;
        _fileStorageService = fileStorageService;
    }

    public async Task<DeclarationDetailDto?> GetDetailAsync(long declarationId, long currentUserId, CancellationToken cancellationToken = default)
    {
        var declaration = await _dbContext.Declarations
            .AsNoTracking()
            .Include(x => x.Task)
            .Include(x => x.ApplicantUser)
            .Include(x => x.Department)
            .Include(x => x.ProjectCategory)
            .FirstOrDefaultAsync(x => x.Id == declarationId, cancellationToken);

        return declaration == null ? null : _mapper.Map<DeclarationDetailDto>(declaration);
    }

    public async Task<long> CreateAsync(long applicantUserId, SaveDeclarationRequestDto request, CancellationToken cancellationToken = default)
    {
        var entity = _mapper.Map<Declaration>(request);
        entity.ApplicantUserId = applicantUserId;
        entity.CurrentStatus = DeclarationStatus.Draft;
        entity.CurrentNode = DeclarationNode.Declaration;
        entity.CreatedAt = DateTime.UtcNow;

        await _dbContext.Declarations.AddAsync(entity, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }

    public async Task UpdateAsync(long declarationId, long applicantUserId, SaveDeclarationRequestDto request, CancellationToken cancellationToken = default)
    {
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

    public async Task SubmitAsync(long applicantUserId, DeclarationSubmitRequestDto request, CancellationToken cancellationToken = default)
    {
        var entity = await _dbContext.Declarations.FirstOrDefaultAsync(x => x.Id == request.DeclarationId && x.ApplicantUserId == applicantUserId, cancellationToken)
                    ?? throw new InvalidOperationException("申报单不存在");

        if (entity.CurrentStatus is not (DeclarationStatus.Draft or DeclarationStatus.PreReviewRejected or DeclarationStatus.InitialReviewRejected))
        {
            throw new InvalidOperationException("当前状态不可提交");
        }

        var task = await _dbContext.DeclarationTasks.FirstOrDefaultAsync(x => x.Id == entity.TaskId, cancellationToken)
                   ?? throw new InvalidOperationException("申报任务不存在");

        var now = DateTime.UtcNow;
        if (!task.IsEnabled || now < task.StartAt || now > task.EndAt)
        {
            throw new InvalidOperationException($"不在申报时间窗内：{task.StartAt:yyyy-MM-dd HH:mm:ss} ~ {task.EndAt:yyyy-MM-dd HH:mm:ss}");
        }

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

    public Task ResubmitAsync(long applicantUserId, DeclarationResubmitRequestDto request, CancellationToken cancellationToken = default)
    {
        return SubmitAsync(applicantUserId, new DeclarationSubmitRequestDto { DeclarationId = request.DeclarationId }, cancellationToken);
    }

    public async Task<PagedResultDto<DeclarationListItemDto>> GetMyDeclarationsAsync(long applicantUserId, DeclarationPageQueryDto query, CancellationToken cancellationToken = default)
    {
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

        var total = await q.LongCountAsync(cancellationToken);
        var list = await q.OrderByDescending(x => x.SubmittedAt ?? x.CreatedAt)
            .Skip((query.PageIndex - 1) * query.PageSize)
            .Take(query.PageSize)
            .ToListAsync(cancellationToken);

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

    public async Task<long> UploadAttachmentAsync(long declarationId, long uploaderId, IFormFile file, CancellationToken cancellationToken = default)
    {
        var declaration = await _dbContext.Declarations.FirstOrDefaultAsync(x => x.Id == declarationId, cancellationToken)
                          ?? throw new InvalidOperationException("申报单不存在");

        if (declaration.ApplicantUserId != uploaderId)
        {
            throw new InvalidOperationException("仅申报人可上传附件");
        }

        var saved = await _fileStorageService.SaveAsync(file, $"declarations/{declarationId}", cancellationToken);
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

    public async Task<(byte[] Content, string FileName, string ContentType)> DownloadAttachmentAsync(long attachmentId, long currentUserId, CancellationToken cancellationToken = default)
    {
        var attachment = await _dbContext.DeclarationAttachments
            .AsNoTracking()
            .Include(x => x.Declaration)
            .FirstOrDefaultAsync(x => x.Id == attachmentId && !x.IsDeleted, cancellationToken)
            ?? throw new InvalidOperationException("附件不存在");

        var bytes = await _fileStorageService.ReadAsync(attachment.StoragePath, cancellationToken);
        return (bytes, attachment.OriginalFileName, attachment.ContentType ?? "application/octet-stream");
    }

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
