using PartsCom.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using PartsCom.Application.Interfaces;
using PartsCom.Infrastructure.Database;

namespace PartsCom.Infrastructure.Repositories;

internal sealed class ProductRepository(PartsComDbContext dbContext) : IProductRepository
{
    public void Add(Product product)
    {
        dbContext.Products.Add(product);
    }

    public async Task<Product?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await dbContext.Products
            .Include(p => p.ProductCategory)
            .Include(p => p.ProductSubCategory)
            .Include(p => p.Images)
            .Include(p => p.ProductTags)
                .ThenInclude(pt => pt.ProductTag)
            .Include(p => p.Reviews)
                .ThenInclude(r => r.User)
            .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
    }

    public async Task<IEnumerable<Product>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await dbContext.Products
            .Include(p => p.ProductCategory)
            .Include(p => p.ProductSubCategory)
            .Include(p => p.Images)
            .Include(p => p.ProductTags)
                .ThenInclude(pt => pt.ProductTag)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Product>> GetByCategoryIdAsync(Guid categoryId, CancellationToken cancellationToken = default)
    {
        return await dbContext.Products
            .Where(p => p.ProductCategoryId == categoryId && p.IsActive)
            .Include(p => p.ProductCategory)
            .Include(p => p.Images)
            .Include(p => p.ProductTags)
                .ThenInclude(pt => pt.ProductTag)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Product>> GetActiveProductsAsync(CancellationToken cancellationToken = default)
    {
        return await dbContext.Products
            .Where(p => p.IsActive)
            .Include(p => p.ProductCategory)
            .Include(p => p.Images)
            .Include(p => p.ProductTags)
                .ThenInclude(pt => pt.ProductTag)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Product>> SearchProductsAsync(string searchTerm, CancellationToken cancellationToken = default)
    {
        return await dbContext.Products
            .Where(p => p.IsActive &&
                       (p.Name.Contains(searchTerm) || p.Description.Contains(searchTerm)))
            .Include(p => p.ProductCategory)
            .Include(p => p.Images)
            .Include(p => p.ProductTags)
                .ThenInclude(pt => pt.ProductTag)
            .ToListAsync(cancellationToken);
    }

    public async Task<int> GetTotalCountAsync(CancellationToken cancellationToken = default)
    {
        return await dbContext.Products.CountAsync(cancellationToken);
    }

    public async Task<int> GetLowStockCountAsync(int threshold = 10, CancellationToken cancellationToken = default)
    {
        return await dbContext.Products
            .Where(p => p.StockQuantity > 0 && p.StockQuantity <= threshold)
            .CountAsync(cancellationToken);
    }

    public async Task<int> GetOutOfStockCountAsync(CancellationToken cancellationToken = default)
    {
        return await dbContext.Products
            .Where(p => p.StockQuantity == 0)
            .CountAsync(cancellationToken);
    }

    public async Task<IEnumerable<Product>> GetRecentProductsAsync(int count = 10, CancellationToken cancellationToken = default)
    {
        return await dbContext.Products
            .Include(p => p.ProductCategory)
            .Include(p => p.Images)
            .OrderByDescending(p => p.CreatedAt)
            .Take(count)
            .ToListAsync(cancellationToken);
    }

    public void Update(Product product)
    {
        dbContext.Products.Update(product);
    }

    public void Delete(Product product)
    {
        dbContext.Products.Remove(product);
    }
}
