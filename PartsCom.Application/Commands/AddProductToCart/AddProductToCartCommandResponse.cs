namespace PartsCom.Application.Commands.AddProductToCart;

public sealed record AddProductToCartCommandResponse(
    Guid CartId,
    Guid CartItemId
);
