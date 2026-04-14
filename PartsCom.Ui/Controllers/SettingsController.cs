using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using PartsCom.Application.Commands.CreateAddress;
using PartsCom.Application.Commands.DeleteAddress;
using PartsCom.Application.Commands.UpdateAddress;
using PartsCom.Application.Commands.UpdateUserProfile;
using PartsCom.Application.Queries.GetUserAddresses;
using PartsCom.Application.Queries.GetUserDashboard;
using PartsCom.Domain.Enums;
using PartsCom.Ui.Extensions;
using PartsCom.Ui.Filters;
using PartsCom.Ui.Models;

namespace PartsCom.Ui.Controllers;

#pragma warning disable CA1515
[RequireAuthentication]
public sealed class SettingsController(ISender sender) : Controller
{
    [HttpGet]
    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        Guid? userId = HttpContext.GetUserId();
        if (userId == null)
        {
            return RedirectToAction("Login", "Account");
        }

        ErrorOr<GetUserDashboardQueryResponse> result = await sender.Send(
            new GetUserDashboardQuery(userId.Value),
            cancellationToken);

        if (result.IsError)
        {
            TempData["Error"] = "Unable to load profile data.";
            return RedirectToAction("Index", "Dashboard");
        }

        GetUserDashboardQueryResponse data = result.Value;

        var model = new EditProfileViewModel
        {
            FirstName = data.Account.FirstName,
            LastName = data.Account.LastName,
            Email = data.Account.Email,
            PhoneNumber = data.Account.PhoneNumber ?? string.Empty,
            City = data.Account.City ?? string.Empty,
            AvatarUrl = data.Account.AvatarUrl ?? string.Empty
        };

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Index(EditProfileViewModel model, CancellationToken cancellationToken)
    {
        Guid? userId = HttpContext.GetUserId();
        if (userId == null)
        {
            return RedirectToAction("Login", "Account");
        }

        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var command = new UpdateUserProfileCommand(
            userId.Value,
            model.FirstName,
            model.LastName,
            string.IsNullOrWhiteSpace(model.PhoneNumber) ? null : model.PhoneNumber,
            string.IsNullOrWhiteSpace(model.City) ? null : model.City,
            string.IsNullOrWhiteSpace(model.AvatarUrl) ? null : model.AvatarUrl);

        ErrorOr<Unit> result = await sender.Send(command, cancellationToken);

        if (result.IsError)
        {
            foreach (Error error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
            return View(model);
        }

        TempData["Success"] = "Profile updated successfully.";
        return RedirectToAction("Index", "Dashboard");
    }

    [HttpGet]
    public async Task<IActionResult> Addresses(CancellationToken cancellationToken)
    {
        Guid? userId = HttpContext.GetUserId();
        if (userId == null)
        {
            return RedirectToAction("Login", "Account");
        }

        ErrorOr<GetUserAddressesQueryResponse> result = await sender.Send(
            new GetUserAddressesQuery(userId.Value),
            cancellationToken);

        if (result.IsError)
        {
            TempData["Error"] = "Unable to load addresses.";
            return RedirectToAction("Index", "Dashboard");
        }

        var addresses = result.Value.Addresses.Select(a => new AddressViewModel
        {
            Id = a.Id,
            FullName = a.FullName,
            AddressLine1 = a.AddressLine1,
            AddressLine2 = a.AddressLine2 ?? string.Empty,
            City = a.City,
            PostalCode = a.PostalCode,
            Country = a.Country,
            PhoneNumber = a.PhoneNumber,
            Type = a.Type,
            IsDefault = a.IsDefault
        }).ToList();

        return View(addresses);
    }

    [HttpGet]
    public IActionResult AddAddress()
    {
        return View(new AddressViewModel { Country = "Polska", Type = AddressType.Both });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AddAddress(AddressViewModel model, CancellationToken cancellationToken)
    {
        Guid? userId = HttpContext.GetUserId();
        if (userId == null)
        {
            return RedirectToAction("Login", "Account");
        }

        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var command = new CreateAddressCommand(
            userId.Value,
            model.FullName,
            model.AddressLine1,
            string.IsNullOrWhiteSpace(model.AddressLine2) ? null : model.AddressLine2,
            model.City,
            model.PostalCode,
            model.Country,
            model.PhoneNumber,
            model.Type,
            model.IsDefault);

        ErrorOr<Guid> result = await sender.Send(command, cancellationToken);

        if (result.IsError)
        {
            foreach (Error error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
            return View(model);
        }

        TempData["Success"] = "Address added successfully.";
        return RedirectToAction(nameof(Addresses));
    }

    [HttpGet]
    public async Task<IActionResult> EditAddress(Guid id, CancellationToken cancellationToken)
    {
        Guid? userId = HttpContext.GetUserId();
        if (userId == null)
        {
            return RedirectToAction("Login", "Account");
        }

        ErrorOr<GetUserAddressesQueryResponse> result = await sender.Send(
            new GetUserAddressesQuery(userId.Value),
            cancellationToken);

        if (result.IsError)
        {
            TempData["Error"] = "Unable to load address.";
            return RedirectToAction(nameof(Addresses));
        }

        AddressDto? address = result.Value.Addresses.FirstOrDefault(a => a.Id == id);

        if (address == null)
        {
            TempData["Error"] = "Address not found.";
            return RedirectToAction(nameof(Addresses));
        }

        var model = new AddressViewModel
        {
            Id = address.Id,
            FullName = address.FullName,
            AddressLine1 = address.AddressLine1,
            AddressLine2 = address.AddressLine2 ?? string.Empty,
            City = address.City,
            PostalCode = address.PostalCode,
            Country = address.Country,
            PhoneNumber = address.PhoneNumber,
            Type = address.Type,
            IsDefault = address.IsDefault
        };

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditAddress(AddressViewModel model, CancellationToken cancellationToken)
    {
        Guid? userId = HttpContext.GetUserId();
        if (userId == null)
        {
            return RedirectToAction("Login", "Account");
        }

        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var command = new UpdateAddressCommand(
            model.Id,
            userId.Value,
            model.FullName,
            model.AddressLine1,
            string.IsNullOrWhiteSpace(model.AddressLine2) ? null : model.AddressLine2,
            model.City,
            model.PostalCode,
            model.Country,
            model.PhoneNumber,
            model.IsDefault);

        ErrorOr<Unit> result = await sender.Send(command, cancellationToken);

        if (result.IsError)
        {
            foreach (Error error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
            return View(model);
        }

        TempData["Success"] = "Address updated successfully.";
        return RedirectToAction(nameof(Addresses));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteAddress(Guid id, CancellationToken cancellationToken)
    {
        Guid? userId = HttpContext.GetUserId();
        if (userId == null)
        {
            return RedirectToAction("Login", "Account");
        }

        var command = new DeleteAddressCommand(id, userId.Value);
        ErrorOr<Unit> result = await sender.Send(command, cancellationToken);

        if (result.IsError)
        {
            TempData["Error"] = result.Errors[0].Description;
        }
        else
        {
            TempData["Success"] = "Address deleted successfully.";
        }

        return RedirectToAction(nameof(Addresses));
    }

    [HttpGet]
    public IActionResult Billing()
    {
        return RedirectToAction(nameof(Addresses));
    }

    [HttpGet]
    public IActionResult Shipping()
    {
        return RedirectToAction(nameof(Addresses));
    }
}
