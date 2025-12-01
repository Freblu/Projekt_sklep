using ErrorOr;
using PartsCom.Application.Interfaces;

namespace PartsCom.Application.Commands.RemoveProductFromCart;

public sealed record RemoveProductFromCartCommand(
    Guid UserId,
    Guid CartItemId
) : ICommand;
