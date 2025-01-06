using System.Security.Cryptography;
using System.Text;

namespace Dvchevskii.Blog.Shared.Authentication.Passwords;

public sealed class PasswordHasher
{
    public byte[] Hash(string plainTextPassword)
    {
        Span<byte> salt = stackalloc byte[4];
        Random.Shared.NextBytes(salt);

        return Hash(plainTextPassword, salt);
    }

    private byte[] Hash(string plainTextPassword, ReadOnlySpan<byte> salt)
    {
        var bytes = Encoding.UTF8.GetBytes(plainTextPassword);
        Span<byte> bytesAndSalt = stackalloc byte[bytes.Length + salt.Length];
        Span<byte> hash = stackalloc byte[32];
        SHA256.HashData(bytesAndSalt, hash);

        var result = new byte[hash.Length + salt.Length];
        salt.CopyTo(result);
        hash.CopyTo(result.AsSpan().Slice(salt.Length));

        return result;
    }

    public bool Verify(string plainTextPassword, byte[] hashedPassword)
    {
        var salt = hashedPassword.AsSpan().Slice(0, 4);
        return Hash(plainTextPassword, salt).SequenceEqual(hashedPassword);
    }
}
