using ErrorOr;
using MediatR;
using PartsCom.Application.Interfaces;
using PartsCom.Domain.Entities;
using PartsCom.Domain.Errors;

namespace PartsCom.Application.Commands.RegisterUser;

internal sealed class RegisterUserCommandHandler(
    IUserRepository userRepository,
    IPasswordHashService passwordHashService,
    IUnitOfWork unitOfWork) : ICommandHandler<RegisterUserCommand>
{
    public async Task<ErrorOr<Unit>> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        if (await userRepository.EmailExistsAsync(request.Email, cancellationToken))
        {
            return Errors.RegisterUserCommandHandlerEmailAlreadyExists;
        }

        ErrorOr<string> hashedPassword = passwordHashService.HashPassword(request.Password);

        if (hashedPassword.IsError)
        {
            return hashedPassword.Errors;
        }

        var user = User.Create(
            request.FirstName,
            request.LastName,
            request.Email,
            hashedPassword.Value);

        userRepository.Add(user);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
