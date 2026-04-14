using MediatR;
using Microsoft.AspNetCore.Mvc;
using PartsCom.Application.Queries.GetUserDashboard;
using PartsCom.Ui.Extensions;
using PartsCom.Ui.Filters;
using PartsCom.Ui.Models;

namespace PartsCom.Ui.Controllers;

#pragma warning disable CA1515
[RequireAuthentication]
public sealed class DashboardController(ISender sender) : Controller
{
    [HttpGet]
    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        Guid? userId = HttpContext.GetUserId();

        if (userId == null)
        {
            return RedirectToAction("Login", "Account", new { returnUrl = Url.Action("Index", "Dashboard") });
        }

        ErrorOr.ErrorOr<GetUserDashboardQueryResponse> result = await sender.Send(
            new GetUserDashboardQuery(userId.Value),
            cancellationToken);

        if (result.IsError)
        {
            TempData["Error"] = "Unable to load dashboard data.";
            return RedirectToAction("Login", "Account");
        }

        GetUserDashboardQueryResponse data = result.Value;

        var model = new DashboardViewModel
        {
            Account = new AccountInfo
            {
                FullName = data.Account.FullName,
                Email = data.Account.Email,
                Phone = data.Account.PhoneNumber ?? string.Empty,
                City = data.Account.City ?? string.Empty,
                AvatarUrl = data.Account.AvatarUrl ?? "https://via.placeholder.com/150"
            },
            BillingAddress = data.BillingAddress != null
                ? new AddressInfo
                {
                    FullName = data.BillingAddress.FullName,
                    Address1 = data.BillingAddress.AddressLine1,
                    Address2 = data.BillingAddress.AddressLine2 ?? string.Empty,
                    City = $"{data.BillingAddress.PostalCode} {data.BillingAddress.City}",
                    Phone = data.BillingAddress.PhoneNumber,
                    Email = data.Account.Email
                }
                : new AddressInfo
                {
                    FullName = data.Account.FullName,
                    Address1 = "No address configured",
                    Address2 = string.Empty,
                    City = string.Empty,
                    Phone = data.Account.PhoneNumber ?? string.Empty,
                    Email = data.Account.Email
                },
            Stats = new StatsInfo
            {
                TotalOrders = data.Stats.TotalOrders,
                PendingOrders = data.Stats.PendingOrders + data.Stats.ProcessingOrders,
                CompletedOrders = data.Stats.CompletedOrders
            },
            Cards = data.Cards.Select(c => new CardInfo
            {
                Last4 = c.Last4Digits,
                Mask = c.MaskedNumber,
                Brand = c.CardBrand,
                Balance = 0m, // Balance not stored in PaymentCard entity
                Currency = "PLN"
            }).ToList(),
            RecentOrders = data.RecentOrders.Select(o => new OrderInfo
            {
                OrderId = o.OrderNumber,
                Status = o.Status,
                StatusClass = o.StatusClass,
                Date = o.CreatedAt,
                Total = o.TotalAmount,
                ProductsCount = o.ItemsCount
            }).ToList(),
            BrowsingHistory = data.BrowsingHistory.Select(h => new ProductInfo
            {
                Title = h.ProductName,
                ImageUrl = h.ProductImageUrl ?? "https://via.placeholder.com/300x200",
                Price = h.ProductPrice,
                Rating = h.AverageRating,
                Reviews = h.ReviewsCount,
                Badge = null
            }).ToList()
        };

        return View(model);
    }

    [HttpGet]
    public Task<IActionResult> Dashboard(CancellationToken cancellationToken)
    {
        return Index(cancellationToken);
    }
}
