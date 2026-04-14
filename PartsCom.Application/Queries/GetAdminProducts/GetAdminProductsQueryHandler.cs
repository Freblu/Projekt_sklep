using ErrorOr;
using PartsCom.Application.Interfaces;
using PartsCom.Domain.Entities;

namespace PartsCom.Application.Queries.GetAdminProducts;

internal sealed class GetAdminProductsQueryHandler(
    IProductRepository productRepository
) : IQueryHandler<GetAdminProductsQuery, GetAdminProductsQueryResponse>
{
    public async Task<ErrorOr<GetAdminProductsQueryResponse>> Handle(
        GetAdminProductsQuery request,
        CancellationToken cancellationToken)
    {
        IEnumerable<Product> allProducts = await productRepository.GetAllAsync(cancellationToken);

        // Apply filters
        IEnumerable<Product> filteredProducts = allProducts.AsEnumerable();

        if (!string.IsNullOrWhiteSpace(request.SearchQuery))
        {
            filteredProducts = filteredProducts.Where(p =>
                p.Name.Contains(request.SearchQuery, StringComparison.OrdinalIgnoreCase) ||
                p.Description.Contains(request.SearchQuery, StringComparison.OrdinalIgnoreCase));
        }

        if (!string.IsNullOrWhiteSpace(request.CategoryFilter))
        {
            filteredProducts = filteredProducts.Where(p =>
                p.ProductCategory?.Name?.Contains(request.CategoryFilter, StringComparison.OrdinalIgnoreCase) == true);
        }

        var filteredProductsList = filteredProducts.ToList();
        int totalCount = filteredProductsList.Count;

        // Apply pagination
        var paginatedProducts = filteredProductsList
            .OrderByDescending(p => p.CreatedAt)
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToList();

        var productDtos = paginatedProducts.Select(p =>
        {
            string status = "Active";
            if (p.StockQuantity == 0)
            {
                status = "Out of Stock";
            }
            else if (p.StockQuantity <= 10)
            {
                status = "Low Stock";
            }

            return new AdminProductListItemDto(
                p.Id,
                p.Name,
                p.MainImageUrl ?? "https://via.placeholder.com/100",
                p.Price,
                p.StockQuantity,
                p.ProductCategory?.Name ?? "Brak kategorii",
                status,
                p.CreatedAt
            );
        }).ToList();

        return new GetAdminProductsQueryResponse(
            productDtos,
            totalCount,
            request.Page,
            request.PageSize
        );
    }
}
