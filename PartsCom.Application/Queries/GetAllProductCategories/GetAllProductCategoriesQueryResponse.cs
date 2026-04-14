namespace PartsCom.Application.Queries.GetAllProductCategories;

public sealed record GetAllProductCategoriesQueryResponse(IEnumerable<ProductCategoryDto> ProductCategories);

public sealed record ProductCategoryDto(Guid Id, string Name, IEnumerable<ProductSubcategoryDto> Subcategories);

public sealed record ProductSubcategoryDto(Guid Id, string Name, Guid ParentCategoryId);
