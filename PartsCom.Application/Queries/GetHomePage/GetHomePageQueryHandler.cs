using System.Globalization;
using ErrorOr;
using PartsCom.Application.Interfaces;
using PartsCom.Application.Queries.GetProducts;
using PartsCom.Domain.Entities;

namespace PartsCom.Application.Queries.GetHomePage;

internal sealed class GetHomePageQueryHandler(
    IProductRepository productRepository,
    IProductCategoryRepository categoryRepository,
    INewsRepository newsRepository
) : IQueryHandler<GetHomePageQuery, GetHomePageQueryResponse>
{
    public async Task<ErrorOr<GetHomePageQueryResponse>> Handle(GetHomePageQuery request, CancellationToken cancellationToken)
    {
        // 1. Fetch Categories
        IEnumerable<ProductCategory> categories = await categoryRepository.GetAllAsync(cancellationToken);
        var categoriesDto = categories.Select(c => new ProductCategoryDto(c.Id, c.Name)).ToList();

        // 2. Fetch News
        IEnumerable<NewsPost> news = await newsRepository.GetLatestAsync(3, cancellationToken);
        var newsDto = news.Select(n => new NewsPostDto(
            n.Id, 
            n.Title, 
            n.ShortDescription, 
            n.ImageUrl, 
            n.PublishedAt.ToString("dd.MM.yyyy", CultureInfo.InvariantCulture)
        )).ToList();

        // 3. Fetch Products
        var allProducts = (await productRepository.GetActiveProductsAsync(cancellationToken)).ToList();

        // Mappers
        static ProductDto MapToDto(Product p) => new(
            p.Id,
            p.Name,
            p.Description,
            p.Price,
            p.MainImageUrl ?? p.Images.FirstOrDefault()?.ImageUrl,
            p.AverageRating,
            p.ReviewsCount,
            p.ProductCategory?.Name,
            p.ProductTags.Select(pt => pt.ProductTag.Name).ToList()
        );

        // Sections Logic
        Product? heroProduct = allProducts.OrderByDescending(p => p.Price).FirstOrDefault(); 
        
        var sideProducts = allProducts.Where(p => p.Id != heroProduct?.Id).Take(2).ToList();
        Product? promoProduct1 = sideProducts.FirstOrDefault();
        Product? promoProduct2 = sideProducts.Skip(1).FirstOrDefault();

        var featured = allProducts.OrderByDescending(p => p.CreatedAt).Take(8).Select(MapToDto).ToList();
        var bestsellers = allProducts.OrderByDescending(p => p.ReviewsCount).Take(4).Select(MapToDto).ToList();

        // Specific Category: "Karty Graficzne"
        string specificCategoryName = "Karty Graficzne";
        var specificCategoryProducts = allProducts
            .Where(p => p.ProductCategory?.Name == specificCategoryName)
            .Take(4)
            .Select(MapToDto)
            .ToList();

        // Specific Feature Product
        Product? featureProduct = allProducts.FirstOrDefault(p => p.ProductCategory?.Name == "Monitory" || p.ProductCategory?.Name == "Peryferia");

        return new GetHomePageQueryResponse(
            featured,
            bestsellers,
            categoriesDto,
            newsDto,
            heroProduct != null ? MapToDto(heroProduct) : null,
            promoProduct1 != null ? MapToDto(promoProduct1) : null,
            promoProduct2 != null ? MapToDto(promoProduct2) : null,
            featureProduct != null ? MapToDto(featureProduct) : null,
            specificCategoryProducts
        );
    }
}
