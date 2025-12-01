using System.Diagnostics.CodeAnalysis;

namespace PartsCom.Application.Queries.GetProductById;

public sealed record GetProductByIdQueryResponse(ProductDetailsDto Product);

[SuppressMessage("Design", "CA1054:URI-like parameters should not be strings", Justification = "DTO for UI consumption")]
public sealed record ProductDetailsDto(
    Guid Id,
    string Name,
    string Description,
    decimal Price,
    int StockQuantity,
    string? MainImageUrl,
    List<string> ImageUrls,
    double AverageRating,
    int ReviewsCount,
    string? CategoryName,
    string? SubCategoryName,
    List<string> Tags,
    List<ReviewDto> Reviews
);

public sealed record ReviewDto(
    Guid Id,
    string UserName,
    int Rating,
    string? Comment,
    DateTime CreatedAt
);
