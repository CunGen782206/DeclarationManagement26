namespace DeclarationManagement.Api.Services;

public class JwtOptions
{
    public const string SectionName = "Jwt";
    public string Issuer { get; set; } = "DeclarationManagement";
    public string Audience { get; set; } = "DeclarationManagementClient";
    public string SecretKey { get; set; } = "ReplaceThisWithAtLeast32CharactersSecret!";
    public int ExpireMinutes { get; set; } = 120;
}
