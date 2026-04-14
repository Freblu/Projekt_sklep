using ErrorOr;
using MediatR;
using PartsCom.Application.Interfaces;
using PartsCom.Application.Queries.ValidateToken;
using PartsCom.Domain.Entities;
using PartsCom.Domain.Errors;

namespace PartsCom.Application.Commands.LogoutUser;

internal sealed class LogoutUserCommandHandler(
    ISender sender,
    IJwtService jwtService,
    IUserRepository userRepository,
    IUnitOfWork unitOfWork) : ICommandHandler<LogoutUserCommand>
{
    public async Task<ErrorOr<Unit>> Handle(LogoutUserCommand request, CancellationToken cancellationToken)
    {
        ErrorOr<Unit> validateResult = await sender.Send(new ValidateTokenQuery(request.Token), cancellationToken);

        if (validateResult.IsError)
        {
            return validateResult.Errors;
        }

        Guid? userId = jwtService.GetUserIdFromToken(request.Token);
        User? user = await userRepository.GetByIdAsync(userId, cancellationToken);

        if (user is null)
        {
            return Errors.LogoutUserCommandHandlerUserNotFound;
        }

        user.SetRefreshToken(string.Empty, DateTime.UtcNow);
        userRepository.Update(user);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
