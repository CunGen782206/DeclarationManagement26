using DeclarationManagement.Api.Data;
using DeclarationManagement.Api.DTOs;
using DeclarationManagement.Api.Entities;
using Microsoft.EntityFrameworkCore;

namespace DeclarationManagement.Api.Services;

/// <summary>
/// ReviewService 类。
/// </summary>
public class ReviewService : IReviewService
{
    /// <summary>
    /// 数据库上下文，用于审核相关查询与状态变更。
    /// </summary>
    private readonly AppDbContext _dbContext;

    /// <summary>
    /// 构造函数。
    /// </summary>
    public ReviewService(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    /// <summary>
    /// 查询当前审核人待办列表（预审/初审）。
    /// </summary>
    public async Task<PagedResultDto<PendingReviewItemDto>> GetPendingAsync(long reviewerUserId, PendingReviewQueryDto query, CancellationToken cancellationToken = default)
    {
        // preDeptIds：当前用户拥有的部门预审权限集合
        var preDeptIds = await _dbContext.UserPreReviewDepartments
            .Where(x => x.UserId == reviewerUserId)
            .Select(x => x.DepartmentId)
            .ToListAsync(cancellationToken);

        // initCatIds：当前用户拥有的初审类别权限集合
        var initCatIds = await _dbContext.UserInitialReviewCategories
            .Where(x => x.UserId == reviewerUserId)
            .Select(x => x.ProjectCategoryId)
            .ToListAsync(cancellationToken);

        // q：待审申报查询（根据状态+权限同时过滤）
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

        // total：待审总数
        var total = await q.LongCountAsync(cancellationToken);
        // list：当前页待审数据
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

    /// <summary>
    /// 执行审核动作，并同步写入审核记录与流程日志。
    /// </summary>
    public async Task ExecuteReviewAsync(long reviewerUserId, ReviewActionRequestDto request, CancellationToken cancellationToken = default)
    {
        // declaration：当前待审核申报单
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

        // fromStatus：记录审核前状态，便于流程追踪
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

    /// <summary>
    /// 获取某申报单的审核记录（累加历史）。
    /// </summary>
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

    /// <summary>
    /// 校验“审核阶段”与“申报当前状态”是否匹配。
    /// </summary>
    private static bool IsStageMatched(DeclarationStatus status, ReviewStage stage)
    {
        return (status, stage) switch
        {
            (DeclarationStatus.PendingPreReview, ReviewStage.PreReview) => true,
            (DeclarationStatus.PendingInitialReview, ReviewStage.InitialReview) => true,
            _ => false
        };
    }

    /// <summary>
    /// 将审核动作映射为目标申报状态。
    /// </summary>
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

    /// <summary>
    /// 将审核动作映射为流程日志动作类型。
    /// </summary>
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
