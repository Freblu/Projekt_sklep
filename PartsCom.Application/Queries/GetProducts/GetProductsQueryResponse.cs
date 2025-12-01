using System.Diagnostics.CodeAnalysis;

namespace PartsCom.Application.Queries.GetProducts;

public sealed record GetProductsQueryResponse(List<ProductDto> Products);

[SuppressMessage("Design", "CA1054:URI-like parameters should not be strings", Justification = "DTO for UI consumption")]
public sealed record ProductDto(
    Guid Id,
    string Name,
    string Description,
    decimal Price,
    string? MainImageUrl,
    double AverageRating,
    int ReviewsCount,
    string? CategoryName,
    List<string> Tags
);
