using ErrorOr;
using MediatR;
using PartsCom.Application.Interfaces;

namespace PartsCom.Application.Commands.UpdateUserProfile;

internal sealed class UpdateUserProfileCommandHandler(
    IUserRepository userRepository,
    IUnitOfWork unitOfWork
) : ICommandHandler<UpdateUserProfileCommand>
{
    public async Task<ErrorOr<Unit>> Handle(UpdateUserProfileCommand request, CancellationToken cancellationToken)
    {
        Domain.Entities.User? user = await userRepository.GetByIdAsync(request.UserId, cancellationToken);

        if (user == null)
        {
            return Error.NotFound("User.NotFound", "User not found.");
        }

        user.UpdateFullProfile(
            request.FirstName,
            request.LastName,
            request.PhoneNumber,
            request.City,
            request.AvatarUrl);

        userRepository.Update(user);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
