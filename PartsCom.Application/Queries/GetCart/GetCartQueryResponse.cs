using System.Diagnostics.CodeAnalysis;

namespace PartsCom.Application.Queries.GetCart;

public sealed record GetCartQueryResponse(CartDto Cart);

public sealed record CartDto(
    Guid Id,
    List<CartItemDto> Items,
    decimal TotalPrice
);

[SuppressMessage("Design", "CA1054:URI-like parameters should not be strings", Justification = "DTO for UI consumption")]
public sealed record CartItemDto(
    Guid Id,
    Guid ProductId,
    string ProductName,
    string? ProductImageUrl,
    int Quantity,
    decimal UnitPrice,
    decimal TotalPrice
);
