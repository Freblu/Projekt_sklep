using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using PartsCom.Application.Commands.CreateProduct;
using PartsCom.Application.Commands.DeleteProduct;
using PartsCom.Application.Commands.UpdateProduct;
using PartsCom.Application.Queries.GetAdminDashboardData;
using PartsCom.Application.Queries.GetAdminProducts;
using PartsCom.Application.Queries.GetProductById;
using PartsCom.Ui.Filters;
using PartsCom.Ui.Models;

namespace PartsCom.Ui.Controllers;

#pragma warning disable CA1515
[RequireAuthentication]
public sealed class AdminController(ISender sender) : Controller
{
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var query = new GetAdminDashboardDataQuery();
        ErrorOr<GetAdminDashboardDataQueryResponse> result = await sender.Send(query);

        if (result.IsError)
        {
            TempData["Error"] = "Nie udało się pobrać danych dashboardu.";
            return View(new AdminDashboardViewModel());
        }

        GetAdminDashboardDataQueryResponse data = result.Value;

        var model = new AdminDashboardViewModel
        {
            Stats = new AdminStatsInfo
            {
                TotalProducts = data.Stats.TotalProducts,
                TotalOrders = data.Stats.TotalOrders,
                TotalCustomers = data.Stats.TotalCustomers,
                TotalRevenue = data.Stats.TotalRevenue,
                RevenueGrowth = data.Stats.RevenueGrowth,
                LowStockProducts = data.Stats.LowStockProducts,
                OutOfStockProducts = data.Stats.OutOfStockProducts
            },
            SalesData = new SalesChartData
            {
                Labels = data.SalesData.Labels,
                Values = data.SalesData.Values
            },
            RecentProducts = data.RecentProducts.Select(p => new AdminProductInfo
            {
                Id = p.Id,
                Name = p.Name,
                ImageUrl = p.ImageUrl,
                Price = p.Price,
                Stock = p.Stock,
                Category = p.Category,
                Status = p.Status,
                CreatedAt = p.CreatedAt
            }).ToList(),
            RecentOrders = data.RecentOrders.Select(o => new AdminOrderInfo
            {
                OrderId = o.OrderId,
                CustomerName = o.CustomerName,
                Date = o.Date,
                Total = o.Total,
                Status = o.Status,
                StatusClass = o.StatusClass,
                PaymentMethod = o.PaymentMethod
            }).ToList(),
            RecentUsers = data.RecentUsers.Select(u => new AdminUserInfo
            {
                Id = u.Id,
                FullName = u.FullName,
                Email = u.Email,
                Role = u.Role,
                JoinedDate = u.JoinedDate,
                OrdersCount = u.OrdersCount
            }).ToList()
        };

