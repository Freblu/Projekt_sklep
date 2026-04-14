using Microsoft.AspNetCore.Mvc;
using PartsCom.Ui.Extensions;

namespace PartsCom.Ui.ViewComponents;

#pragma warning disable CA1515
public sealed class UserNavViewComponent : ViewComponent
{
    public IViewComponentResult Invoke()
    {
        Guid? userId = HttpContext.GetUserId();
        bool isAuthenticated = userId.HasValue;

        return View(new UserNavViewModel(isAuthenticated, userId));
    }
}

public sealed record UserNavViewModel(bool IsAuthenticated, Guid? UserId);
