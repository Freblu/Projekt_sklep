using ErrorOr;
using PartsCom.Application.Interfaces;
using PartsCom.Domain.Entities;
using PartsCom.Domain.Errors;

namespace PartsCom.Application.Commands.UpdateProduct;

internal sealed class UpdateProductCommandHandler(
    IProductRepository productRepository,
    IUnitOfWork unitOfWork
) : ICommandHandler<UpdateProductCommand, UpdateProductCommandResponse>
{
    public async Task<ErrorOr<UpdateProductCommandResponse>> Handle(
        UpdateProductCommand request,
        CancellationToken cancellationToken)
    {
        Product? product = await productRepository.GetByIdAsync(request.ProductId, cancellationToken);

        if (product is null)
        {
            return Errors.ProductNotFound;
        }

        product.UpdateDetails(request.Name, request.Description, request.Price);
        product.UpdateStock(request.StockQuantity);
        product.UpdateCategories(request.ProductCategoryId, request.ProductSubCategoryId);

        if (!string.IsNullOrWhiteSpace(request.MainImageUrl))
        {
            product.SetMainImage(request.MainImageUrl);
        }

        if (request.IsActive)
        {
            product.Activate();
        }
        else
        {
            product.Deactivate();
        }

        productRepository.Update(product);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return new UpdateProductCommandResponse(product.Id);
    }
}
