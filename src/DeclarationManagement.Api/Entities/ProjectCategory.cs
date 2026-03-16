namespace DeclarationManagement.Api.Entities;

/// <summary>
/// 项目类别字典表实体。
/// </summary>
public class ProjectCategory : BaseEntity
{
    /// <summary>
    /// Name 属性。
    /// </summary>
    public string Name { get; set; } = string.Empty;
    /// <summary>
    /// SortOrder 属性。
    /// </summary>
    public int SortOrder { get; set; }
    /// <summary>
    /// IsEnabled 属性。
    /// </summary>
    public bool IsEnabled { get; set; } = true;

    /// <summary>
    /// Declarations 属性。
    /// </summary>
    public ICollection<Declaration> Declarations { get; set; } = new List<Declaration>();
    /// <summary>
    /// UserInitialReviewCategories 属性。
    /// </summary>
    public ICollection<UserInitialReviewCategory> UserInitialReviewCategories { get; set; } = new List<UserInitialReviewCategory>();
}
