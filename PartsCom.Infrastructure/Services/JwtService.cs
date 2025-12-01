using ErrorOr;
using MediatR;
using System.Text;
using System.Security.Claims;
using PartsCom.Domain.Errors;
using PartsCom.Domain.Entities;
using System.Security.Cryptography;
using Microsoft.IdentityModel.Tokens;
using PartsCom.Application.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.Extensions.Configuration;

namespace PartsCom.Infrastructure.Services;

internal sealed class JwtService(IConfiguration configuration, IDateTimeProvider dateTimeProvider) : IJwtService
{
    private readonly string _issuer = configuration.GetValue<string>("JwtSettings:Issuer") ?? string.Empty;
    private readonly string _audience = configuration.GetValue<string>("JwtSettings:Audience") ?? string.Empty;
    private readonly string _key = configuration.GetValue<string>("JwtSettings:Key") ?? string.Empty;
    
    public ErrorOr<string> GenerateToken(User user)
    {
        List<Claim> claims = 
        [
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(ClaimTypes.Name, user.FirstName),
            new(ClaimTypes.Surname, user.LastName),
            new(ClaimTypes.Email, user.Email)
        ];
        
        if (string.IsNullOrEmpty(_issuer))
        {
            return Errors.JwtServiceInvalidIssuer;
        }
        
        if (string.IsNullOrEmpty(_audience))
        {
            return Errors.JwtServiceInvalidAudience;
        }
        
        if (string.IsNullOrEmpty(_key))
        {
            return Errors.JwtServiceInvalidSigningKey;
        }
        
        DateTime expires = DateTime.UtcNow.AddSeconds(configuration.GetValue<int>("JwtSettings:ExpiryInSeconds"));
        
        var singingCredentials = new SigningCredentials(
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_key)),
            SecurityAlgorithms.HmacSha256);
        
        var tokenDescriptor = new JwtSecurityToken(
            _issuer,
            _audience,
            claims,
            expires: expires,
            signingCredentials: singingCredentials);
        
        return new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
    }

    public string GenerateRefreshToken()
    {
        Span<byte> randomNumber = stackalloc byte[32];
        
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        
        return Convert.ToBase64String(randomNumber);
    }

    public ErrorOr<Unit> VerifyToken(string token)
    {
        if (string.IsNullOrEmpty(_issuer))
        {
            return Errors.JwtServiceInvalidIssuer;
        }

        if (string.IsNullOrEmpty(_audience))
        {
            return Errors.JwtServiceInvalidAudience;
        }

        if (string.IsNullOrEmpty(_key))
        {
            return Errors.JwtServiceInvalidSigningKey;
        }

        var tokenHandler = new JwtSecurityTokenHandler();
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_key));

        var validationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = _issuer,
            ValidateAudience = true,
            ValidAudience = _audience,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = key,
            ClockSkew = TimeSpan.Zero,
            LifetimeValidator = (_, expires, _, _) => expires != null && expires > dateTimeProvider.UtcNow
        };

        try
        {
            tokenHandler.ValidateToken(token, validationParameters, out _);
            return Unit.Value;
        }
        catch (SecurityTokenExpiredException)
        {
            return Errors.JwtServiceTokenExpired;
        }
        catch (Exception)
        {
            return Errors.JwtServiceTokenInvalid;
        }
    }

    public Guid? GetUserIdFromToken(string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        JwtSecurityToken? jwtToken = tokenHandler.ReadJwtToken(token);

        Claim? userIdClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
        
        if (userIdClaim != null && Guid.TryParse(userIdClaim.Value, out Guid userId))
        {
            return userId;
        }

        return null;
    }
}
