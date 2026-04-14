using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using PartsCom.Application.Commands.AddProductToCart;
using PartsCom.Application.Commands.RemoveProductFromCart;
using PartsCom.Application.Queries.GetCart;
using PartsCom.Ui.Extensions;
using PartsCom.Ui.Models;

namespace PartsCom.Ui.Controllers;

public sealed class CartController(ISender sender) : Controller
{
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        Guid? userId = HttpContext.GetUserId();
        if (userId == null)
        {
            return RedirectToAction("Login", "Account", new { returnUrl = Url.Action("Index", "Cart") });
        }

        var query = new GetCartQuery(userId.Value);
        ErrorOr<GetCartQueryResponse> result = await sender.Send(query);

        if (result.IsError)
        {
            // If cart not found, show empty cart
            return View(new List<CartItemViewModel>());
        }

        // Map to ViewModel
        var viewModel = result.Value.Cart.Items.Select(item => new CartItemViewModel
        {
            CartItemId = item.Id,
            ProductId = item.ProductId,
            Name = item.ProductName,
            Quantity = item.Quantity,
            Price = item.UnitPrice,
            ImageUrl = item.ProductImageUrl ?? "/img/placeholder.png"
        }).ToList();

        return View(viewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AddToCart(Guid productId, int quantity = 1)
    {
        Guid? userId = HttpContext.GetUserId();
        if (userId == null)
        {
            string returnUrl = Request.Headers.Referer.FirstOrDefault() ?? Url.Action("Index", "Products") ?? "/";
            return RedirectToAction("Login", "Account", new { returnUrl });
        }

        var command = new AddProductToCartCommand(userId.Value, productId, quantity);
        ErrorOr<AddProductToCartCommandResponse> result = await sender.Send(command);

        if (result.IsError)
        {
            TempData["Error"] = result.Errors[0].Description;
            return RedirectToAction("Index", "Products");
        }

        TempData["Success"] = "Produkt dodany do koszyka.";
        return RedirectToAction("Index");
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> RemoveFromCart(Guid cartItemId)
    {
        Guid? userId = HttpContext.GetUserId();
        if (userId == null)
        {
            return RedirectToAction("Login", "Account", new { returnUrl = Url.Action("Index", "Cart") });
        }

        var command = new RemoveProductFromCartCommand(userId.Value, cartItemId);
        ErrorOr<Unit> result = await sender.Send(command);

        if (result.IsError)
        {
            TempData["Error"] = result.Errors[0].Description;
        }
        else
        {
            TempData["Success"] = "Produkt usunięty z koszyka.";
        }

        return RedirectToAction("Index");
    }
}
