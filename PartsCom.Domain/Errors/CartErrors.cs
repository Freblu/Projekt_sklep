using ErrorOr;

namespace PartsCom.Domain.Errors;

public static partial class Errors
{
    public static Error CartNotFound =>
        Error.Custom(0, "CRT001", "Cart not found for the specified user.");

    public static Error CartItemNotFound =>
        Error.Custom(0, "CRT002", "Cart item not found.");

    public static Error CartProductAlreadyInCart =>
        Error.Custom(0, "CRT003", "Product is already in the cart.");

    public static Error CartIsEmpty =>
        Error.Custom(0, "CRT004", "Cart is empty.");
}
