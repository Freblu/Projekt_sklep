using ErrorOr;
using PartsCom.Application.Interfaces;
using PartsCom.Domain.Entities;

namespace PartsCom.Application.Commands.CreateProduct;

internal sealed class CreateProductCommandHandler(
    IProductRepository productRepository,
    IUnitOfWork unitOfWork
) : ICommandHandler<CreateProductCommand, CreateProductCommandResponse>
{
    public async Task<ErrorOr<CreateProductCommandResponse>> Handle(
        CreateProductCommand request,
        CancellationToken cancellationToken)
    {
        var product = Product.Create(
            request.Name,
            request.Description,
            request.Price,
            request.StockQuantity,
            request.ProductCategoryId,
            request.ProductSubCategoryId
        );

        if (!string.IsNullOrWhiteSpace(request.MainImageUrl))
        {
            product.SetMainImage(request.MainImageUrl);
        }

        productRepository.Add(product);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return new CreateProductCommandResponse(product.Id);
    }
}
