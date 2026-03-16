namespace DeclarationManagement.Api.Services;

/// <summary>
/// Jwt配置类。
/// </summary>
public class JwtOptions
{
    public const string SectionName = "Jwt";
    /// <summary>
    /// 签发者属性。
    /// </summary>
    public string Issuer { get; set; } = "DeclarationManagement";
    /// <summary>
    /// 受众属性。
    /// </summary>
    public string Audience { get; set; } = "DeclarationManagementClient";
    /// <summary>
    /// 密钥Key属性。
    /// </summary>
    public string SecretKey { get; set; } = "ReplaceThisWithAtLeast32CharactersSecret!";
    /// <summary>
    /// 过期分钟属性。
    /// </summary>
    public int ExpireMinutes { get; set; } = 120;
}
