namespace PartsCom.Ui.Models;

public sealed class CartItemViewModel
{
    public string Name { get; set; }
    public int Quantity { get; set; }
    public decimal Price { get; set; }
    public string ImageUrl { get; set; }
}
