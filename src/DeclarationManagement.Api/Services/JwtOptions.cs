namespace DeclarationManagement.Api.Services;

/// <summary>
/// JwtOptions 类。
/// </summary>
public class JwtOptions
{
    public const string SectionName = "Jwt";
    /// <summary>
    /// Issuer 属性。
    /// </summary>
    public string Issuer { get; set; } = "DeclarationManagement";
    /// <summary>
    /// Audience 属性。
    /// </summary>
    public string Audience { get; set; } = "DeclarationManagementClient";
    /// <summary>
    /// SecretKey 属性。
    /// </summary>
    public string SecretKey { get; set; } = "ReplaceThisWithAtLeast32CharactersSecret!";
    /// <summary>
    /// ExpireMinutes 属性。
    /// </summary>
    public int ExpireMinutes { get; set; } = 120;
}
