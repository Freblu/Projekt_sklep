namespace PartsCom.Ui.Models;

#pragma warning disable CA1515
public class DashboardViewModel
{
    public AccountInfo Account { get; set; } = null!;
    public AddressInfo BillingAddress { get; set; } = null!;
    public StatsInfo Stats { get; set; } = null!;
    public List<CardInfo> Cards { get; set; } = new();
    public List<OrderInfo> RecentOrders { get; set; } = new();
    public List<ProductInfo> BrowsingHistory { get; set; } = new();
}

public class AccountInfo
{
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string AvatarUrl { get; set; } = string.Empty;
}

public class AddressInfo
{
    public string FullName { get; set; } = string.Empty;
    public string Address1 { get; set; } = string.Empty;
    public string Address2 { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
}

public class StatsInfo
{
    public int TotalOrders { get; set; }
    public int PendingOrders { get; set; }
    public int CompletedOrders { get; set; }
}

public class CardInfo
{
    public string Last4 { get; set; } = string.Empty;
    public string Mask { get; set; } = string.Empty;
    public string Brand { get; set; } = string.Empty;
    public decimal Balance { get; set; }
    public string Currency { get; set; } = string.Empty;
}

public class OrderInfo
{
    public string OrderId { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public string StatusClass { get; set; } = string.Empty;
    public DateTime Date { get; set; }
    public decimal Total { get; set; }
    public int ProductsCount { get; set; }
}

public class ProductInfo
{
    public string Title { get; set; } = string.Empty;
    public string ImageUrl { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public double Rating { get; set; }
    public int Reviews { get; set; }
    public string? Badge { get; set; }
}
