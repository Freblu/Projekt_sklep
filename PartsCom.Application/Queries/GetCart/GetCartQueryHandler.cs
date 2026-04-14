using ErrorOr;
using PartsCom.Application.Interfaces;
using PartsCom.Domain.Entities;
using PartsCom.Domain.Errors;

namespace PartsCom.Application.Queries.GetCart;

internal sealed class GetCartQueryHandler(
    ICartRepository cartRepository
) : IQueryHandler<GetCartQuery, GetCartQueryResponse>
{
    public async Task<ErrorOr<GetCartQueryResponse>> Handle(
        GetCartQuery request,
        CancellationToken cancellationToken)
    {
        Cart? cart = await cartRepository.GetByUserIdWithProductsAsync(request.UserId, cancellationToken);

        if (cart == null)
        {
            return Errors.CartNotFound;
        }

        var cartDto = new CartDto(
            cart.Id,
            cart.Items.Select(item => new CartItemDto(
                item.Id,
                item.ProductId,
                item.Product.Name,
                item.Product.MainImageUrl ?? item.Product.Images.FirstOrDefault()?.ImageUrl,
                item.Quantity,
                item.UnitPrice,
                item.GetTotalPrice()
            )).ToList(),
            cart.GetTotalPrice()
        );

        return new GetCartQueryResponse(cartDto);
    }
}
