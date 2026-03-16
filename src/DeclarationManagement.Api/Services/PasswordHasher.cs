using System.Security.Cryptography;
using System.Text;

namespace DeclarationManagement.Api.Services;

public static class PasswordHasher
{
    public static (string Hash, string Salt) Hash(string plainText)
    {
        var saltBytes = RandomNumberGenerator.GetBytes(16);
        var salt = Convert.ToBase64String(saltBytes);
        var hash = Compute(plainText, salt);
        return (hash, salt);
    }

    public static bool Verify(string plainText, string hash, string? salt)
    {
        if (string.IsNullOrWhiteSpace(salt))
        {
            return false;
        }

        return Compute(plainText, salt) == hash;
    }

    private static string Compute(string plainText, string salt)
    {
        var input = $"{plainText}:{salt}";
        var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(input));
        return Convert.ToBase64String(bytes);
    }
}
