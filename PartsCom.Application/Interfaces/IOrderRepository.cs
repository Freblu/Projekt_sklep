using PartsCom.Domain.Entities;

namespace PartsCom.Application.Interfaces;

public interface IOrderRepository
{
    void Add(Order order);

    Task<Order?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<Order?> GetByOrderNumberAsync(string orderNumber, CancellationToken cancellationToken = default);

    Task<IEnumerable<Order>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);

    Task<int> GetTotalCountAsync(CancellationToken cancellationToken = default);

    Task<decimal> GetTotalRevenueAsync(CancellationToken cancellationToken = default);

    Task<IEnumerable<Order>> GetRecentOrdersAsync(int count = 10, CancellationToken cancellationToken = default);

    void Update(Order order);

    void Delete(Order order);
}
