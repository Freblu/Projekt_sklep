using ErrorOr;
using PartsCom.Application.Interfaces;
using PartsCom.Domain.Entities;
using PartsCom.Domain.Enums;

namespace PartsCom.Application.Queries.GetAdminDashboardData;

internal sealed class GetAdminDashboardDataQueryHandler(
    IProductRepository productRepository,
    IOrderRepository orderRepository,
    IUserRepository userRepository
) : IQueryHandler<GetAdminDashboardDataQuery, GetAdminDashboardDataQueryResponse>
{
    public async Task<ErrorOr<GetAdminDashboardDataQueryResponse>> Handle(
        GetAdminDashboardDataQuery request,
        CancellationToken cancellationToken)
    {
        // Get stats
        int totalProducts = await productRepository.GetTotalCountAsync(cancellationToken);
        int totalOrders = await orderRepository.GetTotalCountAsync(cancellationToken);
        int totalCustomers = await userRepository.GetTotalCountAsync(cancellationToken);
        decimal totalRevenue = await orderRepository.GetTotalRevenueAsync(cancellationToken);
        int lowStockProducts = await productRepository.GetLowStockCountAsync(10, cancellationToken);
        int outOfStockProducts = await productRepository.GetOutOfStockCountAsync(cancellationToken);

        var stats = new AdminStatsDto(
            totalProducts,
            totalOrders,
            totalCustomers,
            totalRevenue,
            12.5m, // Note: Revenue growth would require historical data
            lowStockProducts,
            outOfStockProducts
        );

        // Get recent products
        IEnumerable<Product> recentProducts = await productRepository.GetRecentProductsAsync(4, cancellationToken);
        var recentProductDtos = recentProducts.Select(p =>
        {
            string status = "Active";
            if (p.StockQuantity == 0)
            {
                status = "Out of Stock";
            }
            else if (p.StockQuantity <= 10)
            {
                status = "Low Stock";
            }

            return new AdminProductDto(
                p.Id,
                p.Name,
                p.MainImageUrl ?? "https://via.placeholder.com/100",
                p.Price,
                p.StockQuantity,
                p.ProductCategory?.Name ?? "Brak kategorii",
                status,
                p.CreatedAt
            );
        }).ToList();

        // Get recent orders
        IEnumerable<Order> recentOrders = await orderRepository.GetRecentOrdersAsync(4, cancellationToken);
        var recentOrderDtos = recentOrders.Select(o => new AdminOrderDto(
            o.OrderNumber,
            $"{o.User?.FirstName ?? ""} {o.User?.LastName ?? ""}".Trim(),
            o.CreatedAt,
            o.TotalAmount,
            GetOrderStatusText(o.Status),
            GetOrderStatusClass(o.Status),
            "Credit Card" // Note: Payment method would come from payment info when implemented
        )).ToList();

        // Get recent users
        IEnumerable<User> recentUsers = await userRepository.GetRecentUsersAsync(2, cancellationToken);
        var recentUserDtos = recentUsers.Select(u => new AdminUserDto(
            u.Id,
            $"{u.FirstName} {u.LastName}",
            u.Email,
            "Customer", // Note: Role would come from UserRoles when role check is implemented
            u.CreatedAt,
            u.Orders?.Count ?? 0
        )).ToList();

        // Generate sales data (mock data - would come from actual sales in production)
        var salesData = new SalesChartDto(
            new List<string> { "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul" },
            new List<decimal> { 12500, 15200, 13800, 18900, 21300, 19800, 24500 }
        );

        return new GetAdminDashboardDataQueryResponse(
            stats,
            recentProductDtos,
            recentOrderDtos,
            recentUserDtos,
            salesData
        );
    }

    private static string GetOrderStatusText(OrderStatus status)
    {
        return status switch
        {
            OrderStatus.Pending => "Pending",
            OrderStatus.Processing => "Processing",
            OrderStatus.Shipped => "Shipped",
            OrderStatus.Delivered => "Delivered",
            OrderStatus.Cancelled => "Cancelled",
            _ => "Unknown"
        };
    }

    private static string GetOrderStatusClass(OrderStatus status)
    {
        return status switch
        {
            OrderStatus.Pending => "status--pending",
            OrderStatus.Processing => "status--processing",
            OrderStatus.Shipped => "status--shipped",
            OrderStatus.Delivered => "status--delivered",
            OrderStatus.Cancelled => "status--cancelled",
            _ => "status--unknown"
        };
    }
}
