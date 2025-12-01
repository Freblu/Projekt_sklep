using PartsCom.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using PartsCom.Application.Interfaces;
using PartsCom.Infrastructure.Database;

namespace PartsCom.Infrastructure.Repositories;

internal sealed class ProductCategoryRepository(PartsComDbContext dbContext) : IProductCategoryRepository
{
    public void Add(ProductCategory productCategory)
    {
        dbContext.ProductCategories.Add(productCategory);
    }

    public async Task<IEnumerable<ProductCategory>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await dbContext.ProductCategories.Include(x => x.SubCategories).ToListAsync(cancellationToken);
    }

    public async Task<ProductCategory?> GetByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        return await dbContext.ProductCategories
            .Include(x => x.SubCategories)
            .FirstOrDefaultAsync(x => x.Name == name, cancellationToken);
    }

    public void Update(ProductCategory productCategory)
    {
        dbContext.ProductCategories.Update(productCategory);
    }

    public void Delete(ProductCategory productCategory)
    {
        dbContext.ProductCategories.Remove(productCategory);
    }
}
