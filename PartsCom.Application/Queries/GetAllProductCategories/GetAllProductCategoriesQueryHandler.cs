using ErrorOr;
using PartsCom.Application.Interfaces;
using PartsCom.Domain.Entities;

namespace PartsCom.Application.Queries.GetAllProductCategories;

internal sealed class GetAllProductCategoriesQueryHandler(IProductCategoryRepository productCategoryRepository)
    : IQueryHandler<GetAllProductCategoriesQuery, GetAllProductCategoriesQueryResponse>
{
    public async Task<ErrorOr<GetAllProductCategoriesQueryResponse>> Handle(GetAllProductCategoriesQuery request, CancellationToken cancellationToken)
    {
        IEnumerable<ProductCategory> productCategories = await productCategoryRepository.GetAllAsync(cancellationToken);

        IEnumerable<ProductCategoryDto> productCategoryDtos = productCategories
            .Select(pc => new ProductCategoryDto(pc.Id, pc.Name,
                pc.SubCategories.Select(psc => new ProductSubcategoryDto(psc.Id, psc.Name, pc.Id))));

        return new GetAllProductCategoriesQueryResponse(productCategoryDtos);
    }
}
