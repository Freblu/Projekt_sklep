using MediatR;
using PartsCom.Application.Interfaces;

namespace PartsCom.Application.Commands.DeleteProduct;

public sealed record DeleteProductCommand(Guid ProductId) : ICommand<Unit>;
