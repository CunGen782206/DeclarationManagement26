namespace DeclarationManagement.Api.Entities;

/// <summary>
/// 项目类别字典表实体。
/// </summary>
public class ProjectCategory : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public int SortOrder { get; set; }
    public bool IsEnabled { get; set; } = true;

    public ICollection<Declaration> Declarations { get; set; } = new List<Declaration>();
    public ICollection<UserInitialReviewCategory> UserInitialReviewCategories { get; set; } = new List<UserInitialReviewCategory>();
}
