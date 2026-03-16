namespace DeclarationManagement.Api.Entities;

/// <summary>
/// 申报附件实体。
/// </summary>
public class DeclarationAttachment : BaseEntity
{
    /// <summary>
    /// DeclarationId 属性。
    /// </summary>
    public long DeclarationId { get; set; }
    /// <summary>
    /// OriginalFileName 属性。
    /// </summary>
    public string OriginalFileName { get; set; } = string.Empty;
    /// <summary>
    /// StorageFileName 属性。
    /// </summary>
    public string StorageFileName { get; set; } = string.Empty;
    /// <summary>
    /// StoragePath 属性。
    /// </summary>
    public string StoragePath { get; set; } = string.Empty;
    /// <summary>
    /// ContentType 属性。
    /// </summary>
    public string? ContentType { get; set; }
    /// <summary>
    /// FileSizeBytes 属性。
    /// </summary>
    public long FileSizeBytes { get; set; }
    /// <summary>
    /// UploadedByUserId 属性。
    /// </summary>
    public long UploadedByUserId { get; set; }
    /// <summary>
    /// UploadedAt 属性。
    /// </summary>
    public DateTime UploadedAt { get; set; } = DateTime.UtcNow;
    /// <summary>
    /// IsDeleted 属性。
    /// </summary>
    public bool IsDeleted { get; set; }

    /// <summary>
    /// Declaration 属性。
    /// </summary>
    public Declaration? Declaration { get; set; }
    /// <summary>
    /// UploadedByUser 属性。
    /// </summary>
    public User? UploadedByUser { get; set; }
}
