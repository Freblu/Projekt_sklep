using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using PartsCom.Application.Queries.GetAllProductCategories;

namespace PartsCom.Ui.ViewComponents;

#pragma warning disable CA1515
public sealed class CategoriesDropdownViewComponent(ISender sender) : ViewComponent
{
    public async Task<IViewComponentResult> InvokeAsync()
    {
        ErrorOr<GetAllProductCategoriesQueryResponse> result = await sender.Send(new GetAllProductCategoriesQuery());

        if (result.IsError)
        {
            return View(new GetAllProductCategoriesQueryResponse([]));
        }

        return View(result.Value);
    }
}


