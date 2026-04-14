using PartsCom.Application.Interfaces;

namespace PartsCom.Application.Commands.AddProductCategory;

public sealed record AddProductCategoryCommand(string Name, IEnumerable<string> Subcategories) : ICommand;
