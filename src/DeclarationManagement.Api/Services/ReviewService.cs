using DeclarationManagement.Api.Data;
using DeclarationManagement.Api.DTOs;
using DeclarationManagement.Api.Entities;
using Microsoft.EntityFrameworkCore;

namespace DeclarationManagement.Api.Services;

public class ReviewService : IReviewService
{
    private readonly AppDbContext _dbContext;

    public ReviewService(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<PagedResultDto<PendingReviewItemDto>> GetPendingAsync(long reviewerUserId, PendingReviewQueryDto query, CancellationToken cancellationToken = default)
    {
        var preDeptIds = await _dbContext.UserPreReviewDepartments
            .Where(x => x.UserId == reviewerUserId)
            .Select(x => x.DepartmentId)
            .ToListAsync(cancellationToken);

        var initCatIds = await _dbContext.UserInitialReviewCategories
            .Where(x => x.UserId == reviewerUserId)
            .Select(x => x.ProjectCategoryId)
            .ToListAsync(cancellationToken);

        var q = _dbContext.Declarations
            .AsNoTracking()
            .Include(x => x.Department)
            .Where(x =>
                (x.CurrentStatus == DeclarationStatus.PendingPreReview && preDeptIds.Contains(x.DepartmentId)) ||
                (x.CurrentStatus == DeclarationStatus.PendingInitialReview && initCatIds.Contains(x.ProjectCategoryId)));

        if (query.StartDate.HasValue)
        {
            q = q.Where(x => x.SubmittedAt >= query.StartDate.Value);
        }

        if (query.EndDate.HasValue)
        {
            q = q.Where(x => x.SubmittedAt <= query.EndDate.Value);
        }

        var total = await q.LongCountAsync(cancellationToken);
        var list = await q.OrderByDescending(x => x.SubmittedAt)
            .Skip((query.PageIndex - 1) * query.PageSize)
            .Take(query.PageSize)
            .ToListAsync(cancellationToken);

        return new PagedResultDto<PendingReviewItemDto>
        {
            PageIndex = query.PageIndex,
            PageSize = query.PageSize,
            TotalCount = total,
            Items = list.Select(x => new PendingReviewItemDto
            {
                DeclarationId = x.Id,
                ProjectName = x.ProjectName,
                DepartmentName = x.Department?.Name ?? string.Empty,
                CurrentReviewStage = x.CurrentStatus == DeclarationStatus.PendingPreReview ? ReviewStage.PreReview : ReviewStage.InitialReview,
                SubmittedAt = x.SubmittedAt
            }).ToList()
        };
    }

    public async Task ExecuteReviewAsync(long reviewerUserId, ReviewActionRequestDto request, CancellationToken cancellationToken = default)
    {
        var declaration = await _dbContext.Declarations.FirstOrDefaultAsync(x => x.Id == request.DeclarationId, cancellationToken)
                          ?? throw new InvalidOperationException("申报单不存在");

        if (!IsStageMatched(declaration.CurrentStatus, request.ReviewStage))
        {
            throw new InvalidOperationException("审核阶段与表单当前状态不匹配");
        }

        if (request.ReviewStage == ReviewStage.PreReview)
        {
            var can = await _dbContext.UserPreReviewDepartments.AnyAsync(x => x.UserId == reviewerUserId && x.DepartmentId == declaration.DepartmentId, cancellationToken);
            if (!can) throw new InvalidOperationException("无部门预审权限");
        }
        else
        {
            var can = await _dbContext.UserInitialReviewCategories.AnyAsync(x => x.UserId == reviewerUserId && x.ProjectCategoryId == declaration.ProjectCategoryId, cancellationToken);
            if (!can) throw new InvalidOperationException("无初审权限");
        }

        var fromStatus = declaration.CurrentStatus;
        declaration.CurrentStatus = MapToStatus(request.ReviewStage, request.ReviewAction);
        declaration.CurrentNode = declaration.CurrentStatus switch
        {
            DeclarationStatus.PendingPreReview => DeclarationNode.PreReview,
            DeclarationStatus.PendingInitialReview => DeclarationNode.InitialReview,
            _ => DeclarationNode.End
        };
        declaration.UpdatedAt = DateTime.UtcNow;

        await _dbContext.DeclarationReviewRecords.AddAsync(new DeclarationReviewRecord
        {
            DeclarationId = declaration.Id,
            ReviewStage = request.ReviewStage,
            ReviewAction = request.ReviewAction,
            Reason = request.Reason,
            RecognizedProjectLevel = request.RecognizedProjectLevel,
            RecognizedAwardLevel = request.RecognizedAwardLevel,
            RecognizedAmount = request.RecognizedAmount,
            Remark = request.Remark,
            ReviewedByUserId = reviewerUserId,
            ReviewedAt = DateTime.UtcNow,
            CreatedAt = DateTime.UtcNow
        }, cancellationToken);

        await _dbContext.DeclarationFlowLogs.AddAsync(new DeclarationFlowLog
        {
            DeclarationId = declaration.Id,
            FromStatus = fromStatus,
            ToStatus = declaration.CurrentStatus,
            ActionType = MapToFlowAction(request.ReviewStage, request.ReviewAction),
            OperatorUserId = reviewerUserId,
            Note = request.Reason,
            CreatedAt = DateTime.UtcNow
        }, cancellationToken);

        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<List<ReviewRecordDto>> GetReviewRecordsAsync(long declarationId, long currentUserId, CancellationToken cancellationToken = default)
    {
        return await _dbContext.DeclarationReviewRecords
            .AsNoTracking()
            .Where(x => x.DeclarationId == declarationId)
            .OrderBy(x => x.ReviewedAt)
            .Select(x => new ReviewRecordDto
            {
                Id = x.Id,
                DeclarationId = x.DeclarationId,
                ReviewStage = x.ReviewStage,
                ReviewAction = x.ReviewAction,
                Reason = x.Reason,
                RecognizedProjectLevel = x.RecognizedProjectLevel,
                RecognizedAwardLevel = x.RecognizedAwardLevel,
                RecognizedAmount = x.RecognizedAmount,
                Remark = x.Remark,
                ReviewedByUserId = x.ReviewedByUserId,
                ReviewedAt = x.ReviewedAt
            }).ToListAsync(cancellationToken);
    }

    private static bool IsStageMatched(DeclarationStatus status, ReviewStage stage)
    {
        return (status, stage) switch
        {
            (DeclarationStatus.PendingPreReview, ReviewStage.PreReview) => true,
            (DeclarationStatus.PendingInitialReview, ReviewStage.InitialReview) => true,
            _ => false
        };
    }

    private static DeclarationStatus MapToStatus(ReviewStage stage, ReviewAction action)
    {
        return (stage, action) switch
        {
            (ReviewStage.PreReview, ReviewAction.Pass) => DeclarationStatus.PendingInitialReview,
            (ReviewStage.PreReview, ReviewAction.NotPass) => DeclarationStatus.PreReviewNotPassed,
            (ReviewStage.PreReview, ReviewAction.Reject) => DeclarationStatus.PreReviewRejected,
            (ReviewStage.InitialReview, ReviewAction.Pass) => DeclarationStatus.InitialReviewApproved,
            (ReviewStage.InitialReview, ReviewAction.NotPass) => DeclarationStatus.InitialReviewNotPassed,
            (ReviewStage.InitialReview, ReviewAction.Reject) => DeclarationStatus.InitialReviewRejected,
            _ => throw new InvalidOperationException("不支持的审核动作")
        };
    }

    private static FlowActionType MapToFlowAction(ReviewStage stage, ReviewAction action)
    {
        return (stage, action) switch
        {
            (ReviewStage.PreReview, ReviewAction.Pass) => FlowActionType.PreReviewPass,
            (ReviewStage.PreReview, ReviewAction.NotPass) => FlowActionType.PreReviewNotPass,
            (ReviewStage.PreReview, ReviewAction.Reject) => FlowActionType.PreReviewReject,
            (ReviewStage.InitialReview, ReviewAction.Pass) => FlowActionType.InitialReviewPass,
            (ReviewStage.InitialReview, ReviewAction.NotPass) => FlowActionType.InitialReviewNotPass,
            (ReviewStage.InitialReview, ReviewAction.Reject) => FlowActionType.InitialReviewReject,
            _ => throw new InvalidOperationException("不支持的审核动作")
        };
    }
}
