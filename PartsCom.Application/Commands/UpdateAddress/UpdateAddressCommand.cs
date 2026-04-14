using PartsCom.Application.Interfaces;

namespace PartsCom.Application.Commands.UpdateAddress;

public sealed record UpdateAddressCommand(
    Guid AddressId,
    Guid UserId,
    string FullName,
    string AddressLine1,
    string? AddressLine2,
    string City,
    string PostalCode,
    string Country,
    string PhoneNumber,
    bool IsDefault
) : ICommand;
