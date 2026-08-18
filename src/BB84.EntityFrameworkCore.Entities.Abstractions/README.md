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
| `IConcurrency<TToken>`            | `TToken Timestamp`                                     |
| `IConcurrency`                    | Shorthand: `IConcurrency<byte[]>` (rowversion)         |
| `ITimeAudited`                    | `DateTimeOffset CreatedAt`, `DateTimeOffset? EditedAt` |
| `IUserAudited<TCreator, TEditor>` | `TCreator CreatedBy`, `TEditor EditedBy`               |
| `IUserAudited`                    | Shorthand: `IUserAudited<string, string?>`             |
| `ISoftDeletable`                  | `bool IsDeleted`                                       |
| `IEnumeration`                    | `string Name`, `string? Description`                   |

## Entity interfaces

Each entity interface composes the component interfaces above. Convenience overloads default `TKey` to `Guid` and `TCreator`/`TEditor` to `string`/`string?`.

### `IIdentityEntity<TKey, TToken>` and overloads

Inherits `IIdentity<TKey>` and `IConcurrency<TToken>`. The base for all identity-keyed entities. The overloads default `TToken` to `byte[]` and `TKey` to `Guid`.

```csharp
public interface IIdentityEntity<TKey, TToken> : IIdentity<TKey>, IConcurrency<TToken>
    where TKey : IEquatable<TKey>
public interface IIdentityEntity<TKey> : IIdentityEntity<TKey, byte[]>
    where TKey : IEquatable<TKey>
public interface IIdentityEntity : IIdentityEntity<Guid>
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

### `ICompositeEntity<TToken>` / `ICompositeEntity`

For entities with composite primary keys — inherits only `IConcurrency<TToken>`.

```csharp
public interface ICompositeEntity<TToken> : IConcurrency<TToken>
public interface ICompositeEntity : ICompositeEntity<byte[]>
```

### `IAuditedCompositeEntity<TCreator, TEditor>` / `IAuditedCompositeEntity`

Extends `ICompositeEntity` with `IUserAudited`. The non-generic overload defaults to `string`/`string?`.

### `IEnumeratorEntity<TKey>` / `IEnumeratorEntity`

Lookup/reference data. Extends `IIdentityEntity<TKey>` with `IEnumeration` and `ISoftDeletable`. The non-generic overload defaults `TKey` to `int`.

```csharp
public interface IEnumeratorEntity<TKey, TToken> : IIdentityEntity<TKey, TToken>, IEnumeration, ISoftDeletable
    where TKey : IEquatable<TKey>
public interface IEnumeratorEntity<TKey> : IEnumeratorEntity<TKey, byte[]>
    where TKey : IEquatable<TKey>
public interface IEnumeratorEntity : IEnumeratorEntity<int>
```

## Concurrency tokens

`IConcurrency<TToken>.Timestamp` is settable, which is what makes the disconnected update work. An entity is read, mapped to a DTO, sent over the wire and posted back; assign the original token onto the reconstructed entity before updating it, so it reaches the `WHERE` predicate:

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

### Choosing the token type

`TToken` is the provider's business. `byte[]` is what the short rungs default to and what SQL Server `rowversion` needs; PostgreSQL `xmin` is a `uint`; a `Guid` or an incrementing `int` rotated by the application works too. Name the type on the widest rung of the ladder to pick something else:

```csharp
// rowversion, the default
public sealed class OrderEntity : IdentityEntity { }

// a Guid token instead
public sealed class OrderEntity : IdentityEntity<Guid, Guid> { }
```

Whatever the type, the store has to be told to generate it. The configuration base classes in `BB84.EntityFrameworkCore.Repositories` take a matching `TToken` and mark the property as a concurrency token generated on add or update — which is right for `rowversion` and for `xmin`, but not for a token the application rotates itself. Override the generation strategy after calling `base.Configure(builder)` in that case.

> **Changed in 5.0:** `Timestamp` was get-only before, so the original value could not be restored on a detached entity and optimistic concurrency never fired for the case it exists for. Hand-written implementations of `IConcurrency` need a setter added. The shipped entities also start out with an empty array rather than `null`.

> **Changed in 5.0:** the token type is a type parameter — `IConcurrency<TToken>`, with `IConcurrency` kept as the `byte[]` shorthand. Code naming `IConcurrency` is unaffected; the entity and configuration ladders each gained a rung that names the token type.

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
