using PartsCom.Application.Interfaces;

namespace PartsCom.Application.Commands.DeleteAddress;

public sealed record DeleteAddressCommand(Guid AddressId, Guid UserId) : ICommand;
