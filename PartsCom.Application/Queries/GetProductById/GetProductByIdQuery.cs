using ErrorOr;
using PartsCom.Application.Interfaces;

namespace PartsCom.Application.Queries.GetProductById;

public sealed record GetProductByIdQuery(Guid ProductId) : IQuery<GetProductByIdQueryResponse>;
