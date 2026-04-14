using ErrorOr;
using MediatR;

namespace PartsCom.Application.Interfaces;

public interface ICommand : IRequest<ErrorOr<Unit>>, IBaseCommand;

public interface ICommand<TResponse> : IRequest<ErrorOr<TResponse>>, IBaseCommand;

public interface IBaseCommand;
