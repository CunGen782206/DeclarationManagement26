namespace DeclarationManagement.Api.Services;

public class DatabaseInitializationOptions
{
    public const string SectionName = "DatabaseInitialization";

    public bool AutoMigrate { get; set; } = true;

    public bool SeedOnStartup { get; set; } = true;

    public string UsersExcelPath { get; set; } = string.Empty;

    public string DefaultPassword { get; set; } = "111111";

    public string SuperAdminJobNumber { get; set; } = "2070";
}
