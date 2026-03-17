namespace DeclarationManagement.Api.Entities;

/// <summary>
/// 申报附件实体。
/// </summary>
public class DeclarationAttachment : BaseEntity
{
    /// <summary>
    /// 申报ID属性。
    /// </summary>
    public long? DeclarationId { get; set; }
    /// <summary>
    /// 涓存椂闄勪欢鍏抽敭灞炴€с€?
    /// </summary>
    public string? TempAttachmentKey { get; set; }
    /// <summary>
    /// 原始文件名称属性。
    /// </summary>
    public string OriginalFileName { get; set; } = string.Empty;
    /// <summary>
    /// 存储文件名称属性。
    /// </summary>
    public string StorageFileName { get; set; } = string.Empty;
    /// <summary>
    /// 存储路径属性。
    /// </summary>
    public string StoragePath { get; set; } = string.Empty;
    /// <summary>
    /// 内容类型属性。
    /// </summary>
    public string? ContentType { get; set; }
    /// <summary>
    /// 文件大小字节属性。
    /// </summary>
    public long FileSizeBytes { get; set; }
    /// <summary>
    /// UploadedBy用户ID属性。
    /// </summary>
    public long UploadedByUserId { get; set; }
    /// <summary>
    /// Uploaded时间属性。
    /// </summary>
    public DateTime UploadedAt { get; set; } = DateTime.UtcNow;
    /// <summary>
    /// 是否Deleted属性。
    /// </summary>
    public bool IsDeleted { get; set; }

    /// <summary>
    /// 申报属性。
    /// </summary>
    public Declaration? Declaration { get; set; }
    /// <summary>
    /// UploadedBy用户属性。
    /// </summary>
    public User? UploadedByUser { get; set; }
}
