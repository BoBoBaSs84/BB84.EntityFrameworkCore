[![net80](https://img.shields.io/badge/net8.0-5C2D91?logo=.NET&labelColor=gray)](https://github.com/BoBoBaSs84/BB84.Extensions)
[![net100](https://img.shields.io/badge/net10.0-5C2D91?logo=.NET&labelColor=gray)](https://github.com/BoBoBaSs84/BB84.Extensions)
[![NuGet](https://img.shields.io/nuget/v/BB84.Extensions.svg?logo=nuget&logoColor=white)](https://www.nuget.org/packages/BB84.EntityFrameworkCore.Entities)

# BB84.EntityFrameworkCore.Entities

This package provides the default concrete implementations of the entity abstractions defined in `BB84.EntityFrameworkCore.Entities.Abstractions`.

## Installation

```powershell
dotnet add package BB84.EntityFrameworkCore.Entities
```

## Implementations

Each abstract class mirrors the interface hierarchy. Convenience overloads follow the same defaulting pattern as the interfaces (`TKey` → `Guid`, `TCreator`/`TEdited` → `string`/`string?`).

| Abstract class                               | Implements                                    | Notes                                     |
| -------------------------------------------- | --------------------------------------------- | ----------------------------------------- |
| `IdentityEntity<TKey>`                       | `IIdentityEntity<TKey>`                       |                                           |
| `IdentityEntity`                             | `IIdentityEntity`                             | `TKey` = `Guid`                           |
| `AuditedEntity<TKey, TCreator, TEdited>`     | `IAuditedEntity<TKey, TCreator, TEdited>`     |                                           |
| `AuditedEntity<TKey>`                        | `IAuditedEntity<TKey>`                        | `TCreator`/`TEdited` = `string`/`string?` |
| `AuditedEntity`                              | `IAuditedEntity`                              | `TKey` = `Guid`                           |
| `FullAuditedEntity<TKey, TCreator, TEdited>` | `IFullAuditedEntity<TKey, TCreator, TEdited>` | Adds `CreatedAt`, `EditedAt`              |
| `FullAuditedEntity<TKey>`                    | `IFullAuditedEntity<TKey>`                    |                                           |
| `FullAuditedEntity`                          | `IFullAuditedEntity`                          | `TKey` = `Guid`                           |
| `CompositeEntity`                            | `ICompositeEntity`                            |                                           |
| `AuditedCompositeEntity<TCreator, TEdited>`  | `IAuditedCompositeEntity<TCreator, TEdited>`  |                                           |
| `AuditedCompositeEntity`                     | `IAuditedCompositeEntity`                     | `TCreator`/`TEdited` = `string`/`string?` |
| `EnumeratorEntity<TKey>`                     | `IEnumeratorEntity<TKey>`                     |                                           |
| `EnumeratorEntity`                           | `IEnumeratorEntity`                           | `TKey` = `int`                            |

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

// Fully audited entity — tracks who and when, plus supports soft delete
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
    // Name and Description come from IEnumerator via EnumeratorEntity
}

// Custom key type
public class LedgerEntry : IdentityEntity<long>
{
    public decimal Amount { get; set; }
}
```

> **Note:** `FullAuditedEntity` adds time auditing (`CreatedAt`/`EditedAt`) to the identity and user-audit features. Soft delete (`IsDeleted`) is a separate concern — it is provided by `IEnumeratorEntity` (and thus `EnumeratorEntity`), not by `FullAuditedEntity`. Add `ISoftDeletable` manually to your entity if you need soft delete combined with full auditing.
