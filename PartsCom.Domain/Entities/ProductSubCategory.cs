using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace PartsCom.Domain.Entities;

[Table("ProductSubCategories")]
[Index(nameof(Name), IsUnique = true)]
public sealed class ProductSubCategory
{
    private ProductSubCategory()
    {

    }

    public static ProductSubCategory Create(string subCategoryName)
    {
        var subCategory = new ProductSubCategory
        {
            Id = Guid.NewGuid(),
            Name = subCategoryName
        };

        return subCategory;
    }

    [Key]
    public Guid Id { get; private set; }

    [Required]
    [MaxLength(100)]
    public string Name { get; private set; }

    public Guid? ProductCategoryId { get; private set; }

    public ProductCategory? ProductCategory { get; private set; }
}
