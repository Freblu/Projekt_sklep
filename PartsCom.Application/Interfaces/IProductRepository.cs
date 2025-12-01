using PartsCom.Domain.Entities;

namespace PartsCom.Application.Interfaces;

public interface IProductRepository
{
    void Add(Product product);

    Task<Product?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<IEnumerable<Product>> GetAllAsync(CancellationToken cancellationToken = default);

    Task<IEnumerable<Product>> GetByCategoryIdAsync(Guid categoryId, CancellationToken cancellationToken = default);

    Task<IEnumerable<Product>> GetActiveProductsAsync(CancellationToken cancellationToken = default);

    Task<IEnumerable<Product>> SearchProductsAsync(string searchTerm, CancellationToken cancellationToken = default);

    Task<int> GetTotalCountAsync(CancellationToken cancellationToken = default);

    Task<int> GetLowStockCountAsync(int threshold = 10, CancellationToken cancellationToken = default);

    Task<int> GetOutOfStockCountAsync(CancellationToken cancellationToken = default);

    Task<IEnumerable<Product>> GetRecentProductsAsync(int count = 10, CancellationToken cancellationToken = default);

    void Update(Product product);

    void Delete(Product product);
}
