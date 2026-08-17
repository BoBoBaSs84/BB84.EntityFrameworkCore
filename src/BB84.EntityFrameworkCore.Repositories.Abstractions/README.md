[![net100](https://img.shields.io/badge/net10.0-5C2D91?logo=.NET&labelColor=gray)](https://github.com/BoBoBaSs84/BB84.Extensions)
[![NuGet](https://img.shields.io/nuget/v/BB84.EntityFrameworkCore.Repositories.Abstractions.svg?logo=nuget&logoColor=white)](https://www.nuget.org/packages/BB84.EntityFrameworkCore.Repositories.Abstractions)

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

## `Query<TEntity>`

Every read method takes its options as one immutable record instead of a positional parameter list. All options are optional, so passing nothing reads everything.

| Property             | Type                                                     | Default |
| -------------------- | -------------------------------------------------------- | ------- |
| `Where`              | `Expression<Func<TEntity, bool>>?`                       | `null`  |
| `QueryFilter`        | `Func<IQueryable<TEntity>, IQueryable<TEntity>>?`        | `null`  |
| `Include`            | `IReadOnlyList<Expression<Func<TEntity, object?>>>?`     | `null`  |
| `OrderBy`            | `Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>?` | `null`  |
| `Skip` / `Take`      | `int?`                                                   | `null`  |
| `TrackChanges`       | `bool`                                                   | `false` |
| `IgnoreQueryFilters` | `bool`                                                   | `false` |

Options apply in a fixed order: `Where`, `QueryFilter`, `IgnoreQueryFilters`, `Include`, `OrderBy`, `Skip`, `Take` — so ordering always precedes paging.

```csharp
IReadOnlyList<Person> people = await repository.GetListAsync(
    new()
    {
        Where = x => x.IsActive,
        OrderBy = q => q.OrderBy(x => x.LastName),
        Include = [x => x.Jobs],
        Take = 50,
    },
    cancellationToken);
```

Being a record, a base query can be varied with `with` instead of rebuilt:

```csharp
Query<Person> active = new() { Where = x => x.IsActive };

int total = repository.Count(active);
IReadOnlyList<Person> firstPage = repository.GetList(active with { Take = 25 });
```

`Include` names navigations as expressions, so a rename is a compile error rather than a runtime one. Use `QueryFilter` for anything the properties cannot express — a nested `ThenInclude`, a join, a grouping.

## Read / write split

Each repository abstraction exists as two halves plus a composed alias:

| Read                                       | Write                                       | Composed                                |
| ------------------------------------------- | -------------------------------------------- | ---------------------------------------- |
| `IReadRepository<TEntity>`                 | `IWriteRepository<TEntity>`                 | `IGenericRepository<TEntity>`           |
| `IReadIdentityRepository<TEntity, TKey>`   | `IWriteIdentityRepository<TEntity, TKey>`   | `IIdentityRepository<TEntity, TKey>`    |
| `IReadEnumeratorRepository<TEntity, TKey>` | —                                           | `IEnumeratorRepository<TEntity, TKey>`  |

Depend on a half wherever a component only reads or only writes — a query handler, a reporting service, an importer, either side of a CQRS split. The dependency then states in its type what it is allowed to do, and a hand written test double only has to implement the half it needs.

```csharp
public sealed class PriceReport(IReadRepository<Product> products)
{
    // Cannot delete anything. Not by convention — there is no such member.
    public Task<IReadOnlyList<Product>> GetActiveAsync(CancellationToken cancellationToken)
        => products.GetListAsync(new() { Where = p => p.IsActive }, cancellationToken);
}
```

The composed interfaces add no members of their own, so nothing is reachable only through them. `GenericRepository<TEntity>` and its derivatives still implement the composed interfaces, and existing `IGenericRepository<TEntity>` consumers are unaffected.

There is no `IWriteEnumeratorRepository`: an enumerator repository adds only name based *reads* on top of the identity repository, so its write half is exactly the inherited one.

## `IGenericRepository<TEntity>`

The base repository interface, composing `IReadRepository<TEntity>` and `IWriteRepository<TEntity>`. All methods have synchronous and asynchronous variants, and every `Async` variant takes `CancellationToken cancellationToken` last.

**Create** — declared on `IWriteRepository<TEntity>`

- `Create(entity)` / `Create(entities)` — marks entity/entities as `Added`
- `CreateAsync(entity)` / `CreateAsync(entities)`

**Read** — declared on `IReadRepository<TEntity>`

- `Count(query)` — number of matching rows
- `GetSingle(query)` — one entity or `null`; throws when more than one matches
- `GetSingle<TResult>(selector, query)` — projected single
- `GetList(query)` — matching entities, buffered
- `GetList<TResult>(selector, query)` — projected list
- `Stream(query, cancellationToken)` — matching entities as `IAsyncEnumerable<TEntity>`, not buffered
- `Stream<TResult>(selector, query, cancellationToken)` — projected stream

There is no separate "all" method: an absent `Where` already means everything, so `GetList()` reads the whole set.

`Count` deliberately ignores `OrderBy`, `Skip`, `Take` and `Include`, because none of them changes how many rows match.

Streaming yields rows as they arrive. The sequence is lazy, so it has to be enumerated within the lifetime of the `IDbContext`, only one stream may be live per context at a time, and `TrackChanges` should stay off or the change tracker grows with every entity yielded.

**Update** — declared on `IWriteRepository<TEntity>`

- `Update(entity)` / `Update(entities)` — marks entity/entities as `Modified`
- `ExecuteUpdate(expression, setPropertyCalls)` — immediate bulk `UPDATE` (bypasses change tracker)

**Delete** — declared on `IWriteRepository<TEntity>`

- `Delete(entity)` / `Delete(entities)` — marks entity/entities as `Deleted`
- `ExecuteDelete(expression)` — immediate bulk `DELETE` (bypasses change tracker)

The `Execute` prefix marks the immediate operations, matching EF Core's own `IQueryable.ExecuteDelete()` / `ExecuteUpdate()`. They run against the database there and then, so no save changes interceptor fires for them — auditing is skipped, and `ExecuteDelete` deletes permanently even for soft deletable entities.

## `IIdentityRepository<TEntity, TKey>` / `IIdentityRepository<TEntity>`

Extends `IGenericRepository<TEntity>` with ID-based operations. The non-generic overload defaults `TKey` to `Guid`.

**Additional read methods** — declared on `IReadIdentityRepository<TEntity, TKey>`

- `GetById(id, query)` / `GetById<TResult>(id, selector, query)`
- `GetByIds(ids, query)` / `GetByIds<TResult>(ids, selector, query)`
- Async variants of all four

The key condition is **added** to the query rather than replacing it, so a `Where` in the query keeps applying alongside it.

**Additional delete methods** — declared on `IWriteIdentityRepository<TEntity, TKey>`

- `ExecuteDelete(id)` / `ExecuteDelete(ids)` — immediate bulk `DELETE` by key(s)

**Additional update methods** — declared on `IWriteIdentityRepository<TEntity, TKey>`

- `ExecuteUpdate(id, setPropertyCalls)` / `ExecuteUpdate(ids, setPropertyCalls)` — immediate bulk `UPDATE` by key(s)

Every method on `IWriteIdentityRepository<TEntity, TKey>` is of the immediate kind — there is no key-based deferred operation.

## `IEnumeratorRepository<TEntity, TKey>` / `IEnumeratorRepository<TEntity>`

Extends `IIdentityRepository` with name-based lookups, declared on `IReadEnumeratorRepository<TEntity, TKey>`. The non-generic overload defaults `TKey` to `int`.

- `GetByName(name, query)` / `GetByNames(names, query)`
- Async variants of both

## Migrating from 4.x

| 4.x                                                         | 5.0                                                             |
| ----------------------------------------------------------- | --------------------------------------------------------------- |
| `GetAll(ignoreQueryFilters, trackChanges)`                  | `GetList(new() { IgnoreQueryFilters = …, TrackChanges = … })`   |
| `GetAll<TResult>(selector, fieldSelector, …)`               | `GetList(selector, query)`                                      |
| `GetByCondition(expression, …)`                             | `GetSingle(new() { Where = … })`                                |
| `GetByCondition(queryFilter, …)`                            | `GetSingle(new() { QueryFilter = … })`                          |
| `GetByCondition<TResult>(expression, selector, …)`          | `GetSingle(selector, new() { Where = … })`                      |
| `GetManyByCondition(expression, …, orderBy, skip, take, …)` | `GetList(new() { Where = …, OrderBy = …, Skip = …, Take = … })` |
| `GetManyByCondition<TResult>(…)`                            | `GetList(selector, query)`                                      |
| `CountAll(ignoreQueryFilters)`                              | `Count(new() { IgnoreQueryFilters = … })`                       |
| `CountByCondition(expression, …)`                           | `Count(new() { Where = … })`                                    |
| `StreamAll(…)`                                              | `Stream(query, cancellationToken)`                              |
| `StreamByCondition(expression, …)`                          | `Stream(new() { Where = … }, cancellationToken)`                |
| `StreamByCondition<TResult>(…)`                             | `Stream(selector, query, cancellationToken)`                    |
| `GetById(id, ignoreQueryFilters, trackChanges, includes)`   | `GetById(id, query)`                                            |
| `GetByName(name, ignoreQueryFilters, trackChanges)`         | `GetByName(name, query)`                                        |
| `includeProperties: [nameof(X.Nav)]`                        | `Include = [x => x.Nav]`                                        |
| `fieldSelector: …`                                          | removed — compose it into `selector`                            |
| `token:`                                                    | `cancellationToken:`                                            |
| `Delete(expression)`                                        | `ExecuteDelete(expression)`                                     |
| `DeleteAsync(expression, …)`                                | `ExecuteDeleteAsync(expression, …)`                             |
| `Delete(id)` / `Delete(ids)`                                | `ExecuteDelete(id)` / `ExecuteDelete(ids)`                      |
| `DeleteAsync(id, …)` / `DeleteAsync(ids, …)`                | `ExecuteDeleteAsync(id, …)` / `ExecuteDeleteAsync(ids, …)`      |
| `Update(expression, setPropertyCalls)`                      | `ExecuteUpdate(expression, setPropertyCalls)`                   |
| `UpdateAsync(expression, setPropertyCalls, …)`              | `ExecuteUpdateAsync(expression, setPropertyCalls, …)`           |
| `Update(id, …)` / `Update(ids, …)`                          | `ExecuteUpdate(id, …)` / `ExecuteUpdate(ids, …)`                |
| `UpdateAsync(id, …)` / `UpdateAsync(ids, …)`                | `ExecuteUpdateAsync(id, …)` / `ExecuteUpdateAsync(ids, …)`      |

Every `Async` method now takes its cancellation token **last**. Call sites that passed arguments positionally around the old token position need checking, not just recompiling.

The `Execute` renames cover the immediate operations only; the entity based `Delete(entity)`, `Delete(entities)`, `Update(entity)` and `Update(entities)` keep their names. Both sets used to share a name while differing in whether they defer to a save operation and whether interceptors run — the rename makes that difference visible at the call site. Each rename is mechanical, but it breaks every call site of the immediate overloads; there are no `[Obsolete]` forwarders.

## `ICurrentUserProvider<TUser>` / `ICurrentUserProvider`

Supplies the identity that `UserAuditedInterceptor` records in the `CreatedBy` / `EditedBy` columns. One member:

- `GetCurrentUser()`

The identity source is application specific, so the library asks for it rather than deciding it. `EnvironmentUserProvider` in `BB84.EntityFrameworkCore.Repositories` covers the process-user case; implement the interface yourself to resolve the identity from an HTTP request, a job context, or anywhere else. The non-generic alias fixes `TUser` to `string`, matching the default audit columns.

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
