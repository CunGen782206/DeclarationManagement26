namespace DeclarationManagement.Api.Entities;

/// <summary>
/// 项目等级枚举。
/// </summary>
public enum ProjectLevel
{
    National = 1,
    Municipal = 2,
    IndustryOrTeachingGuidanceCommittee = 3,
    School = 4
}

/// <summary>
/// 奖项等级枚举。
/// </summary>
public enum AwardLevel
{
    First = 1,
    Second = 2,
    Third = 3,
    Fourth = 4,
    None = 5
}

/// <summary>
/// 参与形式枚举。
/// </summary>
public enum ParticipationType
{
    Individual = 1,
    Team = 2
}

/// <summary>
/// 当前流程状态枚举。
/// </summary>
public enum DeclarationStatus
{
    Draft = 1,
    PendingPreReview = 2,
    PreReviewRejected = 3,
    PreReviewNotPassed = 4,
    PendingInitialReview = 5,
    InitialReviewRejected = 6,
    InitialReviewNotPassed = 7,
    InitialReviewApproved = 8
}

/// <summary>
/// 流程节点枚举。
/// </summary>
public enum DeclarationNode
{
    Declaration = 1,
    PreReview = 2,
    InitialReview = 3,
    End = 4
}

/// <summary>
/// 审核阶段枚举。
/// </summary>
public enum ReviewStage
{
    PreReview = 1,
    InitialReview = 2
}

/// <summary>
/// 审核动作枚举。
/// </summary>
public enum ReviewAction
{
    Pass = 1,
    NotPass = 2,
    Reject = 3
}

/// <summary>
/// 流程动作类型枚举。
/// </summary>
public enum FlowActionType
{
    Submit = 1,
    PreReviewPass = 2,
    PreReviewNotPass = 3,
    PreReviewReject = 4,
    InitialReviewPass = 5,
    InitialReviewNotPass = 6,
    InitialReviewReject = 7,
    Resubmit = 8
}
