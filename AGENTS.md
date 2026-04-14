# AGENTS.md - Coding Agent Guidelines for PartsCom

## Project Overview

PartsCom is a .NET 9.0 e-commerce application built with ASP.NET Core MVC and .NET Aspire for orchestration. It follows Clean Architecture with Domain-Driven Design principles.

**Tech Stack:**
- Language: C# 13 / .NET 9.0
- Framework: ASP.NET Core MVC with .NET Aspire
- Database: SQL Server via Entity Framework Core 9
- CQRS: MediatR for Command/Query separation
- Validation: FluentValidation
- Error Handling: ErrorOr pattern
- File Storage: MinIO (S3-compatible)
- Auth: JWT Bearer tokens

## Build/Run/Test Commands

```bash
# Build entire solution
dotnet build

# Run with Aspire orchestration (recommended for full stack)
dotnet run --project PartsCom.AppHost

# Run only the UI project
dotnet run --project PartsCom.Ui

# Format code according to .editorconfig
dotnet format

# Add EF Core migration
dotnet ef migrations add <MigrationName> --project PartsCom.Infrastructure --startup-project PartsCom.AppHost

# Apply migrations
dotnet ef database update --project PartsCom.Infrastructure --startup-project PartsCom.AppHost
```

**Note:** This project currently has no test projects configured. If tests are added, use:
```bash
# Run all tests
dotnet test

# Run single test
dotnet test --filter "FullyQualifiedName~TestClassName.TestMethodName"

# Run tests in specific project
dotnet test path/to/TestProject.csproj
```

## Project Structure

```
PartsCom/
├── PartsCom.AppHost/       # .NET Aspire orchestration host
├── PartsCom.Domain/        # Domain layer (entities, enums, errors)
├── PartsCom.Application/   # Application layer (CQRS commands/queries)
├── PartsCom.Infrastructure/# Infrastructure layer (EF Core, repositories, services)
└── PartsCom.Ui/           # Presentation layer (ASP.NET Core MVC)
```

## Code Style Guidelines

### Formatting (enforced by .editorconfig)

- **Indentation:** 4 spaces (no tabs)
- **Line endings:** CRLF
- **Final newline:** Required
- **Braces:** Always required (`csharp_prefer_braces = true:error`)
- **Namespaces:** File-scoped only (`namespace Foo;` not `namespace Foo { }`)
- **Using directives:** Outside namespace, System first, no blank line separators

### Type Preferences

- Use explicit types for built-in types: `string name = "foo";` not `var name = "foo";`
- Use `var` only when type is apparent: `var user = User.Create(...);`
- Use language keywords over BCL types: `string` not `String`, `int` not `Int32`
- No `this.` qualification unless necessary

### Naming Conventions

| Element | Convention | Example |
|---------|------------|---------|
| Classes/Records | PascalCase | `ProductRepository` |
| Interfaces | I + PascalCase | `IProductRepository` |
| Methods | PascalCase | `GetByIdAsync` |
| Properties | PascalCase | `FirstName` |
| Private fields | _camelCase | `_issuer` |
| Constants | PascalCase | `AccessTokenKey` |
| Async methods | Suffix with Async | `SaveChangesAsync` |

### Class Suffixes

- `*Command` / `*Query` - CQRS messages
- `*CommandHandler` / `*QueryHandler` - MediatR handlers
- `*CommandValidator` - FluentValidation validators
- `*Repository` - Repository implementations
- `*Service` - Service implementations
- `*Controller` - MVC controllers
- `*ViewModel` / `*Dto` - Data transfer objects

### Records vs Classes

- **Records:** Use for immutable DTOs, Commands, Queries, Responses
  ```csharp
  public sealed record RegisterUserCommand(string Email, string Password) : ICommand;
  ```
- **Classes:** Use for entities, services, handlers, stateful objects
  ```csharp
  public sealed class Product { ... }
  ```

### Access Modifiers

- Most classes should be `sealed`
- Infrastructure implementations should be `internal sealed`
- Public interfaces in Application layer, internal implementations in Infrastructure

## Architecture Patterns

### CQRS with MediatR

Commands and queries return `ErrorOr<T>` for typed error handling:

```csharp
// Command definition
public sealed record CreateProductCommand(string Name, decimal Price) : ICommand<Guid>;

// Handler
internal sealed class CreateProductCommandHandler(
    IProductRepository productRepository,
    IUnitOfWork unitOfWork) : ICommandHandler<CreateProductCommand, Guid>
{
    public async Task<ErrorOr<Guid>> Handle(CreateProductCommand request, CancellationToken ct)
    {
        var product = Product.Create(request.Name, request.Price);
        productRepository.Add(product);
        await unitOfWork.SaveChangesAsync(ct);
        return product.Id;
    }
}
```

### Error Handling with ErrorOr

Define errors in `PartsCom.Domain/Errors/` as static properties:

```csharp
public static partial class Errors
{
    public static Error ProductNotFound => Error.NotFound("PRD001", "Product not found.");
}
```

Handle errors in handlers:
```csharp
if (product == null)
    return Errors.ProductNotFound;  // Return error

return new ProductDto(product);     // Return success (implicit conversion)
```

### Domain Entities

- Private constructor with static `Create` factory method
- Private setters for encapsulation
- Rich domain methods for state changes

```csharp
public sealed class Product
{
    private Product() { }
    
    public static Product Create(string name, decimal price) => new()
    {
        Id = Guid.NewGuid(),
        Name = name,
        Price = price
    };
    
    public void UpdatePrice(decimal newPrice) => Price = newPrice;
}
```

### Repository Pattern

- Interface in Application layer
- Implementation in Infrastructure layer (internal sealed)
- Use Unit of Work for transactions

### FluentValidation

```csharp
internal sealed class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
{
    public CreateProductCommandValidator()
    {
        RuleFor(c => c.Name)
            .NotEmpty().WithErrorCode("CP001").WithMessage("Name is required")
            .MaximumLength(200).WithErrorCode("CP002").WithMessage("Name too long");
    }
}
```

### Primary Constructors

Use C# 12 primary constructors for dependency injection:

```csharp
internal sealed class ProductRepository(PartsComDbContext dbContext) : IProductRepository
{
    public void Add(Product product) => dbContext.Products.Add(product);
}
```

## Async/Await Guidelines

- Always propagate `CancellationToken` through async chains
- All async methods must end with `Async` suffix
- Do not use `ConfigureAwait(false)` (disabled via analyzer)

## Dependency Injection

Register services via extension methods in `Installer.cs`:

```csharp
public static IServiceCollection InstallApplication(this IServiceCollection services)
public static IServiceCollection InstallInfrastructure(this IServiceCollection services, IConfiguration config)
```

## Analyzer Configuration

- **TreatWarningsAsErrors:** Enabled globally
- **SonarAnalyzer.CSharp:** Included for static analysis
- **EnforceCodeStyleInBuild:** Enabled

See `.editorconfig` for full list of suppressed/configured analyzer rules.

## Key Disabled Analyzers

- CA1031: Catching general exceptions allowed
- CA2007: ConfigureAwait not required
- CS8618: Non-nullable warnings in EF entities suppressed
- IDE0290: Primary constructor suggestions disabled
