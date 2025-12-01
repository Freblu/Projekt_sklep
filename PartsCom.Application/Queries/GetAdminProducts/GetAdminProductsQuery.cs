using PartsCom.Application.Interfaces;

namespace PartsCom.Application.Queries.GetAdminProducts;

public sealed record GetAdminProductsQuery(
    int Page,
    int PageSize,
    string? SearchQuery,
    string? CategoryFilter
) : IQuery<GetAdminProductsQueryResponse>;
