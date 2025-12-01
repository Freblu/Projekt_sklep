using ErrorOr;
using MediatR;
using System.Globalization;
using PartsCom.Application.Commands.LoginUser;
using PartsCom.Application.Commands.LogoutUser;
using PartsCom.Application.Commands.RefreshToken;
using PartsCom.Application.Interfaces;
using PartsCom.Application.Queries.ValidateToken;

namespace PartsCom.Ui.Extensions;

internal static class AuthenticationExtensions
{
    private const string AccessTokenKey = "AccessToken";
    private const string RefreshTokenKey = "RefreshToken";
    private const string RefreshTokenExpirationKey = "RefreshTokenExpiration";
    
    public static async Task<ErrorOr<LoginUserCommandResponse>> LoginAsync(
        this HttpContext context,
        string email,
        string password,
        ISender sender)
    {
        var command = new LoginUserCommand(email, password);
        ErrorOr<LoginUserCommandResponse> result = await sender.Send(command);

        if (result.IsError)
        {
            return result;
        }

        LoginUserCommandResponse response = result.Value;
        context.SetAuthenticationTokens(response.Token, response.RefreshToken, response.RefreshTokenExpiry);

        return result;
    }
    
    public static async Task<ErrorOr<Unit>> LogoutAsync(this HttpContext context, ISender sender)
    {
        string? token = context.GetAccessToken();
        
        if (token is not null)
        {
            var command = new LogoutUserCommand(token);
            ErrorOr<Unit> result = await sender.Send(command);
            
            if (!result.IsError)
            {
                context.ClearAuthenticationTokens();
            }

            return result;
        }

        context.ClearAuthenticationTokens();
        return Unit.Value;
    }
    
    public static async Task<ErrorOr<string>> RefreshTokenAsync(this HttpContext context, ISender sender)
    {
        string? refreshToken = context.GetRefreshToken();

        if (refreshToken is null)
        {
            return Error.Unauthorized(description: "Brak refresh token");
        }

        var command = new RefreshTokenCommand(refreshToken);
        ErrorOr<string> result = await sender.Send(command);

        if (result.IsError)
        {
            return result;
        }

        string newAccessToken = result.Value;
        context.SetAccessToken(newAccessToken);

        return result;
    }
    
    public static async Task<ErrorOr<Unit>> ValidateTokenAsync(this HttpContext context, ISender sender)
    {
        string? token = context.GetAccessToken();

        if (token is null)
        {
            return Error.Unauthorized(description: "Brak tokenu dostępu");
        }

        var query = new ValidateTokenQuery(token);
        return await sender.Send(query);
    }
    
    public static async Task<bool> IsAuthenticatedAsync(this HttpContext context, ISender sender)
    {
        ErrorOr<Unit> result = await context.ValidateTokenAsync(sender);
        return !result.IsError;
    }

    private static void SetAuthenticationTokens(
        this HttpContext context,
        string accessToken,
        string refreshToken,
        DateTime refreshTokenExpiry)
    {
        var cookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Strict,
            IsEssential = true
        };

        // Access token - short lived, expires in 15 minutes typically
        var accessTokenOptions = new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Strict,
            IsEssential = true,
            Expires = DateTimeOffset.UtcNow.AddMinutes(15)
        };

        // Refresh token - longer lived, uses the expiry from backend
        var refreshTokenOptions = new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Strict,
            IsEssential = true,
            Expires = refreshTokenExpiry
        };

        context.Response.Cookies.Append(AccessTokenKey, accessToken, accessTokenOptions);
        context.Response.Cookies.Append(RefreshTokenKey, refreshToken, refreshTokenOptions);
        context.Response.Cookies.Append(RefreshTokenExpirationKey, refreshTokenExpiry.ToString("o"), cookieOptions);
    }

    private static void ClearAuthenticationTokens(this HttpContext context)
    {
        context.Response.Cookies.Delete(AccessTokenKey);
        context.Response.Cookies.Delete(RefreshTokenKey);
        context.Response.Cookies.Delete(RefreshTokenExpirationKey);
    }

    private static string? GetAccessToken(this HttpContext context)
    {
        return context.Request.Cookies[AccessTokenKey];
    }

    private static string? GetRefreshToken(this HttpContext context)
    {
        return context.Request.Cookies[RefreshTokenKey];
    }

    private static void SetAccessToken(this HttpContext context, string accessToken)
    {
        var accessTokenOptions = new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Lax,
            IsEssential = true,
            Expires = DateTimeOffset.UtcNow.AddMinutes(15)
        };

        context.Response.Cookies.Append(AccessTokenKey, accessToken, accessTokenOptions);
    }

    private static DateTime? GetRefreshTokenExpiration(this HttpContext context)
    {
        string? expirationString = context.Request.Cookies[RefreshTokenExpirationKey];

        if (expirationString is not null && DateTime.TryParse(expirationString, CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind, out DateTime expiration))
        {
            return expiration;
        }

        return null;
    }
    
    public static bool IsRefreshTokenExpired(this HttpContext context)
    {
        DateTime? expiration = context.GetRefreshTokenExpiration();
        return expiration.HasValue && expiration.Value < DateTime.UtcNow;
    }

    public static Guid? GetUserId(this HttpContext context)
    {
        string? token = context.GetAccessToken();

        if (token is null)
        {
            return null;
        }

        IJwtService jwtService = context.RequestServices.GetRequiredService<PartsCom.Application.Interfaces.IJwtService>();
        return jwtService.GetUserIdFromToken(token);
    }
}

