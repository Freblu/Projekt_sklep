namespace PartsCom.Domain.Entities;

public sealed class ProductReview
{
    private ProductReview() { }

    public Guid Id { get; private set; }
    public int Rating { get; private set; } // 1-5
    public string? Comment { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }

    // Relationships
    public Guid ProductId { get; private set; }
    public Product Product { get; private set; } = null!;

    public Guid UserId { get; private set; }
    public User User { get; private set; } = null!;

    public static ProductReview Create(Guid productId, Guid userId, int rating, string? comment)
    {
        return new ProductReview
        {
            Id = Guid.NewGuid(),
            ProductId = productId,
            UserId = userId,
            Rating = rating,
            Comment = comment,
            CreatedAt = DateTime.UtcNow
        };
    }

    public void Update(int rating, string? comment)
    {
        Rating = rating;
        Comment = comment;
        UpdatedAt = DateTime.UtcNow;
    }
}
