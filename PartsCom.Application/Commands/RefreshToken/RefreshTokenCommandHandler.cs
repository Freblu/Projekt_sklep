using ErrorOr;
using PartsCom.Application.Interfaces;
using PartsCom.Domain.Entities;
using PartsCom.Domain.Errors;

namespace PartsCom.Application.Commands.RefreshToken;

internal sealed class RefreshTokenCommandHandler(IUserRepository userRepository, IJwtService jwtService) : ICommandHandler<RefreshTokenCommand, string>
{
    public async Task<ErrorOr<string>> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
    {
        User? user = await userRepository.GetByRefreshTokenAsync(request.RefreshToken, cancellationToken);

        if (user is null || !user.IsRefreshTokenValid(request.RefreshToken))
        {
            return Errors.RefreshTokenCommandHandlerInvalidRefreshToken;
        }

        return jwtService.GenerateToken(user);
    }
}
