using ErrorOr;
using MediatR;
using PartsCom.Application.Interfaces;
using PartsCom.Domain.Entities;
using PartsCom.Domain.Errors;

namespace PartsCom.Application.Commands.AddProductCategory;

internal sealed class AddProductCategoryCommandHandler(
    IProductCategoryRepository productCategoryRepository,
    IUnitOfWork unitOfWork) : ICommandHandler<AddProductCategoryCommand>
{
    public async Task<ErrorOr<Unit>> Handle(AddProductCategoryCommand request, CancellationToken cancellationToken)
    {
        ProductCategory? existingCategory = await productCategoryRepository.GetByNameAsync(request.Name, cancellationToken);

        if (existingCategory is not null)
        {
            return Errors.AddProductCategoryCommandHandlerCategoryAlreadyExists;
        }

        var newCategory = ProductCategory.Create(request.Name);

        foreach (string subcategory in request.Subcategories)
        {
            newCategory.SubCategories.Add(ProductSubCategory.Create(subcategory));
        }

        productCategoryRepository.Add(newCategory);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
