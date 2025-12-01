using ErrorOr;
using PartsCom.Application.Interfaces;

namespace PartsCom.Application.Queries.GetProducts;

public sealed record GetProductsQuery(
    string? SearchTerm,
    Guid? CategoryId,
    decimal? MinPrice,
    decimal? MaxPrice,
    string? SortBy
) : IQuery<GetProductsQueryResponse>;
