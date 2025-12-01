using System.Diagnostics;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PartsCom.Application.Commands.AddProductCategory;
using PartsCom.Ui.Models;

namespace PartsCom.Ui.Controllers;

#pragma warning disable CA1515, CA5395
public sealed class HomeController : Controller
{
    [HttpGet]
    public IActionResult Index()
    {
        // Note: Homepage can use GetProductsQuery for featured products in future

        return View();
    }

    [HttpGet]
    [AllowAnonymous]
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(
            new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier }
        );
    }
}
