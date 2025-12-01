using PartsCom.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using PartsCom.Application.Interfaces;
using PartsCom.Infrastructure.Database;

namespace PartsCom.Infrastructure.Repositories;

internal sealed class CartRepository(PartsComDbContext dbContext) : ICartRepository
{
    public void Add(Cart cart)
    {
        dbContext.Carts.Add(cart);
    }

    public async Task<Cart?> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        return await dbContext.Carts
            .Include(c => c.Items)
                .ThenInclude(i => i.Product)
                    .ThenInclude(p => p.Images)
            .FirstOrDefaultAsync(c => c.UserId == userId, cancellationToken);
    }

    public async Task<Cart?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await dbContext.Carts
            .Include(c => c.Items)
                .ThenInclude(i => i.Product)
                    .ThenInclude(p => p.Images)
            .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
    }

    public void Update(Cart cart)
    {
        dbContext.Carts.Update(cart);
    }

    public void Delete(Cart cart)
    {
        dbContext.Carts.Remove(cart);
    }
}
