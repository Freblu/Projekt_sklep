using System.Diagnostics.CodeAnalysis;

namespace PartsCom.Domain.Entities;

public sealed class Product
{
    private Product() { }

    public Guid Id { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;
    public decimal Price { get; private set; }
    public int StockQuantity { get; private set; }
    public string? MainImageUrl { get; private set; }
    public double AverageRating { get; private set; }
    public int ReviewsCount { get; private set; }
    public bool IsActive { get; private set; } = true;
    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }

    // Relationships
    public Guid? ProductCategoryId { get; private set; }
    public ProductCategory? ProductCategory { get; private set; }

    public Guid? ProductSubCategoryId { get; private set; }
    public ProductSubCategory? ProductSubCategory { get; private set; }

    public ICollection<ProductImage> Images { get; private set; } = new List<ProductImage>();
    public ICollection<ProductProductTag> ProductTags { get; private set; } = new List<ProductProductTag>();
    public ICollection<ProductReview> Reviews { get; private set; } = new List<ProductReview>();
    public ICollection<CartItem> CartItems { get; private set; } = new List<CartItem>();
    public ICollection<OrderItem> OrderItems { get; private set; } = new List<OrderItem>();
    public ICollection<BrowsingHistory> BrowsingHistories { get; private set; } = new List<BrowsingHistory>();

    public static Product Create(
        string name,
        string description,
        decimal price,
        int stockQuantity,
        Guid? productCategoryId,
        Guid? productSubCategoryId)
    {
        return new Product
        {
            Id = Guid.NewGuid(),
            Name = name,
            Description = description,
            Price = price,
            StockQuantity = stockQuantity,
            ProductCategoryId = productCategoryId,
            ProductSubCategoryId = productSubCategoryId,
            IsActive = true,
            CreatedAt = DateTime.UtcNow,
            AverageRating = 0,
            ReviewsCount = 0
        };
    }

    public void UpdateDetails(string name, string description, decimal price)
    {
        Name = name;
        Description = description;
        Price = price;
        UpdatedAt = DateTime.UtcNow;
    }

    public void UpdateStock(int quantity)
    {
        StockQuantity = quantity;
        UpdatedAt = DateTime.UtcNow;
    }

    [SuppressMessage("Design", "CA1054:URI-like parameters should not be strings", Justification = "URL stored as string in database")]
    public void SetMainImage(string imageUrl)
    {
        MainImageUrl = imageUrl;
        UpdatedAt = DateTime.UtcNow;
    }

    public void UpdateRating(double averageRating, int reviewsCount)
    {
        AverageRating = averageRating;
        ReviewsCount = reviewsCount;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Deactivate()
    {
        IsActive = false;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Activate()
    {
        IsActive = true;
        UpdatedAt = DateTime.UtcNow;
    }

    public void UpdateCategories(Guid? productCategoryId, Guid? productSubCategoryId)
    {
        ProductCategoryId = productCategoryId;
        ProductSubCategoryId = productSubCategoryId;
        UpdatedAt = DateTime.UtcNow;
    }
}
