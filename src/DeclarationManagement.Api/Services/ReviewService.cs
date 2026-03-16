using DeclarationManagement.Api.DTOs;
using DeclarationManagement.Api.Entities;
using DeclarationManagement.Api.Repositories;

namespace DeclarationManagement.Api.Services;

/// <summary>
/// 审核服务实现：后续在此补充权限命中、状态流转、审核记录/流程日志双写。
/// </summary>
public class ReviewService : IReviewService
{
    private readonly IRepository<Declaration> _declarationRepository;
    private readonly IRepository<DeclarationReviewRecord> _reviewRecordRepository;
    private readonly IRepository<DeclarationFlowLog> _flowLogRepository;

    public ReviewService(
        IRepository<Declaration> declarationRepository,
        IRepository<DeclarationReviewRecord> reviewRecordRepository,
        IRepository<DeclarationFlowLog> flowLogRepository)
    {
        _declarationRepository = declarationRepository;
        _reviewRecordRepository = reviewRecordRepository;
        _flowLogRepository = flowLogRepository;
    }

    public async Task<PagedResultDto<PendingReviewItemDto>> GetPendingAsync(long reviewerUserId, PendingReviewQueryDto query, CancellationToken cancellationToken = default)
    {
        // TODO: 根据预审/初审权限表筛选当前审核人可处理的待办。
        var declarations = await _declarationRepository.GetListAsync(cancellationToken: cancellationToken);
        var pending = declarations.Where(x => x.CurrentStatus is DeclarationStatus.PendingPreReview or DeclarationStatus.PendingInitialReview).ToList();

        var items = pending
            .Skip((query.PageIndex - 1) * query.PageSize)
            .Take(query.PageSize)
            .Select(x => new PendingReviewItemDto
            {
                DeclarationId = x.Id,
                ProjectName = x.ProjectName,
                CurrentReviewStage = x.CurrentStatus == DeclarationStatus.PendingPreReview ? ReviewStage.PreReview : ReviewStage.InitialReview,
                SubmittedAt = x.SubmittedAt
            }).ToList();

        return new PagedResultDto<PendingReviewItemDto>
        {
            PageIndex = query.PageIndex,
            PageSize = query.PageSize,
            TotalCount = pending.Count,
            Items = items
        };
    }

    public async Task ExecuteReviewAsync(long reviewerUserId, ReviewActionRequestDto request, CancellationToken cancellationToken = default)
    {
        var declaration = await _declarationRepository.GetByIdAsync(request.DeclarationId, cancellationToken)
                          ?? throw new InvalidOperationException("申报单不存在");

        // 1) 写审核记录（累加，不能覆盖）
        var reviewRecord = new DeclarationReviewRecord
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
        };
        await _reviewRecordRepository.AddAsync(reviewRecord, cancellationToken);

        // 2) 更新状态（骨架规则）
        var fromStatus = declaration.CurrentStatus;
        declaration.CurrentStatus = MapToStatus(request.ReviewStage, request.ReviewAction);
        declaration.CurrentNode = declaration.CurrentStatus == DeclarationStatus.PendingInitialReview
            ? DeclarationNode.InitialReview
            : declaration.CurrentStatus == DeclarationStatus.PendingPreReview
                ? DeclarationNode.PreReview
                : DeclarationNode.End;

        _declarationRepository.Update(declaration);

        // 3) 写流程日志（与状态变更同时写入）
        var log = new DeclarationFlowLog
        {
            DeclarationId = declaration.Id,
            FromStatus = fromStatus,
            ToStatus = declaration.CurrentStatus,
            ActionType = MapToFlowAction(request.ReviewStage, request.ReviewAction),
            OperatorUserId = reviewerUserId,
            Note = request.Reason,
            CreatedAt = DateTime.UtcNow
        };
        await _flowLogRepository.AddAsync(log, cancellationToken);

        // 保存
        await _declarationRepository.SaveChangesAsync(cancellationToken);
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
