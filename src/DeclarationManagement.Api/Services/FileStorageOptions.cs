namespace DeclarationManagement.Api.Services;

/// <summary>
/// FileStorageOptions 类。
/// </summary>
public class FileStorageOptions
{
    public const string SectionName = "FileStorage";
    /// <summary>
    /// RootPath 属性。
    /// </summary>
    public string RootPath { get; set; } = "storage";
}
