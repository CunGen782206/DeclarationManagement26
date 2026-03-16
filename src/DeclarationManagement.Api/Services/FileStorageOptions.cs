namespace DeclarationManagement.Api.Services;

/// <summary>
/// 文件存储配置类。
/// </summary>
public class FileStorageOptions
{
    public const string SectionName = "FileStorage";
    /// <summary>
    /// Root路径属性。
    /// </summary>
    public string RootPath { get; set; } = "storage";
}
