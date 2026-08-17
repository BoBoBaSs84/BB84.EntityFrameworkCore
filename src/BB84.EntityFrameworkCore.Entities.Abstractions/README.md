[![net100](https://img.shields.io/badge/net10.0-5C2D91?logo=.NET&labelColor=gray)](https://github.com/BoBoBaSs84/BB84.Extensions)
[![NuGet](https://img.shields.io/nuget/v/BB84.EntityFrameworkCore.Entities.Abstractions.svg?logo=nuget&logoColor=white)](https://www.nuget.org/packages/BB84.EntityFrameworkCore.Entities.Abstractions)

# BB84.EntityFrameworkCore.Entities.Abstractions

This package provides the core entity interface definitions. It has no external dependencies.

## Installation

```powershell
dotnet add package BB84.EntityFrameworkCore.Entities.Abstractions
```

## Component interfaces

These fine-grained interfaces are the building blocks composed by the entity-level interfaces below.

| Interface                         | Members                                                |
| --------------------------------- | ------------------------------------------------------ |
| `IIdentity<TKey>`                 | `TKey Id`                                              |
| `IConcurrency`                    | `byte[] Timestamp` (rowversion)                        |
| `ITimeAudited`                    | `DateTimeOffset CreatedAt`, `DateTimeOffset? EditedAt` |
| `IUserAudited<TCreator, TEditor>` | `TCreator CreatedBy`, `TEditor EditedBy`               |
| `IUserAudited`                    | Shorthand: `IUserAudited<string, string?>`             |
| `ISoftDeletable`                  | `bool IsDeleted`                                       |
| `IEnumeration`                    | `string Name`, `string? Description`                   |

## Entity interfaces

Each entity interface composes the component interfaces above. Convenience overloads default `TKey` to `Guid` and `TCreator`/`TEditor` to `string`/`string?`.

### `IIdentityEntity<TKey>` / `IIdentityEntity`

Inherits `IIdentity<TKey>` and `IConcurrency`. The base for all identity-keyed entities. The non-generic overload defaults `TKey` to `Guid`.

```csharp
public interface IIdentityEntity<TKey> : IIdentity<TKey>, IConcurrency
    where TKey : IEquatable<TKey>
```

### `IAuditedEntity<TKey, TCreator, TEditor>` and overloads

Extends `IIdentityEntity<TKey>` with `IUserAudited<TCreator, TEditor>`.

```csharp
// Full generic form
public interface IAuditedEntity<TKey, TCreator, TEditor> : IIdentityEntity<TKey>, IUserAudited<TCreator, TEditor>

// Defaults TCreator/TEditor to string/string?
public interface IAuditedEntity<TKey> : IAuditedEntity<TKey, string, string?>

// Defaults TKey to Guid, TCreator/TEditor to string/string?
public interface IAuditedEntity : IAuditedEntity<Guid, string, string?>
```

### `IFullAuditedEntity<TKey, TCreator, TEditor>` and overloads

Extends `IIdentityEntity<TKey>` with both `IUserAudited` and `ITimeAudited`.

```csharp
// Full generic form
public interface IFullAuditedEntity<TKey, TCreator, TEditor> : IIdentityEntity<TKey>, IUserAudited<TCreator, TEditor>, ITimeAudited

// Convenience overloads mirror the same pattern as IAuditedEntity
public interface IFullAuditedEntity<TKey> : IFullAuditedEntity<TKey, string, string?>
public interface IFullAuditedEntity : IFullAuditedEntity<Guid, string, string?>
```

### `ICompositeEntity`

For entities with composite primary keys — inherits only `IConcurrency`.

```csharp
public interface ICompositeEntity : IConcurrency
```

### `IAuditedCompositeEntity<TCreator, TEditor>` / `IAuditedCompositeEntity`

Extends `ICompositeEntity` with `IUserAudited`. The non-generic overload defaults to `string`/`string?`.

### `IEnumeratorEntity<TKey>` / `IEnumeratorEntity`

Lookup/reference data. Extends `IIdentityEntity<TKey>` with `IEnumeration` and `ISoftDeletable`. The non-generic overload defaults `TKey` to `int`.

```csharp
public interface IEnumeratorEntity<TKey> : IIdentityEntity<TKey>, IEnumeration, ISoftDeletable
    where TKey : IEquatable<TKey>
```

## Concurrency tokens

`IConcurrency.Timestamp` is settable, which is what makes the disconnected update work. An entity is read, mapped to a DTO, sent over the wire and posted back; assign the original token onto the reconstructed entity before updating it, so it reaches the `WHERE` predicate:

```csharp
OrderEntity entity = new()
{
    Id = dto.Id,
    TotalAmount = dto.TotalAmount,
    Timestamp = dto.Timestamp   // the value read earlier
};

repository.Update(entity);

try
{
    await context.SaveChangesAsync(cancellationToken);
}
catch (DbUpdateConcurrencyException)
{
    // another transaction changed the row in the meantime
}
```

Leave `Timestamp` unset and the update is not guarded — it succeeds regardless of what happened to the row in between. The value is store generated, so never assign it on a newly created entity, and never assign anything other than a value read from the database.

The token is shaped for SQL Server `rowversion`. Providers without an eight byte row version need their own mapping for the property.

> **Changed in 5.0:** `Timestamp` was get-only before, so the original value could not be restored on a detached entity and optimistic concurrency never fired for the case it exists for. Hand-written implementations of `IConcurrency` need a setter added. The shipped entities also start out with an empty array rather than `null`.

## Usage

Implement these interfaces directly on your domain entities, or use them as constraints in repository and service abstractions:

```csharp
// Custom entity interface
public interface IOrderEntity : IAuditedEntity
{
    decimal TotalAmount { get; set; }
}

// Custom repository interface constrained to the abstraction
public interface IOrderRepository : IIdentityRepository<IOrderEntity>
{
    Task<IReadOnlyList<IOrderEntity>> GetByCustomerAsync(Guid customerId, CancellationToken token = default);
}
```
