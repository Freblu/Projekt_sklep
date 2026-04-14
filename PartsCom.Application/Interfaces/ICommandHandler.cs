using ErrorOr;
using MediatR;

namespace PartsCom.Application.Interfaces;

public interface ICommandHandler<in TCommand> : IRequestHandler<TCommand, ErrorOr<Unit>>
    where TCommand : ICommand;

public interface ICommandHandler<in TCommand, TResponse> : IRequestHandler<TCommand, ErrorOr<TResponse>>
    where TCommand : ICommand<TResponse>;
