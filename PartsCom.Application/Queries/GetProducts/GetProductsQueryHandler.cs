using ErrorOr;
using PartsCom.Application.Interfaces;
using PartsCom.Domain.Entities;

namespace PartsCom.Application.Queries.GetProducts;

internal sealed class GetProductsQueryHandler(
    IProductRepository productRepository
) : IQueryHandler<GetProductsQuery, GetProductsQueryResponse>
{
    public async Task<ErrorOr<GetProductsQueryResponse>> Handle(
        GetProductsQuery request,
        CancellationToken cancellationToken)
    {
        IEnumerable<Product> products = string.IsNullOrWhiteSpace(request.SearchTerm)
            ? await productRepository.GetActiveProductsAsync(cancellationToken)
            : await productRepository.SearchProductsAsync(request.SearchTerm, cancellationToken);

        IEnumerable<Product> filteredProducts = products.AsEnumerable();

        // Filter by category
        if (request.CategoryId.HasValue)
        {
            filteredProducts = filteredProducts.Where(p => p.ProductCategoryId == request.CategoryId.Value);
        }

        // Filter by price range
        if (request.MinPrice.HasValue)
        {
            filteredProducts = filteredProducts.Where(p => p.Price >= request.MinPrice.Value);
        }

        if (request.MaxPrice.HasValue)
        {
            filteredProducts = filteredProducts.Where(p => p.Price <= request.MaxPrice.Value);
        }

        // Sort
        filteredProducts = request.SortBy switch
        {
            "price_asc" => filteredProducts.OrderBy(p => p.Price),
            "price_desc" => filteredProducts.OrderByDescending(p => p.Price),
            "name" => filteredProducts.OrderBy(p => p.Name),
            "rating" => filteredProducts.OrderByDescending(p => p.AverageRating),
            _ => filteredProducts.OrderByDescending(p => p.CreatedAt)
        };

        var productDtos = filteredProducts.Select(p => new ProductDto(
            p.Id,
            p.Name,
            p.Description,
            p.Price,
            p.MainImageUrl ?? p.Images.FirstOrDefault()?.ImageUrl,
            p.AverageRating,
            p.ReviewsCount,
            p.ProductCategory?.Name,
            p.ProductTags.Select(pt => pt.ProductTag.Name).ToList()
        )).ToList();

        return new GetProductsQueryResponse(productDtos);
    }
}
