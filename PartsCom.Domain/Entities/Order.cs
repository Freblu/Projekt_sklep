using PartsCom.Domain.Enums;

namespace PartsCom.Domain.Entities;

public sealed class Order
{
    private Order() { }

    public Guid Id { get; private set; }
    public string OrderNumber { get; private set; } = string.Empty;
    public OrderStatus Status { get; private set; }
    public decimal TotalAmount { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }
    public DateTime? ShippedAt { get; private set; }
    public DateTime? DeliveredAt { get; private set; }

    // Relationships
    public Guid UserId { get; private set; }
    public User User { get; private set; } = null!;

    public Guid? ShippingAddressId { get; private set; }
    public Address? ShippingAddress { get; private set; }

    public Guid? BillingAddressId { get; private set; }
    public Address? BillingAddress { get; private set; }

    public ICollection<OrderItem> Items { get; private set; } = new List<OrderItem>();

    public static Order Create(
        Guid userId,
        string orderNumber,
        decimal totalAmount,
        Guid? shippingAddressId,
        Guid? billingAddressId)
    {
        return new Order
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            OrderNumber = orderNumber,
            Status = OrderStatus.Pending,
            TotalAmount = totalAmount,
            ShippingAddressId = shippingAddressId,
            BillingAddressId = billingAddressId,
            CreatedAt = DateTime.UtcNow
        };
    }

    public void UpdateStatus(OrderStatus status)
    {
        Status = status;
        UpdatedAt = DateTime.UtcNow;

        if (status == OrderStatus.Shipped && !ShippedAt.HasValue)
        {
            ShippedAt = DateTime.UtcNow;
        }

        if (status == OrderStatus.Delivered && !DeliveredAt.HasValue)
        {
            DeliveredAt = DateTime.UtcNow;
        }
    }

    public void Cancel()
    {
        Status = OrderStatus.Cancelled;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Refund()
    {
        Status = OrderStatus.Refunded;
        UpdatedAt = DateTime.UtcNow;
    }

    public int GetItemsCount()
    {
        return Items.Sum(item => item.Quantity);
    }
}
