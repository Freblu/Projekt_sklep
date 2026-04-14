namespace PartsCom.Ui.Models;

public class ProductDetailsViewModel
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public decimal? OriginalPrice { get; set; }
    public int StockQuantity { get; set; }
    public string MainImageUrl { get; set; } = string.Empty;
    public List<string> ImageUrls { get; set; } = [];
    public double AverageRating { get; set; }
    public int ReviewsCount { get; set; }
    public string CategoryName { get; set; } = string.Empty;
    public string? SubCategoryName { get; set; }
    public List<string> Tags { get; set; } = [];
    public List<ProductReviewViewModel> Reviews { get; set; } = [];

    public bool IsInStock => StockQuantity > 0;
    public int? DiscountPercent => OriginalPrice.HasValue && OriginalPrice > Price
        ? (int)Math.Round((1 - Price / OriginalPrice.Value) * 100)
        : null;
}

public class ProductReviewViewModel
{
    public Guid Id { get; set; }
    public string UserName { get; set; } = string.Empty;
    public int Rating { get; set; }
    public string? Comment { get; set; }
    public DateTime CreatedAt { get; set; }
}
