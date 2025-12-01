using ErrorOr;
using MediatR;
using PartsCom.Ui.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace PartsCom.Ui.Filters;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
internal sealed class RequireAuthenticationAttribute : Attribute, IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        ISender? sender = context.HttpContext.RequestServices.GetService<ISender>();

        if (sender is null)
        {
            context.Result = new StatusCodeResult(StatusCodes.Status500InternalServerError);
            return;
        }
        
        ErrorOr<Unit> validationResult = await context.HttpContext.ValidateTokenAsync(sender);

        if (validationResult.IsError)
        {
            ErrorOr<string> refreshResult = await context.HttpContext.RefreshTokenAsync(sender);

            if (refreshResult.IsError)
            {
                string returnUrl = context.HttpContext.Request.Path + context.HttpContext.Request.QueryString;
                context.Result = new RedirectToActionResult(
                    actionName: "Login",
                    controllerName: "Account",
                    routeValues: new { returnUrl });
                return;
            }
            
            validationResult = await context.HttpContext.ValidateTokenAsync(sender);

            if (validationResult.IsError)
            {
                string returnUrl = context.HttpContext.Request.Path + context.HttpContext.Request.QueryString;
                context.Result = new RedirectToActionResult(
                    actionName: "Login",
                    controllerName: "Account",
                    routeValues: new { returnUrl });
                return;
            }
        }
        
        await next();
    }
}

