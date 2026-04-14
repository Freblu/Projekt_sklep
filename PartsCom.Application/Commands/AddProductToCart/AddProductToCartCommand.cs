using ErrorOr;
using PartsCom.Application.Interfaces;

namespace PartsCom.Application.Commands.AddProductToCart;

public sealed record AddProductToCartCommand(
    Guid UserId,
    Guid ProductId,
    int Quantity
) : ICommand<AddProductToCartCommandResponse>;
