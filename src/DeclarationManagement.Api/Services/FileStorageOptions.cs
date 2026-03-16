namespace DeclarationManagement.Api.Services;

public class FileStorageOptions
{
    public const string SectionName = "FileStorage";
    public string RootPath { get; set; } = "storage";
}
