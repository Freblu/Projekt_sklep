using ErrorOr;
using PartsCom.Application.Interfaces;
using PartsCom.Domain.Entities;

namespace PartsCom.Application.Queries.GetUserAddresses;

internal sealed class GetUserAddressesQueryHandler(
    IAddressRepository addressRepository
) : IQueryHandler<GetUserAddressesQuery, GetUserAddressesQueryResponse>
{
    public async Task<ErrorOr<GetUserAddressesQueryResponse>> Handle(
        GetUserAddressesQuery request,
        CancellationToken cancellationToken)
    {
        IEnumerable<Address> addresses = await addressRepository.GetByUserIdAsync(
            request.UserId,
            cancellationToken);

        var addressDtos = addresses.Select(a => new AddressDto(
            a.Id,
            a.FullName,
            a.AddressLine1,
            a.AddressLine2,
            a.City,
            a.PostalCode,
            a.Country,
            a.PhoneNumber,
            a.Type,
            a.IsDefault,
            a.CreatedAt
        )).ToList();

        return new GetUserAddressesQueryResponse(addressDtos);
    }
}
