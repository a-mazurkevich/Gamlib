using System.Security.Cryptography;
using System.Text;

namespace Gamlib.Helpers;

public static class CryptoHelper
{
    public static string GenerateNonce(int length)
    {
        var data = new byte[length];
        var cryptoServiceProvider = RandomNumberGenerator.Create();
        cryptoServiceProvider.GetNonZeroBytes(data);

        return Convert.ToBase64String(data).Substring(0, length);
    }

    public static string GetHash<HAType>(string input) where HAType : HashAlgorithm
    {
        var inputBytes = Encoding.UTF8.GetBytes(input);

        var hashAlgorithm = typeof(HAType)?.GetMethod(nameof(HashAlgorithm.Create), new Type[0])?
                                          .Invoke(null, null) as HAType;
        var hash = hashAlgorithm?.ComputeHash(inputBytes);

        return ConvertBytesToString(hash ?? Array.Empty<byte>());
    }

    private static string ConvertBytesToString(byte[] bytes)
    {
        var stringBuilder = new StringBuilder();

        foreach (var b in bytes)
        {
            stringBuilder.Append(b.ToString("X2"));
        }

        return stringBuilder.ToString();
    }
}

