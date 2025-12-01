namespace PartsCom.Ui.Models;

public class ProductListItemViewModel
{
    public Guid Id { get; set; }

    public string Name { get; set; }

    public decimal Price { get; set; }

    public string ImageUrl { get; set; }

    public string Category { get; set; }

    public List<string> Tags { get; set; } = new();
}
