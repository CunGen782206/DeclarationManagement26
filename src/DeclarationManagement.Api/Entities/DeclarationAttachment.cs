namespace DeclarationManagement.Api.Entities;

/// <summary>
/// 申报附件实体。
/// </summary>
public class DeclarationAttachment : BaseEntity
{
    public long DeclarationId { get; set; }
    public string OriginalFileName { get; set; } = string.Empty;
    public string StorageFileName { get; set; } = string.Empty;
    public string StoragePath { get; set; } = string.Empty;
    public string? ContentType { get; set; }
    public long FileSizeBytes { get; set; }
    public long UploadedByUserId { get; set; }
    public DateTime UploadedAt { get; set; } = DateTime.UtcNow;
    public bool IsDeleted { get; set; }

    public Declaration? Declaration { get; set; }
    public User? UploadedByUser { get; set; }
}
