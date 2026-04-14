using ErrorOr;
using MediatR;
using PartsCom.Application.Interfaces;
using PartsCom.Domain.Entities;

namespace PartsCom.Application.Commands.DeleteAddress;

internal sealed class DeleteAddressCommandHandler(
    IAddressRepository addressRepository,
    IUnitOfWork unitOfWork
) : ICommandHandler<DeleteAddressCommand>
{
    public async Task<ErrorOr<Unit>> Handle(DeleteAddressCommand request, CancellationToken cancellationToken)
    {
        Address? address = await addressRepository.GetByIdAsync(request.AddressId, cancellationToken);

        if (address == null)
        {
            return Error.NotFound("Address.NotFound", "Address not found.");
        }

        // Verify the address belongs to the user
        if (address.UserId != request.UserId)
        {
            return Error.Forbidden("Address.Forbidden", "You don't have permission to delete this address.");
        }

        addressRepository.Delete(address);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
