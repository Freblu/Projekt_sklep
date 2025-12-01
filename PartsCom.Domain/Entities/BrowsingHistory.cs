namespace PartsCom.Domain.Entities;

public sealed class BrowsingHistory
{
    private BrowsingHistory() { }

    public Guid Id { get; private set; }
    public DateTime ViewedAt { get; private set; }

    // Relationships
    public Guid UserId { get; private set; }
    public User User { get; private set; } = null!;

    public Guid ProductId { get; private set; }
    public Product Product { get; private set; } = null!;

    public static BrowsingHistory Create(Guid userId, Guid productId)
    {
        return new BrowsingHistory
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            ProductId = productId,
            ViewedAt = DateTime.UtcNow
        };
    }
}
