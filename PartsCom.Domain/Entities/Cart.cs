namespace PartsCom.Domain.Entities;

public sealed class Cart
{
    private Cart() { }

    public Guid Id { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }

    // Relationships
    public Guid UserId { get; private set; }
    public User User { get; private set; } = null!;

    public ICollection<CartItem> Items { get; private set; } = new List<CartItem>();

    public static Cart Create(Guid userId)
    {
        return new Cart
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            CreatedAt = DateTime.UtcNow
        };
    }

    public decimal GetTotalPrice()
    {
        return Items.Sum(item => item.Quantity * item.UnitPrice);
    }

    public void Touch()
    {
        UpdatedAt = DateTime.UtcNow;
    }
}
