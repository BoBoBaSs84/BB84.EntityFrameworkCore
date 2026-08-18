[![net100](https://img.shields.io/badge/net10.0-5C2D91?logo=.NET&labelColor=gray)](https://github.com/BoBoBaSs84/BB84.Extensions)
[![NuGet](https://img.shields.io/nuget/v/BB84.EntityFrameworkCore.Entities.svg?logo=nuget&logoColor=white)](https://www.nuget.org/packages/BB84.EntityFrameworkCore.Entities)

# BB84.EntityFrameworkCore.Entities

This package provides the default concrete implementations of the entity abstractions defined in `BB84.EntityFrameworkCore.Entities.Abstractions`.

## Installation

```powershell
dotnet add package BB84.EntityFrameworkCore.Entities
```

## Implementations

Each abstract class mirrors the interface hierarchy. Convenience overloads follow the same defaulting pattern as the interfaces (`TToken` → `byte[]`, `TKey` → `Guid`, `TCreator`/`TEditor` → `string`/`string?`).

| Abstract class                                       | Implements                                            | Notes                                     |
| ---------------------------------------------------- | ----------------------------------------------------- | ----------------------------------------- |
| `IdentityEntity<TKey, TToken>`                       | `IIdentityEntity<TKey, TToken>`                       |                                           |
| `IdentityEntity<TKey>`                               | `IIdentityEntity<TKey>`                               | `TToken` = `byte[]`                       |
| `IdentityEntity`                                     | `IIdentityEntity`                                     | `TKey` = `Guid`                           |
| `AuditedEntity<TKey, TCreator, TEditor, TToken>`     | `IAuditedEntity<TKey, TCreator, TEditor, TToken>`     |                                           |
| `AuditedEntity<TKey, TCreator, TEditor>`             | `IAuditedEntity<TKey, TCreator, TEditor>`             | `TToken` = `byte[]`                       |
| `AuditedEntity<TKey>`                                | `IAuditedEntity<TKey>`                                | `TCreator`/`TEditor` = `string`/`string?` |
| `AuditedEntity`                                      | `IAuditedEntity`                                      | `TKey` = `Guid`                           |
| `FullAuditedEntity<TKey, TCreator, TEditor, TToken>` | `IFullAuditedEntity<TKey, TCreator, TEditor, TToken>` | Adds `CreatedAt`, `EditedAt`              |
| `FullAuditedEntity<TKey, TCreator, TEditor>`         | `IFullAuditedEntity<TKey, TCreator, TEditor>`         | `TToken` = `byte[]`                       |
| `FullAuditedEntity<TKey>`                            | `IFullAuditedEntity<TKey>`                            |                                           |
| `FullAuditedEntity`                                  | `IFullAuditedEntity`                                  | `TKey` = `Guid`                           |
| `CompositeEntity<TToken>`                            | `ICompositeEntity<TToken>`                            |                                           |
| `CompositeEntity`                                    | `ICompositeEntity`                                    | `TToken` = `byte[]`                       |
| `AuditedCompositeEntity<TCreator, TEditor, TToken>`  | `IAuditedCompositeEntity<TCreator, TEditor, TToken>`  |                                           |
| `AuditedCompositeEntity<TCreator, TEditor>`          | `IAuditedCompositeEntity<TCreator, TEditor>`          | `TToken` = `byte[]`                       |
| `AuditedCompositeEntity`                             | `IAuditedCompositeEntity`                             | `TCreator`/`TEditor` = `string`/`string?` |
| `EnumeratorEntity<TKey, TToken>`                     | `IEnumeratorEntity<TKey, TToken>`                     |                                           |
| `EnumeratorEntity<TKey>`                             | `IEnumeratorEntity<TKey>`                             | `TToken` = `byte[]`                       |
| `EnumeratorEntity`                                   | `IEnumeratorEntity`                                   | `TKey` = `int`                            |

## Usage

Inherit from the appropriate base class and add your domain properties:

```csharp
// Simple entity identified by Guid with concurrency token
public class Product : IdentityEntity
{
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
}

// Audited entity — tracks who created/edited, but not when
public class Order : AuditedEntity
{
    public decimal TotalAmount { get; set; }
}

// Fully audited entity — tracks who and when
public class Invoice : FullAuditedEntity
{
    public string Number { get; set; } = string.Empty;
    public DateOnly IssueDate { get; set; }
}

// Composite-key entity
public class OrderItem : CompositeEntity
{
    public Guid OrderId { get; set; }
    public Guid ProductId { get; set; }
    public int Quantity { get; set; }
}

// Lookup table (int PK, Name unique, soft-deletable)
public class ProductCategory : EnumeratorEntity
{
    // Name and Description come from IEnumeration via EnumeratorEntity
}

// Custom key type
public class LedgerEntry : IdentityEntity<long>
{
    public decimal Amount { get; set; }
}
```

> **Note:** `FullAuditedEntity` adds time auditing (`CreatedAt`/`EditedAt`) to the identity and user-audit features. Soft delete (`IsDeleted`) is a separate concern — it is provided by `IEnumeratorEntity` (and thus `EnumeratorEntity`), not by `FullAuditedEntity`. Add `ISoftDeletable` manually to your entity if you need soft delete combined with full auditing.

> **Changed in 5.0:** `Timestamp` is now settable and starts out as an empty array rather than `null`. Assign the previously read value onto a reconstructed entity before updating it, or the update is not guarded by the concurrency token — see the [abstractions README](../BB84.EntityFrameworkCore.Entities.Abstractions/README.md#concurrency-tokens).
