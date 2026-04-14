using ErrorOr;
using PartsCom.Application.Interfaces;
using PartsCom.Domain.Enums;

namespace PartsCom.Application.Queries.GetUserDashboard;

internal sealed class GetUserDashboardQueryHandler(
    IUserRepository userRepository
) : IQueryHandler<GetUserDashboardQuery, GetUserDashboardQueryResponse>
{
    public async Task<ErrorOr<GetUserDashboardQueryResponse>> Handle(
        GetUserDashboardQuery request,
        CancellationToken cancellationToken)
    {
        Domain.Entities.User? user = await userRepository.GetByIdWithDashboardDataAsync(
            request.UserId,
            cancellationToken);

        if (user == null)
        {
            return Error.NotFound("User.NotFound", "User not found.");
        }

        // Map account info
        var account = new UserAccountDto(
            user.Id,
            user.FirstName,
            user.LastName,
            $"{user.FirstName} {user.LastName}",
            user.Email,
            user.PhoneNumber,
            user.City,
            user.AvatarUrl
        );

        // Map billing address (first default or first available)
        UserAddressDto? billingAddress = user.Addresses
            .Where(a => a.Type == AddressType.Billing || a.IsDefault)
            .OrderByDescending(a => a.IsDefault)
            .Select(a => new UserAddressDto(
                a.Id,
                a.FullName,
                a.AddressLine1,
                a.AddressLine2,
                a.City,
                a.PostalCode,
                a.Country,
                a.PhoneNumber,
                a.IsDefault
            ))
            .FirstOrDefault();

        // Calculate order stats
        int totalOrders = user.Orders.Count;
        int pendingOrders = user.Orders.Count(o => o.Status == OrderStatus.Pending);
        int processingOrders = user.Orders.Count(o => o.Status == OrderStatus.Processing || o.Status == OrderStatus.Shipped);
        int completedOrders = user.Orders.Count(o => o.Status == OrderStatus.Delivered);

        var stats = new UserStatsDto(totalOrders, pendingOrders, processingOrders, completedOrders);

        // Map payment cards
        var cards = user.PaymentCards
            .OrderByDescending(c => c.IsDefault)
            .Select(c => new UserPaymentCardDto(
                c.Id,
                c.Last4Digits,
                c.GetMaskedCardNumber(),
                c.CardBrand,
                c.CardHolderName,
                c.ExpiryMonth,
                c.ExpiryYear,
                c.IsDefault
            ))
            .ToList();

        // Map recent orders (last 5)
        var recentOrders = user.Orders
            .OrderByDescending(o => o.CreatedAt)
            .Take(5)
            .Select(o => new UserOrderDto(
                o.Id,
                o.OrderNumber,
                GetOrderStatusText(o.Status),
                GetOrderStatusClass(o.Status),
                o.CreatedAt,
                o.TotalAmount,
                o.GetItemsCount()
            ))
            .ToList();

        // Map browsing history (last 6)
        var browsingHistory = user.BrowsingHistories
            .OrderByDescending(h => h.ViewedAt)
            .Take(6)
            .Select(h => new UserBrowsingHistoryDto(
                h.ProductId,
                h.Product.Name,
                h.Product.MainImageUrl,
                h.Product.Price,
                h.Product.AverageRating,
                h.Product.ReviewsCount,
                h.ViewedAt
            ))
            .ToList();

        return new GetUserDashboardQueryResponse(
            account,
            billingAddress,
            stats,
            cards,
            recentOrders,
            browsingHistory
        );
    }

    private static string GetOrderStatusText(OrderStatus status)
    {
        return status switch
        {
            OrderStatus.Pending => "Oczekujące",
            OrderStatus.Processing => "W trakcie realizacji",
            OrderStatus.Shipped => "Wysłane",
            OrderStatus.Delivered => "Dostarczone",
            OrderStatus.Cancelled => "Anulowane",
            OrderStatus.Refunded => "Zwrócone",
            _ => "Nieznany"
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
            OrderStatus.Refunded => "status--refunded",
            _ => "status--unknown"
        };
    }
}
