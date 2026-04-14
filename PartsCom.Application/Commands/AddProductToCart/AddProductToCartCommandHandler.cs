using ErrorOr;
using PartsCom.Application.Interfaces;
using PartsCom.Domain.Entities;
using PartsCom.Domain.Errors;

namespace PartsCom.Application.Commands.AddProductToCart;

internal sealed class AddProductToCartCommandHandler(
    ICartRepository cartRepository,
    IProductRepository productRepository,
    IUnitOfWork unitOfWork
) : ICommandHandler<AddProductToCartCommand, AddProductToCartCommandResponse>
{
    public async Task<ErrorOr<AddProductToCartCommandResponse>> Handle(
        AddProductToCartCommand request,
        CancellationToken cancellationToken)
    {
        // Verify product exists and is active
        Product? product = await productRepository.GetByIdAsync(request.ProductId, cancellationToken);
        if (product == null)
        {
            return Errors.ProductNotFound;
        }

        if (!product.IsActive)
        {
            return Errors.ProductInactive;
        }

        if (product.StockQuantity < request.Quantity)
        {
            return Errors.ProductInsufficientStock;
        }

        // Get product price before any modifications
        decimal productPrice = product.Price;
        Guid productId = product.Id;

        // Get or create cart
        Cart? cart = await cartRepository.GetByUserIdAsync(request.UserId, cancellationToken);

        if (cart == null)
        {
            cart = Cart.Create(request.UserId);
            cartRepository.Add(cart);
        }

        // Check if product already in cart
        CartItem? existingItem = cart.Items.FirstOrDefault(i => i.ProductId == productId);
        if (existingItem != null)
        {
            // Update quantity
            existingItem.UpdateQuantity(existingItem.Quantity + request.Quantity);
        }
        else
        {
            // Add new item - use cart reference directly instead of creating with CartId
            var cartItem = CartItem.Create(cart.Id, productId, request.Quantity, productPrice);
            cart.Items.Add(cartItem);
        }

        cart.Touch();

        await unitOfWork.SaveChangesAsync(cancellationToken);

        Guid cartItemId = existingItem?.Id ?? cart.Items.Last().Id;
        return new AddProductToCartCommandResponse(cart.Id, cartItemId);
    }
}
