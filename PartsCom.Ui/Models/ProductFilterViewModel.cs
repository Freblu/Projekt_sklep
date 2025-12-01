namespace PartsCom.Ui.Models;

public class ProductFilterViewModel
{
    public string Search { get; set; }          // fraza wyszukiwania
    public string Category { get; set; }        // filtr kategorii
    public decimal? MinPrice { get; set; }
    public decimal? MaxPrice { get; set; }

    public string Sort { get; set; }            // price_asc, price_desc, name, newest
    public int Page { get; set; } = 1;
}
