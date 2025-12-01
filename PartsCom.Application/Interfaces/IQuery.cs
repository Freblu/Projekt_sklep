using ErrorOr;
using MediatR;

namespace PartsCom.Application.Interfaces;

public interface IQuery<TResponse> : IRequest<ErrorOr<TResponse>>, IBaseQuery;

public interface IQuery : IRequest<ErrorOr<Unit>>, IBaseQuery;

public interface IBaseQuery;
