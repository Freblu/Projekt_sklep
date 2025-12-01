namespace PartsCom.Application.Queries.GetAdminDashboardStats;

public sealed record GetAdminDashboardStatsQueryResponse(
    int TotalProducts,
    int TotalOrders,
    int TotalCustomers,
    decimal TotalRevenue,
    decimal RevenueGrowth,
    int LowStockProducts,
    int OutOfStockProducts
);
