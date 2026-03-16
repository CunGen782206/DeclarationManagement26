namespace DeclarationManagement.Api.Entities;

/// <summary>
/// 流程日志实体（状态每次变化都记录）。
/// </summary>
public class DeclarationFlowLog : BaseEntity
{
    /// <summary>
    /// DeclarationId 属性。
    /// </summary>
    public long DeclarationId { get; set; }
    /// <summary>
    /// FromStatus 属性。
    /// </summary>
    public DeclarationStatus? FromStatus { get; set; }
    /// <summary>
    /// ToStatus 属性。
    /// </summary>
    public DeclarationStatus ToStatus { get; set; }
    /// <summary>
    /// ActionType 属性。
    /// </summary>
    public FlowActionType ActionType { get; set; }
    /// <summary>
    /// OperatorUserId 属性。
    /// </summary>
    public long OperatorUserId { get; set; }
    /// <summary>
    /// Note 属性。
    /// </summary>
    public string? Note { get; set; }

    /// <summary>
    /// Declaration 属性。
    /// </summary>
    public Declaration? Declaration { get; set; }
    /// <summary>
    /// OperatorUser 属性。
    /// </summary>
    public User? OperatorUser { get; set; }
}
