namespace PartsCom.Domain.Entities;

public sealed class ProductTag
{
    private ProductTag() { }

    public Guid Id { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public DateTime CreatedAt { get; private set; }

    // Relationships
    public ICollection<ProductProductTag> ProductProductTags { get; private set; } = new List<ProductProductTag>();

    public static ProductTag Create(string name)
    {
        return new ProductTag
        {
            Id = Guid.NewGuid(),
            Name = name,
            CreatedAt = DateTime.UtcNow
        };
    }
}
