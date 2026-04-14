using System.Diagnostics.CodeAnalysis;

namespace PartsCom.Application.Queries.GetUserDashboard;

public sealed record GetUserDashboardQueryResponse(
    UserAccountDto Account,
    UserAddressDto? BillingAddress,
    UserStatsDto Stats,
    List<UserPaymentCardDto> Cards,
    List<UserOrderDto> RecentOrders,
    List<UserBrowsingHistoryDto> BrowsingHistory
);

[method: SuppressMessage("Design", "CA1054:URI-like parameters should not be strings", Justification = "URL stored as string in database")]
public sealed record UserAccountDto(
    Guid Id,
    string FirstName,
    string LastName,
    string FullName,
    string Email,
    string? PhoneNumber,
    string? City,
    string? AvatarUrl
);

public sealed record UserAddressDto(
    Guid Id,
    string FullName,
    string AddressLine1,
    string? AddressLine2,
    string City,
    string PostalCode,
    string Country,
    string PhoneNumber,
    bool IsDefault
);

public sealed record UserStatsDto(
    int TotalOrders,
    int PendingOrders,
    int ProcessingOrders,
    int CompletedOrders
);

public sealed record UserPaymentCardDto(
    Guid Id,
    string Last4Digits,
    string MaskedNumber,
    string CardBrand,
    string CardHolderName,
    string ExpiryMonth,
    string ExpiryYear,
    bool IsDefault
);

public sealed record UserOrderDto(
    Guid Id,
    string OrderNumber,
    string Status,
    string StatusClass,
    DateTime CreatedAt,
    decimal TotalAmount,
    int ItemsCount
);

[method: SuppressMessage("Design", "CA1054:URI-like parameters should not be strings", Justification = "URL stored as string in database")]
public sealed record UserBrowsingHistoryDto(
    Guid ProductId,
    string ProductName,
    string? ProductImageUrl,
    decimal ProductPrice,
    double AverageRating,
    int ReviewsCount,
    DateTime ViewedAt
);
