using ErrorOr;
using MediatR;
using PartsCom.Application.Interfaces;
using PartsCom.Domain.Entities;

namespace PartsCom.Application.Commands.UpdateAddress;

internal sealed class UpdateAddressCommandHandler(
    IAddressRepository addressRepository,
    IUnitOfWork unitOfWork
) : ICommandHandler<UpdateAddressCommand>
{
    public async Task<ErrorOr<Unit>> Handle(UpdateAddressCommand request, CancellationToken cancellationToken)
    {
        Address? address = await addressRepository.GetByIdAsync(request.AddressId, cancellationToken);

        if (address == null)
        {
            return Error.NotFound("Address.NotFound", "Address not found.");
        }

        // Verify the address belongs to the user
        if (address.UserId != request.UserId)
        {
            return Error.Forbidden("Address.Forbidden", "You don't have permission to update this address.");
        }

        // If setting as default, unset other defaults
        if (request.IsDefault)
        {
            await addressRepository.UnsetDefaultAddressesAsync(request.UserId, address.Type, cancellationToken);
        }

        address.Update(
            request.FullName,
            request.AddressLine1,
            request.AddressLine2,
            request.City,
            request.PostalCode,
            request.Country,
            request.PhoneNumber);

        if (request.IsDefault)
        {
            address.SetAsDefault();
        }
        else
        {
            address.UnsetDefault();
        }

        addressRepository.Update(address);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
