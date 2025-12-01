using ErrorOr;
using MediatR;
using PartsCom.Application.Interfaces;
using PartsCom.Domain.Entities;
using PartsCom.Domain.Errors;

namespace PartsCom.Application.Commands.DeleteProduct;

internal sealed class DeleteProductCommandHandler(
    IProductRepository productRepository,
    IUnitOfWork unitOfWork
) : ICommandHandler<DeleteProductCommand, Unit>
{
    public async Task<ErrorOr<Unit>> Handle(
        DeleteProductCommand request,
        CancellationToken cancellationToken)
    {
        Product? product = await productRepository.GetByIdAsync(request.ProductId, cancellationToken);

        if (product is null)
        {
            return Errors.ProductNotFound;
        }

        productRepository.Delete(product);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
