[![net80](https://img.shields.io/badge/net8.0-5C2D91?logo=.NET&labelColor=gray)](https://github.com/BoBoBaSs84/BB84.Extensions)
[![net100](https://img.shields.io/badge/net10.0-5C2D91?logo=.NET&labelColor=gray)](https://github.com/BoBoBaSs84/BB84.Extensions)
[![NuGet](https://img.shields.io/nuget/v/BB84.Extensions.svg?logo=nuget&logoColor=white)](https://www.nuget.org/packages/BB84.EntityFrameworkCore.Repositories)

# BB84.EntityFrameworkCore.Repositories

This package provides the default repository implementations and `DatabaseFacadeExtensions` for calling stored procedures and SQL functions.

## Installation

```powershell
dotnet add package BB84.EntityFrameworkCore.Repositories
```

## Repository implementations

### `GenericRepository<TEntity>`

Abstract base for all repositories. Accepts `IDbContext` as a constructor parameter. All query methods delegate to `PrepareQuery(...)`, which composes `Where`, `IgnoreQueryFilters`, `Include`, `OrderBy`, `Skip`, `Take`, and `AsNoTracking` into a single `IQueryable<TEntity>`. Projection overloads use the protected `ApplyProjection(query, selector, fieldSelector)` helper.

### `IdentityRepository<TEntity, TKey>` / `IdentityRepository<TEntity>`

Extends `GenericRepository<TEntity>` with key-based `GetById`, `GetByIds`, and bulk `Delete`/`Update` by ID. The non-generic overload defaults `TKey` to `Guid`.

### `EnumeratorRepository<TEntity, TKey>` / `EnumeratorRepository<TEntity>`

Extends `IdentityRepository` with `GetByName` and `GetByNames`. The non-generic overload defaults `TKey` to `int`.

## Usage

Inherit from the appropriate base class and inject `IDbContext`:

```csharp
public class ProductRepository : IdentityRepository<Product>, IProductRepository
{
    public ProductRepository(IDbContext dbContext) : base(dbContext) { }

    public async Task<IReadOnlyList<Product>> GetByCategoryAsync(
        int categoryId, CancellationToken token = default)
        => await GetManyByConditionAsync(
            p => p.CategoryId == categoryId,
            orderBy: q => q.OrderBy(p => p.Name),
            token: token);
}

public class CategoryRepository : EnumeratorRepository<ProductCategory>, ICategoryRepository
{
    public CategoryRepository(IDbContext dbContext) : base(dbContext) { }
    // GetByName / GetByNames inherited
}
```

DI registration:

```csharp
services.AddScoped<IProductRepository, ProductRepository>();
services.AddScoped<ICategoryRepository, CategoryRepository>();
```

Calling save changes is the responsibility of the caller (unit-of-work pattern):

```csharp
repository.Create(entity);
await dbContext.SaveChangesAsync(token);
```

## `DatabaseFacadeExtensions`

Extension methods on `DatabaseFacade` (`context.Database`) for calling SQL Server stored procedures and functions safely with parameterized SQL.

### Stored procedures

```csharp
// Returns rows as IReadOnlyList<T>
IReadOnlyList<ReportRow> rows = context.Database.ExecuteProcedure<ReportRow>(
    schema: "dbo",
    name: "usp_GetReport",
    parameters: [new SqlParameter("@FromDate", fromDate)]);

// Non-generic overload returns rows-affected count
int affected = context.Database.ExecuteProcedure(
    schema: "dbo",
    name: "usp_ArchiveOrders",
    parameters: [new SqlParameter("@CutoffDate", cutoff)]);

// Async variants
await context.Database.ExecuteProcedureAsync<ReportRow>(..., cancellationToken);
await context.Database.ExecuteProcedureAsync(..., cancellationToken);
```

Output parameters are supported — parameters with `Direction = ParameterDirection.Output` are emitted as `@param = @param OUTPUT` in the generated SQL.

### Table-valued functions

```csharp
IReadOnlyList<ProductDto> results = context.Database.ExecuteTableFunction<ProductDto>(
    schema: "dbo",
    name: "fn_GetActiveProducts",
    parameters: [new SqlParameter("@CategoryId", categoryId)]);

await context.Database.ExecuteTableFunctionAsync<ProductDto>(..., cancellationToken);
```

### Scalar-valued functions

```csharp
decimal? total = context.Database.ExecuteScalarFunction<decimal>(
    schema: "dbo",
    name: "fn_GetOrderTotal",
    parameters: [new SqlParameter("@OrderId", orderId)]);

await context.Database.ExecuteScalarFunctionAsync<decimal>(..., cancellationToken);
```

All methods sanitize schema/name inputs and use parameterized SQL to prevent injection.
