using PartsCom.Domain.Entities;

namespace PartsCom.Application.Interfaces;

public interface ICartRepository
{
    void Add(Cart cart);

    Task<Cart?> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);

    Task<Cart?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    void Update(Cart cart);

    void Delete(Cart cart);
}
