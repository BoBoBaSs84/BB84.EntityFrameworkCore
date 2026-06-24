[![net80](https://img.shields.io/badge/net8.0-5C2D91?logo=.NET&labelColor=gray)](https://github.com/BoBoBaSs84/BB84.Extensions)
[![net100](https://img.shields.io/badge/net10.0-5C2D91?logo=.NET&labelColor=gray)](https://github.com/BoBoBaSs84/BB84.Extensions)
[![NuGet](https://img.shields.io/nuget/v/BB84.Extensions.svg?logo=nuget&logoColor=white)](https://www.nuget.org/packages/BB84.EntityFrameworkCore.Repositories.Abstractions)

# BB84.EntityFrameworkCore.Repositories.Abstractions

This package provides the repository interface definitions and the `IDbContext` abstraction.

## Installation

```powershell
dotnet add package BB84.EntityFrameworkCore.Repositories.Abstractions
```

## `IDbContext`

A thin abstraction over `DbContext` that exposes only what repositories need: `Set<TEntity>()`, `SaveChanges`, `SaveChangesAsync`, `ChangeTracker`, `Database`, and the save-events. Your application `DbContext` should implement this interface so repositories remain decoupled from the concrete EF context type.

```csharp
public class AppDbContext : DbContext, IDbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    // DbSet properties ...
}

// DI registration
services.AddDbContext<AppDbContext>(...);
services.AddScoped<IDbContext>(sp => sp.GetRequiredService<AppDbContext>());
```

## `IGenericRepository<TEntity>`

The base repository interface. All methods have synchronous and asynchronous variants.

**Create**

- `Create(entity)` / `Create(entities)` — marks entity/entities as `Added`
- `CreateAsync(entity)` / `CreateAsync(entities)`

**Read**

- `GetAll(ignoreQueryFilters, trackChanges)` — returns all entities
- `GetAll<TResult>(selector, fieldSelector, ignoreQueryFilters)` — returns projected DTO list
- `GetByCondition(expression, queryFilter, ignoreQueryFilters, trackChanges, includeProperties)` — returns single or null
- `GetByCondition<TResult>(expression, selector, fieldSelector, ...)` — projected single
- `GetManyByCondition(expression, queryFilter, ignoreQueryFilters, orderBy, skip, take, trackChanges, includeProperties)` — paged/filtered list
- `GetManyByCondition<TResult>(...)` — projected paged/filtered list
- `CountAll(ignoreQueryFilters)` / `CountByCondition(expression, ...)` — count queries

**Update**

- `Update(entity)` / `Update(entities)` — marks entity/entities as `Modified`
- `Update(expression, setPropertyCalls)` — bulk `ExecuteUpdate` (bypasses change tracker)

**Delete**

- `Delete(entity)` / `Delete(entities)` — marks entity/entities as `Deleted`
- `Delete(expression)` — bulk `ExecuteDelete` (bypasses change tracker)

All `Async` variants accept an optional `CancellationToken`.

## `IIdentityRepository<TEntity, TKey>` / `IIdentityRepository<TEntity>`

Extends `IGenericRepository<TEntity>` with ID-based operations. The non-generic overload defaults `TKey` to `Guid`.

**Additional read methods**

- `GetById(id, ignoreQueryFilters, trackChanges, includeProperties)`
- `GetById<TResult>(id, selector, fieldSelector, ignoreQueryFilters)` — projected
- `GetByIds(ids, ...)` / `GetByIdsAsync(ids, ...)`

**Additional delete methods**

- `Delete(id)` / `Delete(ids)` — bulk `ExecuteDelete` by key(s)

**Additional update methods**

- `Update(id, setPropertyCalls)` / `Update(ids, setPropertyCalls)` — bulk `ExecuteUpdate` by key(s)

## `IEnumeratorRepository<TEntity, TKey>` / `IEnumeratorRepository<TEntity>`

Extends `IIdentityRepository` with name-based lookups. The non-generic overload defaults `TKey` to `int`.

- `GetByName(name, ignoreQueryFilters, trackChanges)`
- `GetByNames(names, ignoreQueryFilters, trackChanges)`
- Async variants of both

## Usage

Define custom repository interfaces against these abstractions:

```csharp
public interface IProductRepository : IIdentityRepository<Product>
{
    Task<IReadOnlyList<Product>> GetByCategoryAsync(int categoryId, CancellationToken token = default);
}

public interface ICategoryRepository : IEnumeratorRepository<ProductCategory>
{
    // GetByName / GetByNames already provided
}
```
