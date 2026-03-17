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

        if (declaration == null)
        {
            return null;
        }

        await EnsureCanAccessDeclarationAsync(declaration, currentUserId, cancellationToken);
        return _mapper.Map<DeclarationDetailDto>(declaration);
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

        if (!string.IsNullOrWhiteSpace(request.TempAttachmentKey))
        {
            await BindTemporaryAttachmentsAsync(request.TempAttachmentKey, applicantUserId, entity.Id, cancellationToken);
        }

        return entity.Id;
    }

    public async Task UpdateAsync(long declarationId, long applicantUserId, SaveDeclarationRequestDto request, CancellationToken cancellationToken = default)
    {
        var entity = await _dbContext.Declarations.FirstOrDefaultAsync(
            x => x.Id == declarationId && x.ApplicantUserId == applicantUserId,
            cancellationToken) ?? throw new InvalidOperationException("申报单不存在");

        EnsureCanEdit(entity);
        ApplyDeclarationChanges(entity, request);
        entity.VersionNo += 1;
        entity.UpdatedAt = DateTime.UtcNow;

        await _dbContext.SaveChangesAsync(cancellationToken);

        if (!string.IsNullOrWhiteSpace(request.TempAttachmentKey))
        {
            await BindTemporaryAttachmentsAsync(request.TempAttachmentKey, applicantUserId, entity.Id, cancellationToken);
        }
    }

    public async Task<long> SubmitAsync(long applicantUserId, DeclarationSubmitRequestDto request, CancellationToken cancellationToken = default)
    {
        ValidateSubmitRequest(request);

        var now = DateTime.UtcNow;
        var isExisting = request.DeclarationId.HasValue;
        Declaration entity;

        if (isExisting)
        {
            entity = await _dbContext.Declarations.FirstOrDefaultAsync(
                x => x.Id == request.DeclarationId!.Value && x.ApplicantUserId == applicantUserId,
                cancellationToken) ?? throw new InvalidOperationException("申报单不存在");

            EnsureCanEdit(entity);
            ApplyDeclarationChanges(entity, request);
            entity.VersionNo += 1;
        }
        else
        {
            entity = new Declaration
            {
                ApplicantUserId = applicantUserId,
                VersionNo = 1,
                CurrentStatus = DeclarationStatus.Draft,
                CurrentNode = DeclarationNode.Declaration,
                CreatedAt = now
            };

            ApplyDeclarationChanges(entity, request);
            await _dbContext.Declarations.AddAsync(entity, cancellationToken);
        }

        var task = await _dbContext.DeclarationTasks.FirstOrDefaultAsync(x => x.Id == entity.TaskId, cancellationToken)
            ?? throw new InvalidOperationException("申报任务不存在");

        if (!task.IsEnabled || now < task.StartAt || now > task.EndAt)
        {
            throw new InvalidOperationException($"不在申报时间窗口内：{task.StartAt:yyyy-MM-dd HH:mm:ss} ~ {task.EndAt:yyyy-MM-dd HH:mm:ss}");
        }

        var fromStatus = entity.CurrentStatus;
        entity.CurrentStatus = DeclarationStatus.PendingPreReview;
        entity.CurrentNode = DeclarationNode.PreReview;
        entity.SubmittedAt = now;
        entity.UpdatedAt = now;

        await _dbContext.SaveChangesAsync(cancellationToken);

        if (!string.IsNullOrWhiteSpace(request.TempAttachmentKey))
        {
            await BindTemporaryAttachmentsAsync(request.TempAttachmentKey, applicantUserId, entity.Id, cancellationToken);
        }

        await _dbContext.DeclarationFlowLogs.AddAsync(new DeclarationFlowLog
        {
            DeclarationId = entity.Id,
            FromStatus = fromStatus,
            ToStatus = DeclarationStatus.PendingPreReview,
            ActionType = isExisting && fromStatus != DeclarationStatus.Draft ? FlowActionType.Resubmit : FlowActionType.Submit,
            OperatorUserId = applicantUserId,
            CreatedAt = now
        }, cancellationToken);

        await _dbContext.SaveChangesAsync(cancellationToken);
        return entity.Id;
    }

    public async Task<PagedResultDto<DeclarationListItemDto>> GetMyDeclarationsAsync(long applicantUserId, DeclarationPageQueryDto query, CancellationToken cancellationToken = default)
    {
        ValidateDateRange(query.StartDate, query.EndDate);

        var q = _dbContext.Declarations
            .AsNoTracking()
            .Include(x => x.Task)
            .Include(x => x.Department)
            .Include(x => x.ProjectCategory)
            .Where(x => x.ApplicantUserId == applicantUserId);

        if (query.StartDate.HasValue)
        {
            q = q.Where(x => x.SubmittedAt >= query.StartDate.Value.Date);
        }

        if (query.EndDate.HasValue)
        {
            var endExclusive = query.EndDate.Value.Date.AddDays(1);
            q = q.Where(x => x.SubmittedAt < endExclusive);
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
            ProjectLevel = x.ProjectLevel,
            AwardLevel = x.AwardLevel,
            ParticipationType = x.ParticipationType,
            PrincipalName = x.PrincipalName,
            CurrentStatus = x.CurrentStatus,
            SubmittedAt = x.SubmittedAt,
            TaskName = x.Task?.TaskName ?? string.Empty,
            Action = x.CurrentStatus is DeclarationStatus.PreReviewRejected or DeclarationStatus.InitialReviewRejected or DeclarationStatus.Draft
                ? "修改"
                : "查看"
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

        EnsureCanEdit(declaration);

        return await SaveAttachmentAsync(
            declarationId,
            null,
            uploaderId,
            file,
            $"declarations/{declarationId}",
            cancellationToken);
    }

    public async Task<long> UploadTemporaryAttachmentAsync(string tempAttachmentKey, long uploaderId, IFormFile file, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(tempAttachmentKey))
        {
            throw new InvalidOperationException("临时附件标识不能为空");
        }

        return await SaveAttachmentAsync(
            null,
            tempAttachmentKey.Trim(),
            uploaderId,
            file,
            Path.Combine("declarations", "temp", uploaderId.ToString(), tempAttachmentKey.Trim()),
            cancellationToken);
    }

    public async Task<(byte[] Content, string FileName, string ContentType)> DownloadAttachmentAsync(long attachmentId, long currentUserId, CancellationToken cancellationToken = default)
    {
        var attachment = await _dbContext.DeclarationAttachments
            .AsNoTracking()
            .Include(x => x.Declaration)
            .FirstOrDefaultAsync(x => x.Id == attachmentId && !x.IsDeleted, cancellationToken)
            ?? throw new InvalidOperationException("附件不存在");

        var declaration = attachment.Declaration ?? throw new InvalidOperationException("申报单不存在");
        await EnsureCanAccessDeclarationAsync(declaration, currentUserId, cancellationToken);

        var bytes = await _fileStorageService.ReadAsync(attachment.StoragePath, cancellationToken);
        return (bytes, attachment.OriginalFileName, attachment.ContentType ?? "application/octet-stream");
    }

    public async Task<List<AttachmentDto>> GetAttachmentsAsync(long declarationId, long currentUserId, CancellationToken cancellationToken = default)
    {
        var declaration = await _dbContext.Declarations
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == declarationId, cancellationToken)
            ?? throw new InvalidOperationException("申报单不存在");

        await EnsureCanAccessDeclarationAsync(declaration, currentUserId, cancellationToken);

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
            })
            .ToListAsync(cancellationToken);
    }

    public async Task<List<AttachmentDto>> GetTemporaryAttachmentsAsync(string tempAttachmentKey, long currentUserId, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(tempAttachmentKey))
        {
            return [];
        }

        return await _dbContext.DeclarationAttachments
            .AsNoTracking()
            .Where(x => x.TempAttachmentKey == tempAttachmentKey.Trim() && x.UploadedByUserId == currentUserId && !x.IsDeleted)
            .OrderByDescending(x => x.UploadedAt)
            .Select(x => new AttachmentDto
            {
                Id = x.Id,
                OriginalFileName = x.OriginalFileName,
                FileSizeBytes = x.FileSizeBytes,
                UploadedAt = x.UploadedAt
            })
            .ToListAsync(cancellationToken);
    }

    public async Task ClearTemporaryAttachmentsAsync(string tempAttachmentKey, long currentUserId, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(tempAttachmentKey))
        {
            return;
        }

        var attachments = await _dbContext.DeclarationAttachments
            .Where(x => x.TempAttachmentKey == tempAttachmentKey.Trim() && x.UploadedByUserId == currentUserId && !x.IsDeleted)
            .ToListAsync(cancellationToken);

        foreach (var attachment in attachments)
        {
            attachment.IsDeleted = true;
            attachment.TempAttachmentKey = null;
            await _fileStorageService.DeleteAsync(attachment.StoragePath, cancellationToken);
        }

        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    private async Task<long> SaveAttachmentAsync(
        long? declarationId,
        string? tempAttachmentKey,
        long uploaderId,
        IFormFile file,
        string subFolder,
        CancellationToken cancellationToken)
    {
        var saved = await _fileStorageService.SaveAsync(file, subFolder, cancellationToken);
        var attachment = new DeclarationAttachment
        {
            DeclarationId = declarationId,
            TempAttachmentKey = tempAttachmentKey,
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

    private async Task BindTemporaryAttachmentsAsync(string tempAttachmentKey, long currentUserId, long declarationId, CancellationToken cancellationToken)
    {
        var attachments = await _dbContext.DeclarationAttachments
            .Where(x => x.TempAttachmentKey == tempAttachmentKey.Trim() && x.UploadedByUserId == currentUserId && !x.IsDeleted)
            .ToListAsync(cancellationToken);

        if (attachments.Count == 0)
        {
            return;
        }

        foreach (var attachment in attachments)
        {
            attachment.DeclarationId = declarationId;
            attachment.TempAttachmentKey = null;
        }

        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    private static void ApplyDeclarationChanges(Declaration entity, SaveDeclarationRequestDto request)
    {
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
    }

    private static void ApplyDeclarationChanges(Declaration entity, DeclarationSubmitRequestDto request)
    {
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
    }

    private static void EnsureCanEdit(Declaration entity)
    {
        if (entity.CurrentStatus is not (DeclarationStatus.Draft or DeclarationStatus.PreReviewRejected or DeclarationStatus.InitialReviewRejected))
        {
            throw new InvalidOperationException("当前状态不可修改");
        }
    }

    private static void ValidateDateRange(DateTime? startDate, DateTime? endDate)
    {
        if (startDate.HasValue && endDate.HasValue && startDate.Value.Date > endDate.Value.Date)
        {
            throw new InvalidOperationException("开始日期不能晚于结束日期");
        }
    }

    private static void ValidateSubmitRequest(DeclarationSubmitRequestDto request)
    {
        if (request.TaskId <= 0)
        {
            throw new InvalidOperationException("申报任务不能为空");
        }
    }

    private async Task EnsureCanAccessDeclarationAsync(Declaration declaration, long currentUserId, CancellationToken cancellationToken)
    {
        if (declaration.ApplicantUserId == currentUserId)
        {
            return;
        }

        var isSuperAdmin = await _dbContext.Users.AnyAsync(x => x.Id == currentUserId && x.IsSuperAdmin, cancellationToken);
        if (isSuperAdmin)
        {
            return;
        }

        var hasPreReviewPermission = await _dbContext.UserPreReviewDepartments
            .AnyAsync(x => x.UserId == currentUserId && x.DepartmentId == declaration.DepartmentId, cancellationToken);
        if (hasPreReviewPermission)
        {
            return;
        }

        var hasInitialReviewPermission = await _dbContext.UserInitialReviewCategories
            .AnyAsync(x => x.UserId == currentUserId && x.ProjectCategoryId == declaration.ProjectCategoryId, cancellationToken);
        if (hasInitialReviewPermission)
        {
            return;
        }

        throw new InvalidOperationException("无权访问该申报单");
    }
}
