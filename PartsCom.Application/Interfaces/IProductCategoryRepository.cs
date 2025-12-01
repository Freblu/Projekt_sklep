using PartsCom.Domain.Entities;

namespace PartsCom.Application.Interfaces;

public interface IProductCategoryRepository
{
    void Add(ProductCategory productCategory);
    
    Task<IEnumerable<ProductCategory>> GetAllAsync(CancellationToken cancellationToken = default);
    
    Task<ProductCategory?> GetByNameAsync(string name, CancellationToken cancellationToken = default);
    
    void Update(ProductCategory productCategory);
    
    void Delete(ProductCategory productCategory);
}
