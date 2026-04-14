using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using PartsCom.Application.Queries.GetProductById;
using PartsCom.Application.Queries.GetProducts;
using PartsCom.Ui.Models;

namespace PartsCom.Ui.Controllers;

[Route("[controller]")]
public class ProductsController(ISender sender) : Controller
{
    [HttpGet("")]
    public async Task<IActionResult> Index([FromQuery] ProductFilterViewModel filters)
    {
        // Parse category to Guid if provided
        Guid? categoryId = null;
        if (!string.IsNullOrWhiteSpace(filters.Category) && Guid.TryParse(filters.Category, out Guid parsedCategoryId))
        {
            categoryId = parsedCategoryId;
        }

        var query = new GetProductsQuery(
            filters.Search,
            categoryId,
            filters.MinPrice,
            filters.MaxPrice,
            filters.Sort
        );

        ErrorOr<GetProductsQueryResponse> result = await sender.Send(query);

        if (result.IsError)
        {
            TempData["Error"] = "Nie udało się pobrać produktów.";
            return View(new List<ProductListItemViewModel>());
        }

        // Map to ViewModel
        var viewModel = result.Value.Products.Select(p => new ProductListItemViewModel
        {
            Id = p.Id,
            Name = p.Name,
            Price = p.Price,
            Category = p.CategoryName ?? "Brak kategorii",
            Tags = p.Tags,
            ImageUrl = p.MainImageUrl ?? "/img/placeholder.png"
        }).ToList();

        return View(viewModel);
    }

    [HttpGet("Details/{id:guid}")]
    public async Task<IActionResult> Details(Guid id)
    {
        var query = new GetProductByIdQuery(id);
        ErrorOr<GetProductByIdQueryResponse> result = await sender.Send(query);

        if (result.IsError)
        {
            TempData["Error"] = "Nie udało się znaleźć produktu.";
            return RedirectToAction(nameof(Index));
        }

        ProductDetailsDto product = result.Value.Product;

        var viewModel = new ProductDetailsViewModel
        {
            Id = product.Id,
            Name = product.Name,
            Description = product.Description,
            Price = product.Price,
            StockQuantity = product.StockQuantity,
            MainImageUrl = product.MainImageUrl ?? "/img/placeholder.png",
            ImageUrls = product.ImageUrls.Count > 0 ? product.ImageUrls : ["/img/placeholder.png"],
            AverageRating = product.AverageRating,
            ReviewsCount = product.ReviewsCount,
            CategoryName = product.CategoryName ?? "Brak kategorii",
            SubCategoryName = product.SubCategoryName,
            Tags = product.Tags,
            Reviews = product.Reviews.Select(r => new ProductReviewViewModel
            {
                Id = r.Id,
                UserName = r.UserName,
                Rating = r.Rating,
                Comment = r.Comment,
                CreatedAt = r.CreatedAt
            }).ToList()
        };

        ViewData["BreadcrumbTitle"] = product.Name;

        return View(viewModel);
    }
}
