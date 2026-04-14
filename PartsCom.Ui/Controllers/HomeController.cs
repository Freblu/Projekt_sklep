using System.Diagnostics;
using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PartsCom.Application.Queries.GetHomePage;
using PartsCom.Ui.Models;

namespace PartsCom.Ui.Controllers;

#pragma warning disable CA1515, CA5395
public sealed class HomeController(ISender sender) : Controller
{
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        ErrorOr<GetHomePageQueryResponse> result = await sender.Send(new GetHomePageQuery());

        if (result.IsError)
        {
             // Handle error appropriately, possibly returning an empty model or error view
             return View(new GetHomePageQueryResponse([], [], [], [], null, null, null, null, []));
        }

        return View(result.Value);
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
