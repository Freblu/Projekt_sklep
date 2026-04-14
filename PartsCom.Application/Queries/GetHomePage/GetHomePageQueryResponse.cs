using System.Diagnostics.CodeAnalysis;
using PartsCom.Application.Queries.GetProducts;

namespace PartsCom.Application.Queries.GetHomePage;

public sealed record GetHomePageQueryResponse(
    List<ProductDto> FeaturedProducts,
    List<ProductDto> Bestsellers,
    List<ProductCategoryDto> Categories,
    List<NewsPostDto> News,
    ProductDto? HeroProduct,
    ProductDto? PromoProduct1, // For side banner
    ProductDto? PromoProduct2, // For side banner
    ProductDto? SpecificFeatureProduct, // For the single product placeholder
    List<ProductDto> SpecificCategoryProducts // For the specific category section
);

public sealed record ProductCategoryDto(Guid Id, string Name);

[SuppressMessage("Design", "CA1054:URI-like parameters should not be strings", Justification = "DTO for UI consumption")]
public sealed record NewsPostDto(Guid Id, string Title, string ShortDescription, string ImageUrl, string PublishedAt);