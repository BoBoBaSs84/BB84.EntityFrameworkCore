[![net100](https://img.shields.io/badge/net10.0-5C2D91?logo=.NET&labelColor=gray)](https://github.com/BoBoBaSs84/BB84.Extensions)
[![NuGet](https://img.shields.io/nuget/v/BB84.EntityFrameworkCore.Repositories.svg?logo=nuget&logoColor=white)](https://www.nuget.org/packages/BB84.EntityFrameworkCore.Repositories)

# BB84.EntityFrameworkCore.Repositories

This package provides the default repository implementations, the provider-agnostic entity type configuration base classes, and the save changes interceptors for auditing and soft delete.

> **Moved in 5.0.** The configuration base classes and the interceptors came here from
> `BB84.EntityFrameworkCore.Repositories.SqlServer`, where almost nothing about them was SQL Server specific.
> `DatabaseFacadeExtensions` went the other way, into that package, because it emits T-SQL.

## Installation

```powershell
dotnet add package BB84.EntityFrameworkCore.Repositories
```

## Repository implementations

### `GenericRepository<TEntity>`

Abstract base for all repositories. Accepts `IDbContext` as a constructor parameter. Every read method takes a single `Query<TEntity>` and delegates to the protected `PrepareQuery(query, forCount)`, which composes `Where`, `QueryFilter`, `IgnoreQueryFilters`, `Include`, `OrderBy`, `Skip`, `Take` and `AsNoTracking` into one `IQueryable<TEntity>`. Projections go through the protected `ApplyProjection(query, selector)`.

Two protected helpers are worth knowing when writing a repository method the interface does not cover:

- `PrepareQuery(query, forCount)` — the single place a `Query<TEntity>` becomes a query. Pass `forCount: true` to skip the options that cannot change how many rows match.
- `WithCondition(query, condition)` — appends a condition to a query without discarding what the caller already set. This is how `GetById` and `GetByName` add their key or name filter.

#### Streaming reads

`Stream` returns `IAsyncEnumerable<T>` instead of `IReadOnlyList<T>`. It takes the same `Query<TEntity>` as `GetListAsync` and goes through the same composition, but the result set is not buffered — rows are yielded as they arrive. Use it for exports, batch jobs and other unbounded reads:

```csharp
await foreach (Product product in repository.Stream(
    new()
    {
        Where = p => p.IsActive,
        OrderBy = q => q.OrderBy(p => p.Name),
    },
    cancellationToken))
{
    await writer.WriteAsync(product, cancellationToken);
}
```

Two things to keep in mind:

- The sequence is lazy. It must be enumerated within the lifetime of the `IDbContext`, and EF Core allows only one active stream per context at a time.
- `TrackChanges` defaults to `false` and should stay that way for large reads — with tracking enabled the change tracker grows with every yielded entity, which defeats the purpose of streaming.

### `IdentityRepository<TEntity, TKey>` / `IdentityRepository<TEntity>`

Extends `GenericRepository<TEntity>` with key-based `GetById`, `GetByIds`, and immediate bulk `ExecuteDelete`/`ExecuteUpdate` by ID. The non-generic overload defaults `TKey` to `Guid`.

### `EnumeratorRepository<TEntity, TKey>` / `EnumeratorRepository<TEntity>`

Extends `IdentityRepository` with `GetByName` and `GetByNames`. The non-generic overload defaults `TKey` to `int`.

## Usage

Inherit from the appropriate base class and inject `IDbContext`:

```csharp
public class ProductRepository : IdentityRepository<Product>, IProductRepository
{
    public ProductRepository(IDbContext dbContext) : base(dbContext) { }

    public async Task<IReadOnlyList<Product>> GetByCategoryAsync(
        int categoryId, CancellationToken cancellationToken = default)
        => await GetListAsync(
            new()
            {
                Where = p => p.CategoryId == categoryId,
                OrderBy = q => q.OrderBy(p => p.Name),
            },
            cancellationToken);
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
await dbContext.SaveChangesAsync(cancellationToken);
```

## Configuration base classes

Provider-agnostic `IEntityTypeConfiguration<TEntity>` base classes in `BB84.EntityFrameworkCore.Repositories.Configurations`. They apply key declaration, column ordering, the concurrency token, audit columns and — for enumerator entities — the name/description constraints, unique index and soft delete query filter. Everything they do is EF Core or EF Core Relational, so they work on PostgreSQL, SQLite, MySQL and Oracle.

| Configuration class                                          | For entity type                               |
| ------------------------------------------------------------ | --------------------------------------------- |
| `IdentityConfiguration<TEntity, TKey>`                       | `IIdentityEntity<TKey>`                       |
| `AuditedConfiguration<TEntity, TKey, TCreator, TEdited>`     | `IAuditedEntity<TKey, TCreator, TEdited>`     |
| `FullAuditedConfiguration<TEntity, TKey, TCreator, TEdited>` | `IFullAuditedEntity<TKey, TCreator, TEdited>` |
| `CompositeConfiguration<TEntity>`                            | `ICompositeEntity`                            |
| `AuditedCompositeConfiguration<TEntity, TCreator, TEdited>`  | `IAuditedCompositeEntity<TCreator, TEdited>`  |
| `EnumeratorConfiguration<TEntity, TKey>`                     | `IEnumeratorEntity<TKey>`                     |

Each ladder has narrower generic aliases that default the key to `Guid` (`int` for the enumerator) and the creator/editor to `string`.

On SQL Server, use the same type names from `BB84.EntityFrameworkCore.Repositories.SqlServer.Configurations` instead — those derive from these and add clustering, `NEWID()` defaults and `sysname` audit columns.

## Interceptors

Register on the `DbContextOptionsBuilder`:

```csharp
services.AddSingleton<ICurrentUserProvider, EnvironmentUserProvider>();
services.AddSingleton(TimeProvider.System);

services.AddSingleton<SoftDeletableInterceptor>();
services.AddSingleton<TimeAuditedInterceptor>();
services.AddSingleton<UserAuditedInterceptor>();

services.AddDbContext<AppDbContext>((sp, options) =>
{
    options
        .UseSqlServer(connectionString) // any provider
        .AddInterceptors(
            sp.GetRequiredService<SoftDeletableInterceptor>(),
            sp.GetRequiredService<TimeAuditedInterceptor>(),
            sp.GetRequiredService<UserAuditedInterceptor>());
});
```

### `SoftDeletableInterceptor`

Fires on `SavingChanges`/`SavingChangesAsync`. For every entity tracked as `Deleted` that implements `ISoftDeletable`, it sets `IsDeleted = true` and changes the state to `Modified` — preventing a physical `DELETE` from being issued.

Pair it with a configuration deriving from `EnumeratorConfiguration<TEntity, TKey>`, which applies the matching `HasQueryFilter(e => !e.IsDeleted)`. Without that filter the flag is set but never read.

### `TimeAuditedInterceptor`

Fires on `SavingChanges`/`SavingChangesAsync`. For every entity implementing `ITimeAudited`:

- `EntityState.Added` → sets `CreatedAt`
- `EntityState.Modified` → sets `EditedAt`

The clock comes from an optional `TimeProvider` constructor parameter, defaulting to `TimeProvider.System`. It is read **once per save**, so every entity in one save shares a timestamp. Supply a fake provider to freeze time:

```csharp
FakeTimeProvider timeProvider = new(new DateTimeOffset(2026, 1, 1, 0, 0, 0, TimeSpan.Zero));
TimeAuditedInterceptor interceptor = new(timeProvider);
```

### `UserAuditedInterceptor`

Fires on `SavingChanges`/`SavingChangesAsync`. For every entity implementing `IUserAudited`:

- `EntityState.Added` → sets `CreatedBy`
- `EntityState.Modified` → sets `EditedBy`

The identity comes from `ICurrentUserProvider` (in `BB84.EntityFrameworkCore.Repositories.Abstractions`), asked once per save. `EnvironmentUserProvider` ships as the default and reports `MachineName\UserName`, which suits desktop applications and services. A web application implements the interface over the current request instead:

```csharp
public sealed class HttpCurrentUserProvider(IHttpContextAccessor accessor) : ICurrentUserProvider
{
    public string GetCurrentUser()
        => accessor.HttpContext?.User.Identity?.Name ?? "anonymous";
}
```

For audit columns that are not `string`, use the generic `UserAuditedInterceptor<TUser>` with a matching `ICurrentUserProvider<TUser>`.

All three interceptors run on save. The `ExecuteDelete` / `ExecuteUpdate` repository methods execute immediately and bypass the change tracker, so none of them fire for those.
