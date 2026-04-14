using ErrorOr;
using PartsCom.Application.Interfaces;

namespace PartsCom.Application.Queries.GetCart;

public sealed record GetCartQuery(Guid UserId) : IQuery<GetCartQueryResponse>;
