using System.Security.Cryptography;
using System.Text;
using ErrorOr;
using Microsoft.Extensions.Configuration;
using PartsCom.Application.Interfaces;
using PartsCom.Domain.Errors;

namespace PartsCom.Infrastructure.Services;

internal sealed class PasswordHashService(IConfiguration configuration) : IPasswordHashService
{
    private readonly string _salt = configuration.GetValue<string>("HashSalt") ?? throw new InvalidOperationException("Password hashing salt is not configured.");
    private readonly int _iterations = configuration.GetValue<int>("HashIterations");

    public ErrorOr<string> HashPassword(string password)
    {
        if (string.IsNullOrEmpty(password))
        {
            return Errors.PasswordHashServicePasswordCannotBeNullOrEmpty;
        }

        Span<byte> uniqueSaltBytes = stackalloc byte[16];

        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(uniqueSaltBytes);

        Span<byte> baseSaltBytes = Encoding.UTF8.GetBytes(_salt);
        Span<byte> combinedSaltBytes = stackalloc byte[baseSaltBytes.Length + uniqueSaltBytes.Length];

        baseSaltBytes.CopyTo(combinedSaltBytes);
        uniqueSaltBytes.CopyTo(combinedSaltBytes[baseSaltBytes.Length..]);

        byte[] passwordBytes = Encoding.UTF8.GetBytes(password);
        byte[] hashBytes = Rfc2898DeriveBytes.Pbkdf2(passwordBytes, combinedSaltBytes, _iterations, HashAlgorithmName.SHA256, 32);

        return $"{_iterations}:{Convert.ToBase64String(uniqueSaltBytes)}:{Convert.ToBase64String(hashBytes)}";
    }

    public bool VerifyPassword(string password, string hashedPassword)
    {
        if (string.IsNullOrEmpty(password) || string.IsNullOrEmpty(hashedPassword))
        {
            return false;
        }

        Span<string> parts = hashedPassword.Split(':');

        if (parts.Length != 3)
        {
            return false;
        }

        if (!int.TryParse(parts[0], out int iterations))
        {
            return false;
        }

        try
        {
            Span<byte> uniqueSaltBytes = Convert.FromBase64String(parts[1]);
            Span<byte> baseSaltBytes = Encoding.UTF8.GetBytes(_salt);
            Span<byte> combinedSaltBytes = stackalloc byte[baseSaltBytes.Length + uniqueSaltBytes.Length];

            baseSaltBytes.CopyTo(combinedSaltBytes);
            uniqueSaltBytes.CopyTo(combinedSaltBytes[baseSaltBytes.Length..]);

            byte[] hashBytes = Rfc2898DeriveBytes.Pbkdf2(Encoding.UTF8.GetBytes(password), combinedSaltBytes, iterations, HashAlgorithmName.SHA256, 32);

            return CryptographicOperations.FixedTimeEquals(hashBytes, Convert.FromBase64String(parts[2]));
        }
        catch (Exception)
        {
            return false;
        }
    }
}
