using System.Diagnostics.CodeAnalysis;
using PartsCom.Application.Interfaces;

namespace PartsCom.Application.Commands.CreateProduct;

[method: SuppressMessage("Design", "CA1054:URI-like parameters should not be strings", Justification = "URL stored as string in database")]
public sealed record CreateProductCommand(
    string Name,
    string Description,
    decimal Price,
    int StockQuantity,
    Guid? ProductCategoryId,
    Guid? ProductSubCategoryId,
    string? MainImageUrl,
    string? Brand
) : ICommand<CreateProductCommandResponse>;
