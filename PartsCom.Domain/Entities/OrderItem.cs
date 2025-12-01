namespace PartsCom.Domain.Entities;

public sealed class OrderItem
{
    private OrderItem() { }

    public Guid Id { get; private set; }
    public int Quantity { get; private set; }
    public decimal UnitPrice { get; private set; }
    public decimal TotalPrice { get; private set; }

    // Relationships
    public Guid OrderId { get; private set; }
    public Order Order { get; private set; } = null!;

    public Guid ProductId { get; private set; }
    public Product Product { get; private set; } = null!;

    public static OrderItem Create(Guid orderId, Guid productId, int quantity, decimal unitPrice)
    {
        return new OrderItem
        {
            Id = Guid.NewGuid(),
            OrderId = orderId,
            ProductId = productId,
            Quantity = quantity,
            UnitPrice = unitPrice,
            TotalPrice = quantity * unitPrice
        };
    }
}
