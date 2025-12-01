using ErrorOr;
using System.Text;
using PartsCom.Domain.Errors;
using System.Security.Cryptography;
using PartsCom.Application.Interfaces;
using Microsoft.Extensions.Configuration;

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
        
        using var pbkdf2 = new Rfc2898DeriveBytes(passwordBytes, combinedSaltBytes.ToArray(), _iterations,
            HashAlgorithmName.SHA256);
        
        return $"{_iterations}:{Convert.ToBase64String(uniqueSaltBytes)}:{Convert.ToBase64String(pbkdf2.GetBytes(32))}";
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
            
            using var pbkdf2 =
                new Rfc2898DeriveBytes(Encoding.UTF8.GetBytes(password), combinedSaltBytes.ToArray(), iterations, HashAlgorithmName.SHA256);
            
            return CryptographicOperations.FixedTimeEquals(pbkdf2.GetBytes(32), Convert.FromBase64String(parts[2]));
        }
        catch (Exception)
        {
            return false;
        }
    }
}
