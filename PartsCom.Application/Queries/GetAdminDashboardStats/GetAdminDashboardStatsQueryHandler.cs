using ErrorOr;
using PartsCom.Application.Interfaces;

namespace PartsCom.Application.Queries.GetAdminDashboardStats;

internal sealed class GetAdminDashboardStatsQueryHandler(
    IProductRepository productRepository,
    IOrderRepository orderRepository,
    IUserRepository userRepository
) : IQueryHandler<GetAdminDashboardStatsQuery, GetAdminDashboardStatsQueryResponse>
{
    public async Task<ErrorOr<GetAdminDashboardStatsQueryResponse>> Handle(
        GetAdminDashboardStatsQuery request,
        CancellationToken cancellationToken)
    {
        int totalProducts = await productRepository.GetTotalCountAsync(cancellationToken);
        int totalOrders = await orderRepository.GetTotalCountAsync(cancellationToken);
        int totalCustomers = await userRepository.GetTotalCountAsync(cancellationToken);
        decimal totalRevenue = await orderRepository.GetTotalRevenueAsync(cancellationToken);
        int lowStockProducts = await productRepository.GetLowStockCountAsync(10, cancellationToken);
        int outOfStockProducts = await productRepository.GetOutOfStockCountAsync(cancellationToken);

        // Note: Revenue growth calculation would require historical data - using placeholder for now
        decimal revenueGrowth = 12.5m;

        return new GetAdminDashboardStatsQueryResponse(
            totalProducts,
            totalOrders,
            totalCustomers,
            totalRevenue,
            revenueGrowth,
            lowStockProducts,
            outOfStockProducts
        );
    }
}
