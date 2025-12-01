using PartsCom.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using PartsCom.Application.Interfaces;
using PartsCom.Infrastructure.Database;

namespace PartsCom.Infrastructure.Repositories;

internal sealed class OrderRepository(PartsComDbContext dbContext) : IOrderRepository
{
    public void Add(Order order)
    {
        dbContext.Orders.Add(order);
    }

    public async Task<Order?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await dbContext.Orders
            .Include(o => o.Items)
                .ThenInclude(i => i.Product)
            .Include(o => o.ShippingAddress)
            .Include(o => o.BillingAddress)
            .FirstOrDefaultAsync(o => o.Id == id, cancellationToken);
    }

    public async Task<Order?> GetByOrderNumberAsync(string orderNumber, CancellationToken cancellationToken = default)
    {
        return await dbContext.Orders
            .Include(o => o.Items)
                .ThenInclude(i => i.Product)
            .Include(o => o.ShippingAddress)
            .Include(o => o.BillingAddress)
            .FirstOrDefaultAsync(o => o.OrderNumber == orderNumber, cancellationToken);
    }

    public async Task<IEnumerable<Order>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        return await dbContext.Orders
            .Where(o => o.UserId == userId)
            .Include(o => o.Items)
                .ThenInclude(i => i.Product)
            .Include(o => o.ShippingAddress)
            .Include(o => o.BillingAddress)
            .OrderByDescending(o => o.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<int> GetTotalCountAsync(CancellationToken cancellationToken = default)
    {
        return await dbContext.Orders.CountAsync(cancellationToken);
    }

    public async Task<decimal> GetTotalRevenueAsync(CancellationToken cancellationToken = default)
    {
        return await dbContext.Orders
            .SumAsync(o => o.TotalAmount, cancellationToken);
    }

    public async Task<IEnumerable<Order>> GetRecentOrdersAsync(int count = 10, CancellationToken cancellationToken = default)
    {
        return await dbContext.Orders
            .Include(o => o.User)
            .Include(o => o.Items)
            .OrderByDescending(o => o.CreatedAt)
            .Take(count)
            .ToListAsync(cancellationToken);
    }

    public void Update(Order order)
    {
        dbContext.Orders.Update(order);
    }

    public void Delete(Order order)
    {
        dbContext.Orders.Remove(order);
    }
}
