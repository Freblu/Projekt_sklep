using ErrorOr;

namespace PartsCom.Domain.Errors;

public static partial class Errors
{
    public static Error ProductNotFound =>
        Error.Custom(0, "PRD001", "Product with the specified ID was not found.");

    public static Error ProductInsufficientStock =>
        Error.Custom(0, "PRD002", "Insufficient stock for the requested quantity.");

    public static Error ProductInactive =>
        Error.Custom(0, "PRD003", "Product is not available for purchase.");
}
