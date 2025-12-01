using MediatR;
using PartsCom.Ui.Models;
using PartsCom.Ui.Filters;
using PartsCom.Ui.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace PartsCom.Ui.Controllers;

#pragma warning disable CA1515
[RequireAuthentication]
public sealed class DashboardController : Controller
{
    [HttpGet]
    public IActionResult Index()
    {
        Guid? userId = HttpContext.GetUserId();

        if (userId == null)
        {
            return RedirectToAction("Login", "Account", new { returnUrl = Url.Action("Index", "Dashboard") });
        }

        // Note: Dashboard will be integrated with real queries in future iterations
        // Currently using placeholder data for demonstration
        var model = new DashboardViewModel
        {
            Account = new AccountInfo
            {
                FullName = "Jan Kowalski",
                Email = "jan.kowalski@example.com",
                Phone = "+48 123 456 789",
                City = "Warszawa",
                AvatarUrl = "https://via.placeholder.com/150"
            },
            BillingAddress = new AddressInfo
            {
                FullName = "Jan Kowalski",
                Address1 = "ul. Przykładowa 123",
                Address2 = "m. 45",
                City = "00-001 Warszawa",
                Phone = "+48 123 456 789",
                Email = "jan.kowalski@example.com"
            },
            Stats = new StatsInfo
            {
                TotalOrders = 15,
                PendingOrders = 2,
                CompletedOrders = 13
            },
            Cards = new List<CardInfo>
            {
                new CardInfo
                {
                    Last4 = "4242",
                    Mask = "**** **** **** 4242",
                    Brand = "Visa",
                    Balance = 1250.50m,
                    Currency = "PLN"
                },
                new CardInfo
                {
                    Last4 = "8888",
                    Mask = "**** **** **** 8888",
                    Brand = "Mastercard",
                    Balance = 2340.00m,
                    Currency = "PLN"
                }
            },
            RecentOrders = new List<OrderInfo>
            {
                new OrderInfo
                {
                    OrderId = "12345",
                    Status = "Dostarczone",
                    StatusClass = "status--delivered",
                    Date = DateTime.Now.AddDays(-5),
                    Total = 299.99m,
                    ProductsCount = 3
                },
                new OrderInfo
                {
                    OrderId = "12344",
                    Status = "W trakcie realizacji",
                    StatusClass = "status--processing",
                    Date = DateTime.Now.AddDays(-2),
                    Total = 149.99m,
                    ProductsCount = 1
                }
            },
            BrowsingHistory = new List<ProductInfo>
            {
                new ProductInfo
                {
                    Title = "Przykładowy produkt 1",
                    ImageUrl = "https://via.placeholder.com/300x200",
                    Price = 99.99m,
                    Rating = 4.5,
                    Reviews = 128,
                    Badge = "HOT"
                },
                new ProductInfo
                {
                    Title = "Przykładowy produkt 2",
                    ImageUrl = "https://via.placeholder.com/300x200",
                    Price = 149.99m,
                    Rating = 4.8,
                    Reviews = 256,
                    Badge = null
                },
                new ProductInfo
                {
                    Title = "Przykładowy produkt 3",
                    ImageUrl = "https://via.placeholder.com/300x200",
                    Price = 79.99m,
                    Rating = 4.2,
                    Reviews = 64,
                    Badge = "SALE"
                }
            }
        };

        return View(model);
    }

    [HttpGet]
    public IActionResult Dashboard()
    {
        return Index();
    }
}
