using System.Security.Cryptography;
using System.Text;

namespace DeclarationManagement.Api.Services;

public static class PasswordHasher
{
    /// <summary>
    /// Hash 方法。
    /// </summary>
    public static (string Hash, string Salt) Hash(string plainText)
    {
        var saltBytes = RandomNumberGenerator.GetBytes(16); // saltBytes：盐值字节
        var salt = Convert.ToBase64String(saltBytes); // salt：盐值
        var hash = Compute(plainText, salt); // hash：哈希值
        return (hash, salt);
    }

    /// <summary>
    /// Verify 方法。
    /// </summary>
    public static bool Verify(string plainText, string hash, string? salt)
    {
        if (string.IsNullOrWhiteSpace(salt))
        {
            return false;
        }

        return Compute(plainText, salt) == hash;
    }

    /// <summary>
    /// 计算处理。
    /// </summary>
    private static string Compute(string plainText, string salt)
    {
        var input = $"{plainText}:{salt}"; // input：input
        var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(input)); // bytes：字节
        return Convert.ToBase64String(bytes);
    }
}
