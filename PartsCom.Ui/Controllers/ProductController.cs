using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using PartsCom.Application.Queries.GetProducts;
using PartsCom.Ui.Models;

namespace PartsCom.Ui.Controllers;

public class ProductsController(ISender sender) : Controller
{
    [HttpGet]
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
}
