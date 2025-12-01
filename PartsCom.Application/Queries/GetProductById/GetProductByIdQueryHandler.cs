using ErrorOr;
using PartsCom.Application.Interfaces;
using PartsCom.Domain.Entities;
using PartsCom.Domain.Errors;

namespace PartsCom.Application.Queries.GetProductById;

internal sealed class GetProductByIdQueryHandler(
    IProductRepository productRepository
) : IQueryHandler<GetProductByIdQuery, GetProductByIdQueryResponse>
{
    public async Task<ErrorOr<GetProductByIdQueryResponse>> Handle(
        GetProductByIdQuery request,
        CancellationToken cancellationToken)
    {
        Product? product = await productRepository.GetByIdAsync(request.ProductId, cancellationToken);

        if (product == null)
        {
            return Errors.ProductNotFound;
        }

        var productDto = new ProductDetailsDto(
            product.Id,
            product.Name,
            product.Description,
            product.Price,
            product.StockQuantity,
            product.MainImageUrl ?? product.Images.FirstOrDefault()?.ImageUrl,
            product.Images.OrderBy(i => i.DisplayOrder).Select(i => i.ImageUrl).ToList(),
            product.AverageRating,
            product.ReviewsCount,
            product.ProductCategory?.Name,
            product.ProductSubCategory?.Name,
            product.ProductTags.Select(pt => pt.ProductTag.Name).ToList(),
            product.Reviews.OrderByDescending(r => r.CreatedAt).Select(r => new ReviewDto(
                r.Id,
                $"{r.User.FirstName} {r.User.LastName}",
                r.Rating,
                r.Comment,
                r.CreatedAt
            )).ToList()
        );

        return new GetProductByIdQueryResponse(productDto);
    }
}
