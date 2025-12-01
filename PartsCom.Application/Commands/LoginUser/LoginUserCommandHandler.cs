using ErrorOr;
using PartsCom.Domain.Errors;
using PartsCom.Domain.Entities;
using PartsCom.Application.Interfaces;
using Microsoft.Extensions.Configuration;

namespace PartsCom.Application.Commands.LoginUser;

internal sealed class LoginUserCommandHandler(
    IUserRepository userRepository,
    IPasswordHashService passwordHashService,
    IJwtService jwtService,
    IConfiguration configuration,
    IDateTimeProvider dateTimeProvider,
    IUnitOfWork unitOfWork) 
    : ICommandHandler<LoginUserCommand, LoginUserCommandResponse>
{
    public async Task<ErrorOr<LoginUserCommandResponse>> Handle(LoginUserCommand request, CancellationToken cancellationToken)
    {
        User? user = await userRepository.GetByEmailAsync(request.Email, cancellationToken);
        
        if (user is null)
        {
            return Errors.LoginUserCommandHandlerInvalidCredentials;
        }
        
        bool isPasswordValid = passwordHashService.VerifyPassword(request.Password, user.PasswordHash);
        
        if (!isPasswordValid)
        {
            return Errors.LoginUserCommandHandlerInvalidCredentials;
        }
        
        ErrorOr<string> token = jwtService.GenerateToken(user);
        
        if (token.IsError)
        {
            return token.Errors;
        }

        if (user.IsRefreshTokenValid(user.RefreshToken ?? string.Empty))
        {
            return new LoginUserCommandResponse(token.Value, user.RefreshToken!, (DateTime)user.RefreshTokenExpiryTime!);
        }
        
        string refreshToken = jwtService.GenerateRefreshToken();
        
        DateTime refreshTokenExpiry = dateTimeProvider.UtcNow.AddMinutes(configuration.GetValue<int>("Authentication:RefreshTokenExpiryInMinutes"));

        user.SetRefreshToken(refreshToken, refreshTokenExpiry);
        
        userRepository.Update(user);
        
        await unitOfWork.SaveChangesAsync(cancellationToken);
        
        return new LoginUserCommandResponse(token.Value, refreshToken, refreshTokenExpiry);
    }
}
