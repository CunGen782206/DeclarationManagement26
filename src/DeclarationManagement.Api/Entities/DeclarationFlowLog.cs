namespace DeclarationManagement.Api.Entities;

/// <summary>
/// 流程日志实体（状态每次变化都记录）。
/// </summary>
public class DeclarationFlowLog : BaseEntity
{
    public long DeclarationId { get; set; }
    public DeclarationStatus? FromStatus { get; set; }
    public DeclarationStatus ToStatus { get; set; }
    public FlowActionType ActionType { get; set; }
    public long OperatorUserId { get; set; }
    public string? Note { get; set; }

    public Declaration? Declaration { get; set; }
    public User? OperatorUser { get; set; }
}
