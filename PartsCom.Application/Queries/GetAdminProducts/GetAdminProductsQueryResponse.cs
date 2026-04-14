using System.Diagnostics.CodeAnalysis;

namespace PartsCom.Application.Queries.GetAdminProducts;

public sealed record GetAdminProductsQueryResponse(
    List<AdminProductListItemDto> Products,
    int TotalCount,
    int PageNumber,
    int PageSize
);

[method: SuppressMessage("Design", "CA1054:URI-like parameters should not be strings", Justification = "URL stored as string in database")]
public sealed record AdminProductListItemDto(
    Guid Id,
    string Name,
    string ImageUrl,
    decimal Price,
    int Stock,
    string Category,
    string Status,
    DateTime CreatedAt
);
