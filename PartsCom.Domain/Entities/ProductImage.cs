using System.Diagnostics.CodeAnalysis;

namespace PartsCom.Domain.Entities;

public sealed class ProductImage
{
    private ProductImage() { }

    public Guid Id { get; private set; }
    public string ImageUrl { get; private set; } = string.Empty;
    public int DisplayOrder { get; private set; }
    public DateTime CreatedAt { get; private set; }

    // Relationships
    public Guid ProductId { get; private set; }
    public Product Product { get; private set; } = null!;

    [SuppressMessage("Design", "CA1054:URI-like parameters should not be strings", Justification = "URL stored as string in database")]
    public static ProductImage Create(Guid productId, string imageUrl, int displayOrder)
    {
        return new ProductImage
        {
            Id = Guid.NewGuid(),
            ProductId = productId,
            ImageUrl = imageUrl,
            DisplayOrder = displayOrder,
            CreatedAt = DateTime.UtcNow
        };
    }
}
