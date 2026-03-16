namespace DeclarationManagement.Api.Entities;

/// <summary>
/// 项目类别字典表实体。
/// </summary>
public class ProjectCategory : BaseEntity
{
    /// <summary>
    /// 名称属性。
    /// </summary>
    public string Name { get; set; } = string.Empty;
    /// <summary>
    /// SortOrder属性。
    /// </summary>
    public int SortOrder { get; set; }
    /// <summary>
    /// 是否启用属性。
    /// </summary>
    public bool IsEnabled { get; set; } = true;

    /// <summary>
    /// Declarations属性。
    /// </summary>
    public ICollection<Declaration> Declarations { get; set; } = new List<Declaration>();
    /// <summary>
    /// 用户初审核Categories属性。
    /// </summary>
    public ICollection<UserInitialReviewCategory> UserInitialReviewCategories { get; set; } = new List<UserInitialReviewCategory>();
}
