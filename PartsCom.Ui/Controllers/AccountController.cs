using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using PartsCom.Application.Commands.LoginUser;
using PartsCom.Application.Commands.RegisterUser;
using PartsCom.Ui.Extensions;
using PartsCom.Ui.Models;

namespace PartsCom.Ui.Controllers;

#pragma warning disable CA1515, CA1054, S4144
public sealed class AccountController(ISender sender) : Controller
{
    [HttpGet]
    public IActionResult Login(string? returnUrl = null)
    {
        ViewData["ReturnUrl"] = returnUrl;
        return View(new LoginViewModel { RememberMe = false });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginViewModel model, string? returnUrl = null)
    {
        ViewData["ReturnUrl"] = returnUrl;

        if (ModelState.IsValid)
        {
            ErrorOr<LoginUserCommandResponse> result = await HttpContext.LoginAsync(model.Email, model.Password, sender);

            if (result.IsError)
            {
                AddErrors(result.Errors);
                return View(model);
            }


            return RedirectToLocal(returnUrl);
        }

        return View(model);
    }

    [HttpGet]
    public IActionResult Register(string? returnUrl = null)
    {
        ViewData["ReturnUrl"] = returnUrl;
        return View(new RegisterViewModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(RegisterViewModel model, string? returnUrl = null)
    {
        ViewData["ReturnUrl"] = returnUrl;

        if (ModelState.IsValid)
        {
            var command = new RegisterUserCommand(model.FirstName, model.LastName, model.Email, model.Password, model.ConfirmPassword);
            ErrorOr<Unit> result = await sender.Send(command);

            if (result.IsError)
            {
                AddErrors(result.Errors);
                return View(model);
            }

            TempData["Message"] = "Rejestracja zakończona powodzeniem. Możesz się zalogować.";
            return RedirectToAction(nameof(Login));
        }

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.LogoutAsync(sender);
        TempData["Message"] = "Wylogowano pomyślnie.";
        return RedirectToAction("Index", "Home");
    }

    [HttpGet]
    public IActionResult Edit()
    {
        return RedirectToAction("Index", "Settings");
    }

    [HttpGet]
    public IActionResult Details()
    {
        return RedirectToAction("Index", "Settings");
    }

    [HttpGet]
    public IActionResult Security()
    {
        // Password change functionality will be implemented in future iteration
        return RedirectToAction("Index", "Settings");
    }

    private IActionResult RedirectToLocal(string? returnUrl)
    {
        if (Url.IsLocalUrl(returnUrl))
        {
            return Redirect(returnUrl);
        }

        return RedirectToAction("Index", "Dashboard");
    }

    private void AddErrors(IEnumerable<Error> errors)
    {
        foreach (Error error in errors)
        {
            ModelState.AddModelError(string.Empty, error.Description);
        }
    }
}
