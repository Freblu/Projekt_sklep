using System.Diagnostics.CodeAnalysis;

namespace PartsCom.Application.Queries.GetAdminDashboardData;

public sealed record GetAdminDashboardDataQueryResponse(
    AdminStatsDto Stats,
    List<AdminProductDto> RecentProducts,
    List<AdminOrderDto> RecentOrders,
    List<AdminUserDto> RecentUsers,
    SalesChartDto SalesData
);

public sealed record AdminStatsDto(
    int TotalProducts,
    int TotalOrders,
    int TotalCustomers,
    decimal TotalRevenue,
    decimal RevenueGrowth,
    int LowStockProducts,
    int OutOfStockProducts
);

[method: SuppressMessage("Design", "CA1054:URI-like parameters should not be strings", Justification = "URL stored as string in database")]
public sealed record AdminProductDto(
    Guid Id,
    string Name,
    string ImageUrl,
    decimal Price,
    int Stock,
    string Category,
    string Status,
    DateTime CreatedAt
);

public sealed record AdminOrderDto(
    string OrderId,
    string CustomerName,
    DateTime Date,
    decimal Total,
    string Status,
    string StatusClass,
    string PaymentMethod
);

public sealed record AdminUserDto(
    Guid Id,
    string FullName,
    string Email,
    string Role,
    DateTime JoinedDate,
    int OrdersCount
);

public sealed record SalesChartDto(
    List<string> Labels,
    List<decimal> Values
);
