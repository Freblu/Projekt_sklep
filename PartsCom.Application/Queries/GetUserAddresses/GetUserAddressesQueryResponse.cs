using PartsCom.Domain.Enums;

namespace PartsCom.Application.Queries.GetUserAddresses;

public sealed record GetUserAddressesQueryResponse(List<AddressDto> Addresses);

public sealed record AddressDto(
    Guid Id,
    string FullName,
    string AddressLine1,
    string? AddressLine2,
    string City,
    string PostalCode,
    string Country,
    string PhoneNumber,
    AddressType Type,
    bool IsDefault,
    DateTime CreatedAt
);
