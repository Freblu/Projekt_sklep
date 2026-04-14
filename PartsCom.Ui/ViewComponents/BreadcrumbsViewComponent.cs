using System.Globalization;
using System.Text;
using Microsoft.AspNetCore.Mvc;

namespace PartsCom.Ui.ViewComponents;

public class BreadcrumbsViewComponent : ViewComponent
{
    public IViewComponentResult Invoke()
    {
        string path = HttpContext.Request.Path.ToString();

        if (path == "/" || path.Equals("/home", StringComparison.OrdinalIgnoreCase))
        {
            return Content(string.Empty);
        }

        string[] segments = path.Split('/', StringSplitOptions.RemoveEmptyEntries);
        List<BreadcrumbItem> items =
        [
            new BreadcrumbItem("Home", "/", false)
        ];

        StringBuilder currentPathBuilder = new();

        for (int i = 0; i < segments.Length; i++)
        {
            string segment = segments[i];

            if (i == 0 && segment.Equals("Home", StringComparison.OrdinalIgnoreCase))
            {
                currentPathBuilder.Append('/').Append(segment);
                continue;
            }

            currentPathBuilder.Append('/').Append(segment);
            bool isActive = i == segments.Length - 1;

            string displayText = GetDisplayText(segment, isActive);

            items.Add(new BreadcrumbItem(displayText, currentPathBuilder.ToString(), isActive));
        }

        return View(items);
    }

    private string GetDisplayText(string segment, bool isActive)
    {
        // If this is the last segment and we have a BreadcrumbTitle in ViewData, use it
        if (isActive && ViewData["BreadcrumbTitle"] is string title && !string.IsNullOrEmpty(title))
        {
            return title;
        }

        // Skip formatting for segments that look like GUIDs or IDs - use BreadcrumbTitle if available
        if (Guid.TryParse(segment, out _) && ViewData["BreadcrumbTitle"] is string guidTitle && !string.IsNullOrEmpty(guidTitle))
        {
            return guidTitle;
        }

        // Skip "Details" segment in breadcrumbs for product pages
        if (segment.Equals("Details", StringComparison.OrdinalIgnoreCase))
        {
            return string.Empty;
        }

        return FormatSegment(segment);
    }

    private static string FormatSegment(string segment)
    {
        if (string.IsNullOrEmpty(segment))
        {
            return segment;
        }

        if (segment.Length > 1)
        {
            return char.ToUpper(segment[0], CultureInfo.InvariantCulture) + segment.Substring(1);
        }
        return segment.ToUpper(CultureInfo.InvariantCulture);
    }
}

public record BreadcrumbItem(string Text, string Path, bool IsActive);