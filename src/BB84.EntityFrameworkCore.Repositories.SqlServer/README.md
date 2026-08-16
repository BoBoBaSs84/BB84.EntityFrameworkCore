[![net100](https://img.shields.io/badge/net10.0-5C2D91?logo=.NET&labelColor=gray)](https://github.com/BoBoBaSs84/BB84.Extensions)
[![NuGet](https://img.shields.io/nuget/v/BB84.EntityFrameworkCore.Repositories.SqlServer.svg?logo=nuget&logoColor=white)](https://www.nuget.org/packages/BB84.EntityFrameworkCore.Repositories.SqlServer)

# BB84.EntityFrameworkCore.Repositories.SqlServer

This package provides the SQL Server tuning of the entity type configuration base classes, `PropertyBuilderExtensions` for column types, `EntityTypeBuilderExtensions` for temporal table support, and `DatabaseFacadeExtensions` for calling stored procedures and SQL functions.

> **Moved in 5.0.** The configuration base classes now derive from provider-agnostic bases in
> `BB84.EntityFrameworkCore.Repositories`, and the interceptors moved there outright. `DatabaseFacadeExtensions` moved
> the other way, into this package, because it emits T-SQL. See [Package boundaries](#package-boundaries).

## Installation

```powershell
dotnet add package BB84.EntityFrameworkCore.Repositories.SqlServer
```

## Package boundaries

Each configuration ladder exists twice, under the **same type names**:

| Namespace                                                | Applies                                                                     |
| -------------------------------------------------------- | --------------------------------------------------------------------------- |
| `BB84.EntityFrameworkCore.Repositories.Configurations`   | Key declaration, column order, concurrency token, audit columns, soft delete filter |
| `BB84.EntityFrameworkCore.Repositories.SqlServer.Configurations` | The above, plus clustering, `NEWID()` defaults and `sysname` audit columns |

Pick the namespace, not the type. On SQL Server, keep importing this package's namespace and nothing changes. On any other provider, import the agnostic one and you get everything except the three SQL Server calls.

A file that imports **both** namespaces — a project mapping a SQL Server context and a second provider's context side by side — hits `CS0104` on the ambiguous name. Alias one of them:

```csharp
using SqlServerConfigurations = BB84.EntityFrameworkCore.Repositories.SqlServer.Configurations;
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

`SoftDeletableInterceptor` and `TimeAuditedInterceptor` moved to `BB84.EntityFrameworkCore.Repositories` in 5.0 — nothing in them was SQL Server specific. Update the `using` to `BB84.EntityFrameworkCore.Repositories.Interceptors`; the types and their behavior are unchanged.

## `DatabaseFacadeExtensions`

Extension methods on `DatabaseFacade` (`context.Database`) for calling SQL Server stored procedures and functions safely with parameterized SQL. These moved here from `BB84.EntityFrameworkCore.Repositories` in 5.0, because the SQL they emit — `EXECUTE [schema].[name] @p = @p OUTPUT`, `SELECT [Value] = [schema].[fn](@p)` — is T-SQL by construction.

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

All methods sanitize schema/name inputs and use parameterized SQL to prevent injection. The parameter sequence is materialized once on entry, so a lazily generated `IEnumerable<DbParameter>` is safe to pass.
