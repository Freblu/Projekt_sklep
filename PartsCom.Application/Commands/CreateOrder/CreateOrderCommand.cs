using ErrorOr;
using PartsCom.Application.Interfaces;

namespace PartsCom.Application.Commands.CreateOrder;

public sealed record CreateOrderCommand(
    Guid UserId,
    Guid? ShippingAddressId,
    Guid? BillingAddressId
) : ICommand<CreateOrderCommandResponse>;
