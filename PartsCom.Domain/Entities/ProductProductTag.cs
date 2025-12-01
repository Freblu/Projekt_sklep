namespace PartsCom.Domain.Entities;

/// <summary>
/// Join table for Many-to-Many relationship between Product and ProductTag
/// </summary>
public sealed class ProductProductTag
{
    private ProductProductTag() { }

    public Guid ProductId { get; private set; }
    public Product Product { get; private set; } = null!;

    public Guid ProductTagId { get; private set; }
    public ProductTag ProductTag { get; private set; } = null!;

    public DateTime CreatedAt { get; private set; }

    public static ProductProductTag Create(Guid productId, Guid productTagId)
    {
        return new ProductProductTag
        {
            ProductId = productId,
            ProductTagId = productTagId,
            CreatedAt = DateTime.UtcNow
        };
    }
}
