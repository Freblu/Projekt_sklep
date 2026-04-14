using ErrorOr;
using MediatR;
using PartsCom.Application.Interfaces;
using PartsCom.Domain.Entities;
using PartsCom.Domain.Errors;

namespace PartsCom.Application.Commands.RemoveProductFromCart;

internal sealed class RemoveProductFromCartCommandHandler(
    ICartRepository cartRepository,
    IUnitOfWork unitOfWork
) : ICommandHandler<RemoveProductFromCartCommand>
{
    public async Task<ErrorOr<Unit>> Handle(
        RemoveProductFromCartCommand request,
        CancellationToken cancellationToken)
    {
        Cart? cart = await cartRepository.GetByUserIdAsync(request.UserId, cancellationToken);
        if (cart == null)
        {
            return Errors.CartNotFound;
        }

        CartItem? cartItem = cart.Items.FirstOrDefault(i => i.Id == request.CartItemId);
        if (cartItem == null)
        {
            return Errors.CartItemNotFound;
        }

        cart.Items.Remove(cartItem);
        cart.Touch();

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
