namespace DeclarationManagement.Api.Entities;

/// <summary>
/// 流程日志实体（状态每次变化都记录）。
/// </summary>
public class DeclarationFlowLog : BaseEntity
{
    /// <summary>
    /// 申报ID属性。
    /// </summary>
    public long DeclarationId { get; set; }
    /// <summary>
    /// 来源状态属性。
    /// </summary>
    public DeclarationStatus? FromStatus { get; set; }
    /// <summary>
    /// 目标状态属性。
    /// </summary>
    public DeclarationStatus ToStatus { get; set; }
    /// <summary>
    /// 动作类型属性。
    /// </summary>
    public FlowActionType ActionType { get; set; }
    /// <summary>
    /// 操作人用户ID属性。
    /// </summary>
    public long OperatorUserId { get; set; }
    /// <summary>
    /// Note属性。
    /// </summary>
    public string? Note { get; set; }

    /// <summary>
    /// 申报属性。
    /// </summary>
    public Declaration? Declaration { get; set; }
    /// <summary>
    /// 操作人用户属性。
    /// </summary>
    public User? OperatorUser { get; set; }
}
