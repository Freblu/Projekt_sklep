using ErrorOr;
using PartsCom.Application.Interfaces;
using PartsCom.Domain.Entities;

namespace PartsCom.Application.Commands.CreateAddress;

internal sealed class CreateAddressCommandHandler(
    IAddressRepository addressRepository,
    IUnitOfWork unitOfWork
) : ICommandHandler<CreateAddressCommand, Guid>
{
    public async Task<ErrorOr<Guid>> Handle(CreateAddressCommand request, CancellationToken cancellationToken)
    {
        // If this is set as default, unset other defaults for this user
        if (request.IsDefault)
        {
            await addressRepository.UnsetDefaultAddressesAsync(request.UserId, request.Type, cancellationToken);
        }

        var address = Address.Create(
            request.UserId,
            request.FullName,
            request.AddressLine1,
            request.AddressLine2,
            request.City,
            request.PostalCode,
            request.Country,
            request.PhoneNumber,
            request.Type,
            request.IsDefault);

        addressRepository.Add(address);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return address.Id;
    }
}
