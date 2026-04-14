using PartsCom.Application.Interfaces;
using PartsCom.Domain.Enums;

namespace PartsCom.Application.Commands.CreateAddress;

public sealed record CreateAddressCommand(
    Guid UserId,
    string FullName,
    string AddressLine1,
    string? AddressLine2,
    string City,
    string PostalCode,
    string Country,
    string PhoneNumber,
    AddressType Type,
    bool IsDefault
) : ICommand<Guid>;
