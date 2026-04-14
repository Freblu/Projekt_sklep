using ErrorOr;

namespace PartsCom.Domain.Errors;

public static partial class Errors
{
    public static Error PasswordHashServicePasswordCannotBeNullOrEmpty =>
        Error.Custom(0, "PHS001", "Password cannot be null or empty.");

    public static Error JwtServiceInvalidIssuer =>
        Error.Custom(0, "JWS001", "JWT issuer is invalid.");

    public static Error JwtServiceInvalidAudience =>
        Error.Custom(0, "JWS002", "JWT audience is invalid.");

    public static Error JwtServiceInvalidSigningKey =>
        Error.Custom(0, "JWS003", "JWT signing key is invalid.");

    public static Error JwtServiceTokenExpired =>
        Error.Custom(0, "JWS004", "The JWT token has expired.");

    public static Error JwtServiceTokenInvalid =>
        Error.Custom(0, "JWS005", "The JWT token is invalid.");

    public static Error RegisterUserCommandHandlerEmailAlreadyExists =>
        Error.Custom(0, "RUCH001", "A user with the given email already exists.");

    public static Error LoginUserCommandHandlerInvalidCredentials =>
        Error.Custom(0, "LUCH001", "The provided credentials are invalid.");

    public static Error RefreshTokenCommandHandlerInvalidRefreshToken =>
        Error.Custom(0, "RTCH001", "The provided refresh token is invalid.");

    public static Error LogoutUserCommandHandlerUserNotFound =>
        Error.Custom(0, "LUCH002", "User not found.");

    public static Error AddProductCategoryCommandHandlerCategoryAlreadyExists =>
        Error.Custom(0, "APCCH001", "A product category with the given name already exists.");
}
