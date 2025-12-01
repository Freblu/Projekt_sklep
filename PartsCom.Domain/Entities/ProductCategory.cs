using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PartsCom.Domain.Entities;

[Table("ProductCategories")]
[Index(nameof(Name), IsUnique = true)]
public sealed class ProductCategory
{
    private ProductCategory()
    {
        
    }
    
    public static ProductCategory Create(string categoryName)
    {
        var category = new ProductCategory
        {
            Id = Guid.NewGuid(),
            Name = categoryName,
            SubCategories = []
        };
        
        return category;
    }

    [Key]
    public Guid Id { get; private set; }
    
    [Required]
    [MaxLength(100)]
    public string Name { get; private set; }
    
    public ICollection<ProductSubCategory> SubCategories { get; private set; }
}
