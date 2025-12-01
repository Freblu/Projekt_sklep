using ErrorOr;
using MediatR;

namespace PartsCom.Application.Interfaces;

public interface IQueryHandler<in TQuery, TResponse> : IRequestHandler<TQuery, ErrorOr<TResponse>>
    where TQuery : IQuery<TResponse>;

public interface IQueryHandler<in TQuery> : IRequestHandler<TQuery, ErrorOr<Unit>>
    where TQuery : IQuery;
