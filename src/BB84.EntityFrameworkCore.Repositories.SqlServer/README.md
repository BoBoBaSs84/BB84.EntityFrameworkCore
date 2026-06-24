[![net80](https://img.shields.io/badge/net8.0-5C2D91?logo=.NET&labelColor=gray)](https://github.com/BoBoBaSs84/BB84.Extensions)
[![net100](https://img.shields.io/badge/net10.0-5C2D91?logo=.NET&labelColor=gray)](https://github.com/BoBoBaSs84/BB84.Extensions)
[![NuGet](https://img.shields.io/nuget/v/BB84.EntityFrameworkCore.Repositories.SqlServer.svg?logo=nuget&logoColor=white)](https://www.nuget.org/packages/BB84.EntityFrameworkCore.Repositories.SqlServer)

# BB84.EntityFrameworkCore.Repositories.SqlServer

This package provides SQL Server–specific entity type configuration base classes, `SaveChangesInterceptor` implementations for auditing and soft delete, and `EntityTypeBuilderExtensions` for temporal table support.

## Installation

```powershell
dotnet add package BB84.EntityFrameworkCore.Repositories.SqlServer
```

## Configuration base classes

Inherit from these in your `IEntityTypeConfiguration<TEntity>` implementations and call `base.Configure(builder)` to apply the standard column order, constraints, concurrency tokens, and indexes. Override before or after the base call to add entity-specific configuration.

| Configuration class                                          | For entity type                               | What `base.Configure` applies                                                                                                                                        |
| ------------------------------------------------------------ | --------------------------------------------- | -------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| `IdentityConfiguration<TEntity, TKey>`                       | `IIdentityEntity<TKey>`                       | Non-clustered PK, `Id` (`ValueGeneratedOnAdd`), `Timestamp` as concurrency token                                                                                     |
| `AuditedConfiguration<TEntity, TKey, TCreator, TEdited>`     | `IAuditedEntity<TKey, TCreator, TEdited>`     | Above + `CreatedBy` (required), `EditedBy` (optional); `string` overload maps both as `sysname`                                                                      |
| `FullAuditedConfiguration<TEntity, TKey, TCreator, TEdited>` | `IFullAuditedEntity<TKey, TCreator, TEdited>` | Above + `CreatedAt` (required), `EditedAt` (optional); `Guid` overload adds `NEWID()` default                                                                        |
| `CompositeConfiguration<TEntity>`                            | `ICompositeEntity`                            | `Timestamp` as concurrency token                                                                                                                                     |
| `AuditedCompositeConfiguration<TEntity, TCreator, TEdited>`  | `IAuditedCompositeEntity<TCreator, TEdited>`  | Above + audit user columns                                                                                                                                           |
| `EnumeratorConfiguration<TEntity, TKey>`                     | `IEnumeratorEntity<TKey>`                     | Non-clustered PK, `Name` (`nvarchar(64)`, unique index, non-unicode), `Description` (`nvarchar(256)`), `IsDeleted` default `false`; `int` overload uses clustered PK |

```csharp
public class ProductConfiguration : IdentityConfiguration<Product>
{
    public override void Configure(EntityTypeBuilder<Product> builder)
    {
        base.Configure(builder);

        builder.ToTable("Products", "catalog");
        builder.Property(p => p.Name).IsRequired().HasMaxLength(200);
        builder.Property(p => p.Price).IsDecimalColumn(10, 2);
    }
}

public class InvoiceConfiguration : FullAuditedConfiguration<Invoice>
{
    public override void Configure(EntityTypeBuilder<Invoice> builder)
    {
        base.Configure(builder); // applies NEWID() default, sysname for audit columns

        builder.ToTable("Invoices", "billing");
        builder.Property(i => i.Number).IsRequired().HasMaxLength(50);
    }
}

public class ProductCategoryConfiguration : EnumeratorConfiguration<ProductCategory>
{
    public override void Configure(EntityTypeBuilder<ProductCategory> builder)
    {
        base.Configure(builder); // clustered int PK, Name unique index, IsDeleted default false
        builder.ToTable("ProductCategories", "catalog");
    }
}
```

Apply configurations via `ApplyConfigurationsFromAssembly` in `OnModelCreating`:

```csharp
protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
}
```

## `PropertyBuilderExtensions`

Extension methods on `PropertyBuilder` that set the SQL Server column type in one call. All methods return the same `PropertyBuilder` for chaining.

| Method                              | SQL Server type                   | Parameters                                                       |
| ----------------------------------- | --------------------------------- | ---------------------------------------------------------------- |
| `IsBinaryColumn(precision)`         | `binary(n)`                       | `precision`: 1–8000 (default 8000)                               |
| `IsDateColumn()`                    | `date`                            | —                                                                |
| `IsDateTimeColumn(small)`           | `datetime` / `smalldatetime`      | `small: false` → `datetime`                                      |
| `IsDateTime2Column(precision)`      | `datetime2(n)`                    | `precision`: 0–7 (default 7)                                     |
| `IsDateTimeOffsetColumn(precision)` | `datetimeoffset(n)`               | `precision`: 0–7 (default 7)                                     |
| `IsDecimalColumn(precision, scale)` | `decimal(p,s)`                    | `precision`: 1–38 (default 18), `scale`: 0–precision (default 2) |
| `IsMoneyColumn(small)`              | `money` / `smallmoney`            | `small: false` → `money`                                         |
| `IsSysNameColumn()`                 | `sysname`                         | —                                                                |
| `IsTimeColumn(precision)`           | `time(n)`                         | `precision`: 0–7 (default 7)                                     |
| `IsUniqueIdentifierColumn()`        | `uniqueidentifier`                | —                                                                |
| `IsVarbinaryColumn(precision)`      | `varbinary(n)` / `varbinary(max)` | `precision`: 0–8000; 0 → `varbinary(max)` (default 0)            |
| `IsXmlColumn()`                     | `xml`                             | —                                                                |

Methods with a precision parameter throw `ArgumentOutOfRangeException` when the value is outside the valid range.

```csharp
public class OrderConfiguration : IdentityConfiguration<Order>
{
    public override void Configure(EntityTypeBuilder<Order> builder)
    {
        base.Configure(builder);

        builder.ToTable("Orders", "sales");

        builder.Property(o => o.TotalAmount)
            .IsDecimalColumn(precision: 18, scale: 4);

        builder.Property(o => o.PlacedAt)
            .IsDateTimeOffsetColumn(precision: 3);

        builder.Property(o => o.DueDate)
            .IsDateColumn();

        builder.Property(o => o.Notes)
            .IsXmlColumn();

        builder.Property(o => o.Attachment)
            .IsVarbinaryColumn(); // varbinary(max)
    }
}
```

## `EntityTypeBuilderExtensions`

### `ToHistoryTable`

One-line setup for SQL Server temporal tables with a separate history schema:

```csharp
builder.ToHistoryTable(
    tableName: "Orders",          // defaults to entity class name
    tableSchema: "sales",         // defaults to "dbo"
    historyTableName: "Orders",   // defaults to tableName
    historyTableSchema: "history" // defaults to "history"
);
```

## Interceptors

Register interceptors on the `DbContextOptionsBuilder`:

```csharp
services.AddSingleton<SoftDeletableInterceptor>();
services.AddSingleton<TimeAuditedInterceptor>();

services.AddDbContext<AppDbContext>((sp, options) =>
{
    options
        .UseSqlServer(connectionString)
        .AddInterceptors(
            sp.GetRequiredService<SoftDeletableInterceptor>(),
            sp.GetRequiredService<TimeAuditedInterceptor>());
});
```

### `SoftDeletableInterceptor`

Fires on `SavingChanges`/`SavingChangesAsync`. For every entity tracked as `Deleted` that implements `ISoftDeletable`, it sets `IsDeleted = true` and changes the state to `Modified` — preventing a physical `DELETE` from being issued.

### `TimeAuditedInterceptor`

Fires on `SavingChanges`/`SavingChangesAsync`. For every entity implementing `ITimeAudited`:

- `EntityState.Added` → sets `CreatedAt = DateTimeOffset.UtcNow`
- `EntityState.Modified` → sets `EditedAt = DateTimeOffset.UtcNow`

`CreatedBy` / `EditedBy` are **not** set automatically by this interceptor — implement a custom `SaveChangesInterceptor` for user auditing and register it alongside the built-in ones.