        return View(model);
    }

    [HttpGet]
    public async Task<IActionResult> Products(int page = 1, string search = "", string category = "")
    {
        var query = new GetAdminProductsQuery(
            page,
            20,
            string.IsNullOrWhiteSpace(search) ? null : search,
            string.IsNullOrWhiteSpace(category) ? null : category
        );

        ErrorOr<GetAdminProductsQueryResponse> result = await sender.Send(query);

        if (result.IsError)
        {
            TempData["Error"] = "Nie udało się pobrać produktów.";
            return View(new AdminProductListViewModel
            {
                PageNumber = page,
                PageSize = 20,
                SearchQuery = search,
                CategoryFilter = category,
                Products = new List<AdminProductInfo>(),
                TotalCount = 0
            });
        }

        GetAdminProductsQueryResponse data = result.Value;

        var model = new AdminProductListViewModel
        {
            PageNumber = data.PageNumber,
            PageSize = data.PageSize,
            SearchQuery = search,
            CategoryFilter = category,
            Products = data.Products.Select(p => new AdminProductInfo
            {
                Id = p.Id,
                Name = p.Name,
                ImageUrl = p.ImageUrl,
                Price = p.Price,
                Stock = p.Stock,
                Category = p.Category,
                Status = p.Status,
                CreatedAt = p.CreatedAt
            }).ToList(),
            TotalCount = data.TotalCount
        };

        return View(model);
    }

    [HttpGet]
    public IActionResult ProductCreate()
    {
        var model = new ProductManageViewModel
        {
            Categories = new List<string> { "Laptops", "Accessories", "Monitors", "Components", "Peripherals" },
            Status = "Active"
        };
        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ProductCreate(ProductManageViewModel model)
    {
        // if (!ModelState.IsValid)
#pragma warning disable S125
        // {
#pragma warning restore S125
        //     model.Categories = new List<string> { "Laptops", "Accessories", "Monitors", "Components", "Peripherals" };
        //     return View(model);
        // }

        Guid? categoryId = Guid.TryParse(model.Category, out Guid parsedCategoryId) ? parsedCategoryId : null;

        var command = new CreateProductCommand(
            model.Name,
            model.Description,
            model.Price,
            model.Stock,
            categoryId,
            null,
            model.ImageUrl,
            model.Brand
        );

        ErrorOr<CreateProductCommandResponse> result = await sender.Send(command);

        if (result.IsError)
        {
            TempData["Error"] = result.Errors[0].Description;
            model.Categories = new List<string> { "Laptops", "Accessories", "Monitors", "Components", "Peripherals" };
            return View(model);
        }

        TempData["Success"] = "Produkt został utworzony pomyślnie!";
        return RedirectToAction(nameof(Products));
    }

    [HttpGet]
    public async Task<IActionResult> ProductEdit(Guid id)
    {
        var query = new GetProductByIdQuery(id);
        ErrorOr<GetProductByIdQueryResponse> result = await sender.Send(query);

        if (result.IsError)
        {
            TempData["Error"] = "Nie udało się pobrać produktu.";
            return RedirectToAction(nameof(Products));
        }

        ProductDetailsDto productData = result.Value.Product;

        var model = new ProductManageViewModel
        {
            Id = id,
            Name = productData.Name,
            Description = productData.Description,
            Price = productData.Price,
            Stock = productData.StockQuantity,
            Category = "", // Note: Would need category ID, but we only have category name
            Brand = "", // Note: Brand field not yet in Product entity
            ImageUrl = productData.MainImageUrl ?? "",
            Status = "Active", // Note: IsActive field not in DTO
            Categories = new List<string> { "Laptops", "Accessories", "Monitors", "Components", "Peripherals" }
        };

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ProductEdit(ProductManageViewModel model)
    {
        // if (!ModelState.IsValid)
#pragma warning disable S125
        // {
#pragma warning restore S125
        //     // Collect model state errors for easier debugging
        //     var errors = ModelState
        //         .Where(kv => kv.Value?.Errors.Count > 0)
        //         .SelectMany(kv => kv.Value?.Errors.Select(e => $"{kv.Key}: {e.ErrorMessage}"))
        //         .ToList();
        //
        //     TempData["ModelErrors"] = string.Join("; ", errors);
        //
        //     model.Categories = new List<string> { "Laptops", "Accessories", "Monitors", "Components", "Peripherals" };
        //     return View(model);
        // }

        Guid id = model.Id;

        Guid? categoryId = Guid.TryParse(model.Category, out Guid parsedCategoryId) ? parsedCategoryId : null;

        var command = new UpdateProductCommand(
            id,
            model.Name,
            model.Description,
            model.Price,
            model.Stock,
            categoryId,
            null,
            model.ImageUrl,
            model.Status == "Active"
        );

        ErrorOr<UpdateProductCommandResponse> result = await sender.Send(command);

        if (result.IsError)
        {
            TempData["Error"] = result.Errors[0].Description;
            model.Categories = new List<string> { "Laptops", "Accessories", "Monitors", "Components", "Peripherals" };
            return View(model);
        }

        TempData["Success"] = "Produkt został zaktualizowany pomyślnie!";
        return RedirectToAction(nameof(Products));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ProductDelete(Guid id)
    {
        var command = new DeleteProductCommand(id);
        ErrorOr<Unit> result = await sender.Send(command);

        if (result.IsError)
        {
            TempData["Error"] = result.Errors[0].Description;
        }
        else
        {
            TempData["Success"] = "Produkt został usunięty pomyślnie!";
        }

        return RedirectToAction(nameof(Products));
    }

    [HttpGet]
    public IActionResult Orders()
    {
        return View();
    }

    [HttpGet]
    public IActionResult Customers()
    {
        return View();
    }

    [HttpGet]
    public IActionResult Settings()
    {
        return View();
    }
}
