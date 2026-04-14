namespace PartsCom.Application.Commands.LoginUser;

public record LoginUserCommandResponse(string Token, string RefreshToken, DateTime RefreshTokenExpiry);
