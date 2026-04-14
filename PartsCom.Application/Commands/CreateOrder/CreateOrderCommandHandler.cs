using System.Globalization;
using ErrorOr;
using PartsCom.Application.Interfaces;
using PartsCom.Domain.Entities;
using PartsCom.Domain.Errors;

namespace PartsCom.Application.Commands.CreateOrder;

internal sealed class CreateOrderCommandHandler(
    ICartRepository cartRepository,
    IOrderRepository orderRepository,
    IUnitOfWork unitOfWork,
    IDateTimeProvider dateTimeProvider
) : ICommandHandler<CreateOrderCommand, CreateOrderCommandResponse>
{
    public async Task<ErrorOr<CreateOrderCommandResponse>> Handle(
        CreateOrderCommand request,
        CancellationToken cancellationToken)
    {
        // Get cart
        Cart? cart = await cartRepository.GetByUserIdAsync(request.UserId, cancellationToken);
        if (cart == null || !cart.Items.Any())
        {
            return Errors.CartNotFound;
        }

        // Generate order number
        string orderNumber = GenerateOrderNumber();

        // Calculate total
        decimal totalAmount = cart.GetTotalPrice();

        // Create order
        var order = Order.Create(
            request.UserId,
            orderNumber,
            totalAmount,
            request.ShippingAddressId,
            request.BillingAddressId
        );

        // Add order items from cart
        foreach (CartItem cartItem in cart.Items)
        {
            var orderItem = OrderItem.Create(
                order.Id,
                cartItem.ProductId,
                cartItem.Quantity,
                cartItem.UnitPrice
            );
            order.Items.Add(orderItem);
        }

        orderRepository.Add(order);

        // Clear cart
        cart.Items.Clear();
        cart.Touch();

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return new CreateOrderCommandResponse(order.Id, order.OrderNumber, order.TotalAmount);
    }

    private string GenerateOrderNumber()
    {
        DateTime now = dateTimeProvider.UtcNow;
        return $"ORD-{now:yyyyMMdd}-{Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture)[..8].ToUpperInvariant()}";
    }
}
