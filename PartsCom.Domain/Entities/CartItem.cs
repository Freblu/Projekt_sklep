namespace PartsCom.Domain.Entities;

public sealed class CartItem
{
    private CartItem() { }

    public Guid Id { get; private set; }
    public int Quantity { get; private set; }
    public decimal UnitPrice { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }

    // Relationships
    public Guid CartId { get; private set; }
    public Cart Cart { get; private set; } = null!;

    public Guid ProductId { get; private set; }
    public Product Product { get; private set; } = null!;

    public static CartItem Create(Guid cartId, Guid productId, int quantity, decimal unitPrice)
    {
        return new CartItem
        {
            Id = Guid.NewGuid(),
            CartId = cartId,
            ProductId = productId,
            Quantity = quantity,
            UnitPrice = unitPrice,
            CreatedAt = DateTime.UtcNow
        };
    }

    public void UpdateQuantity(int quantity)
    {
        Quantity = quantity;
        UpdatedAt = DateTime.UtcNow;
    }

    public void UpdatePrice(decimal unitPrice)
    {
        UnitPrice = unitPrice;
        UpdatedAt = DateTime.UtcNow;
    }

    public decimal GetTotalPrice()
    {
        return Quantity * UnitPrice;
    }
}
