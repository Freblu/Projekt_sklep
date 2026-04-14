namespace PartsCom.Ui.Models;

#pragma warning disable CA1515
public class AdminDashboardViewModel
{
    public AdminStatsInfo Stats { get; set; } = null!;
    public List<AdminProductInfo> RecentProducts { get; set; } = new();
    public List<AdminOrderInfo> RecentOrders { get; set; } = new();
    public List<AdminUserInfo> RecentUsers { get; set; } = new();
    public SalesChartData SalesData { get; set; } = null!;
}

public class AdminStatsInfo
{
    public int TotalProducts { get; set; }
    public int TotalOrders { get; set; }
    public int TotalCustomers { get; set; }
    public decimal TotalRevenue { get; set; }
    public decimal RevenueGrowth { get; set; }
    public int LowStockProducts { get; set; }
    public int OutOfStockProducts { get; set; }
}

public class AdminProductInfo
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string ImageUrl { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int Stock { get; set; }
    public string Category { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}

public class AdminOrderInfo
{
    public string OrderId { get; set; } = string.Empty;
    public string CustomerName { get; set; } = string.Empty;
    public DateTime Date { get; set; }
    public decimal Total { get; set; }
    public string Status { get; set; } = string.Empty;
    public string StatusClass { get; set; } = string.Empty;
    public string PaymentMethod { get; set; } = string.Empty;
}

public class AdminUserInfo
{
    public Guid Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
    public DateTime JoinedDate { get; set; }
    public int OrdersCount { get; set; }
}

public class SalesChartData
{
    public List<string> Labels { get; set; } = new();
    public List<decimal> Values { get; set; } = new();
}

public class ProductManageViewModel
{
#pragma warning disable S6964
    public Guid Id { get; set; }
#pragma warning restore S6964
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
#pragma warning disable S6964
    public decimal Price { get; set; }
    public int Stock { get; set; }
#pragma warning restore S6964
    public string Category { get; set; } = string.Empty;
    public string Brand { get; set; } = string.Empty;
    public string ImageUrl { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public List<string> Categories { get; set; } = new();
}

public class AdminProductListViewModel
{
    public List<AdminProductInfo> Products { get; set; } = new();
    public int TotalCount { get; set; }
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public string SearchQuery { get; set; } = string.Empty;
    public string CategoryFilter { get; set; } = string.Empty;
}
