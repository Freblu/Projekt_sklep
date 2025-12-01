using PartsCom.Application.Interfaces;

namespace PartsCom.Application.Queries.ValidateToken;

public sealed record ValidateTokenQuery(string Token) : IQuery;
