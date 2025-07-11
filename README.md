# BB84.EntityFrameworkCore

[![CI](https://github.com/BoBoBaSs84/BB84.EntityFrameworkCore/actions/workflows/ci.yml/badge.svg?branch=main)](https://github.com/BoBoBaSs84/BB84.EntityFrameworkCore/actions/workflows/ci.yml)
[![CD](https://github.com/BoBoBaSs84/BB84.EntityFrameworkCore/actions/workflows/cd.yml/badge.svg?branch=main)](https://github.com/BoBoBaSs84/BB84.EntityFrameworkCore/actions/workflows/cd.yml)
[![CodeQL](https://github.com/BoBoBaSs84/BB84.EntityFrameworkCore/actions/workflows/github-code-scanning/codeql/badge.svg?branch=main)](https://github.com/BoBoBaSs84/BB84.EntityFrameworkCore/actions/workflows/github-code-scanning/codeql)
[![Dependabot](https://github.com/BoBoBaSs84/BB84.EntityFrameworkCore/actions/workflows/dependabot/dependabot-updates/badge.svg?branch=main)](https://github.com/BoBoBaSs84/BB84.EntityFrameworkCore/actions/workflows/dependabot/dependabot-updates)

[![.NET](https://img.shields.io/badge/net8.0-5C2D91?logo=.NET&labelColor=gray)](https://github.com/BoBoBaSs84/BB84.EntityFrameworkCore)
[![C#](https://img.shields.io/badge/C%23-13.0-239120)](https://github.com/BoBoBaSs84/BB84.EntityFrameworkCore)
[![Issues](https://img.shields.io/github/issues/BoBoBaSs84/BB84.EntityFrameworkCore)](https://github.com/BoBoBaSs84/BB84.EntityFrameworkCore/issues)
[![Commit](https://img.shields.io/github/last-commit/BoBoBaSs84/BB84.EntityFrameworkCore)](https://github.com/BoBoBaSs84/BB84.EntityFrameworkCore/commit/main)
[![License](https://img.shields.io/github/license/BoBoBaSs84/BB84.EntityFrameworkCore)](https://github.com/BoBoBaSs84/BB84.EntityFrameworkCore/blob/main/LICENSE)
[![RepoSize](https://img.shields.io/github/repo-size/BoBoBaSs84/BB84.EntityFrameworkCore)](https://github.com/BoBoBaSs84/BB84.EntityFrameworkCore)
[![Release](https://img.shields.io/github/v/release/BoBoBaSs84/BB84.EntityFrameworkCore)](https://github.com/BoBoBaSs84/BB84.EntityFrameworkCore/releases/latest)

## 🧭 Overview

**BB84.EntityFrameworkCore** is a comprehensive .NET 8.0 library that provides a reusable repository pattern implementation for ASP.NET Core applications. The library offers commonly used entity abstractions, their default implementations, and repository abstractions with their corresponding implementations, specifically designed to work seamlessly with Entity Framework Core.

### Key Features

- **Generic Repository Pattern**: Complete implementation of the repository pattern with CRUD operations
- **Entity Abstractions**: Pre-built interfaces for common entity types (Identity, Audited, Composite, etc.)
- **SQL Server Integration**: Specialized configurations and extensions for SQL Server
- **Auditing Support**: Built-in support for creation and modification tracking
- **Soft Delete**: Integrated soft delete functionality
- **Concurrency Control**: Built-in optimistic concurrency support
- **Type Safety**: Strongly typed implementations with generic constraints

### Target Framework

- **.NET 8.0** - Latest LTS version of .NET

## 🏗 Project Structure

```
BB84.EntityFrameworkCore/
├── src/                                                    # Source code
│   ├── BB84.EntityFrameworkCore.Entities.Abstractions/     # Entity interfaces
│   ├── BB84.EntityFrameworkCore.Entities/                  # Entity implementations
│   ├── BB84.EntityFrameworkCore.Repositories.Abstractions/ # Repository interfaces
│   ├── BB84.EntityFrameworkCore.Repositories/              # Repository implementations
│   └── BB84.EntityFrameworkCore.Repositories.SqlServer/    # SQL Server specific features
├── tests/                                                  # Unit tests
│   ├── BB84.EntityFrameworkCore.Entities.Tests/
│   └── BB84.EntityFrameworkCore.Repositories.Tests/
├── docs/                                                   # Documentation
├── Directory.Build.props                                   # Build configuration
├── Directory.Packages.props                                # Package management
└── BB84.EntityFrameworkCore.sln                            # Solution file
```

## 📦 Package Architecture

The project is organized into five distinct NuGet packages, each serving a specific purpose:

### BB84.EntityFrameworkCore.Entities.Abstractions

**Purpose**: Core entity interface definitions
**Dependencies**: None

This package contains the fundamental interfaces that define the contract for different types of entities:

- `IIdentityEntity<TKey>` - Entities with unique identifiers
- `IAuditedEntity<TKey, TCreator, TEdited>` - Entities with creation/modification tracking
- `IFullAuditedEntity<TKey, TCreator, TEdited>` - Entities with full audit trails including soft delete
- `ICompositeEntity` - Entities with composite keys
- `IEnumeratorEntity<TKey>` - Entities representing enumeration values

**Component Interfaces**:

- `IIdentity<TKey>` - Provides unique identification
- `IConcurrency` - Provides optimistic concurrency control
- `ITimeAudited` - Tracks creation and modification timestamps
- `IUserAudited<TCreator, TEdited>` - Tracks user information for auditing
- `ISoftDeletable` - Provides soft delete functionality

### BB84.EntityFrameworkCore.Entities

**Purpose**: Default implementations of entity abstractions
**Dependencies**: BB84.EntityFrameworkCore.Entities.Abstractions

Provides concrete implementations of the entity interfaces:

- `IdentityEntity<TKey>` - Base class for entities with identity
- `AuditedEntity<TKey, TCreator, TEdited>` - Base class for audited entities
- `FullAuditedEntity<TKey, TCreator, TEdited>` - Base class for fully audited entities
- `CompositeEntity` - Base class for composite key entities
- `EnumeratorEntity<TKey>` - Base class for enumeration entities

### BB84.EntityFrameworkCore.Repositories.Abstractions

**Purpose**: Repository interface definitions
**Dependencies**:

- BB84.EntityFrameworkCore.Entities.Abstractions
- Microsoft.EntityFrameworkCore.Relational (9.0.6)

Core repository interfaces:

- `IDbContext` - Database context abstraction
- `IGenericRepository<TEntity>` - Generic CRUD operations
- `IIdentityRepository<TEntity, TKey>` - Repository for identity-based entities
- `IEnumeratorRepository<TEntity, TKey>` - Repository for enumeration entities

### BB84.EntityFrameworkCore.Repositories

**Purpose**: Default repository implementations
**Dependencies**:

- BB84.EntityFrameworkCore.Entities
- BB84.EntityFrameworkCore.Repositories.Abstractions

Concrete repository implementations:

- `GenericRepository<TEntity>` - Base repository with common operations
- `IdentityRepository<TEntity, TKey>` - Repository for identity entities
- `EnumeratorRepository<TEntity, TKey>` - Repository for enumerator entities

### BB84.EntityFrameworkCore.Repositories.SqlServer

**Purpose**: SQL Server specific configurations and extensions
**Dependencies**: BB84.EntityFrameworkCore.Repositories

SQL Server specific features:

- **Configurations**: Entity type configurations for different entity types
- **Extensions**: Extension methods for Entity Framework configurations
- **Interceptors**: Database interceptors for auditing and soft delete

## 💭 Core Concepts

### Entity Hierarchy

The library follows a hierarchical approach to entity design:

```
IIdentityEntity<TKey>
├── IAuditedEntity<TKey, TCreator, TEdited>
│   └── IFullAuditedEntity<TKey, TCreator, TEdited>
├── ICompositeEntity
│   └── IAuditedCompositeEntity<TCreator, TEdited>
└── IEnumeratorEntity<TKey>
```

### Repository Pattern

The repository pattern implementation provides:

- **Separation of Concerns**: Business logic separated from data access
- **Testability**: Easy mocking and unit testing
- **Consistency**: Standardized data access patterns
- **Flexibility**: Support for different entity types and requirements

## ✨ Entity Abstractions

### Identity Entities

Identity entities represent the most basic entity type with a unique identifier and concurrency control.

```csharp
public interface IIdentityEntity<TKey> : IIdentity<TKey>, IConcurrency
    where TKey : IEquatable<TKey>
{
    // Inherits:
    // TKey Id { get; set; }           // From IIdentity<TKey>
    // byte[]? Timestamp { get; set; } // From IConcurrency
}

// Default implementation with Guid
public interface IIdentityEntity : IIdentityEntity<Guid>
```

**Key Features**:

- Unique identifier of specified type
- Optimistic concurrency control via timestamp
- Base interface for all other entity types

### Audited Entities

Audited entities extend identity entities with creation and modification tracking.

```csharp
public interface IAuditedEntity<TKey, TCreator, TEdited> :
    IIdentityEntity<TKey>, IUserAudited<TCreator, TEdited>
    where TKey : IEquatable<TKey>
    where TCreator : notnull
{
    // Inherits from IIdentityEntity<TKey>:
    // TKey Id { get; set; }
    // byte[]? Timestamp { get; set; }

    // Inherits from IUserAudited<TCreator, TEdited>:
    // TCreator CreatedBy { get; set; }
    // TEdited? EditedBy { get; set; }
    // DateTime CreatedAt { get; set; }
    // DateTime? EditedAt { get; set; }
}
```

**Convenience Overloads**:

```csharp
// Default string-based user types
public interface IAuditedEntity<TKey> : IAuditedEntity<TKey, string, string?>

// Default Guid key with string users
public interface IAuditedEntity : IAuditedEntity<Guid, string, string?>
```

### Full Audited Entities

Full audited entities include soft delete capabilities in addition to standard auditing.

```csharp
public interface IFullAuditedEntity<TKey, TCreator, TEdited> :
    IAuditedEntity<TKey, TCreator, TEdited>, ISoftDeletable
    where TKey : IEquatable<TKey>
    where TCreator : notnull
{
    // Inherits all from IAuditedEntity plus:
    // bool IsDeleted { get; set; }      // From ISoftDeletable
    // DateTime? DeletedAt { get; set; } // From ISoftDeletable
}
```

### Composite Entities

Composite entities are designed for entities with composite primary keys.

```csharp
public interface ICompositeEntity : IConcurrency
{
    // byte[]? Timestamp { get; set; } // From IConcurrency
}

public interface IAuditedCompositeEntity<TCreator, TEdited> :
    ICompositeEntity, IUserAudited<TCreator, TEdited>
    where TCreator : notnull
```

### Enumerator Entities

Enumerator entities represent lookup/reference data with additional descriptive properties.

```csharp
public interface IEnumeratorEntity<TKey> : IIdentityEntity<TKey>
    where TKey : IEquatable<TKey>
{
    // Inherits from IIdentityEntity<TKey> plus:
    string Name { get; set; }
    string? Description { get; set; }
}
```

## ⚡ Repository Pattern

### Generic Repository

The `IGenericRepository<TEntity>` provides standard CRUD operations, just to name a few:

```csharp
public interface IGenericRepository<TEntity> where TEntity : class
{
    // Create operations
    void Create(TEntity entity);
    void Create(IEnumerable<TEntity> entities);
    Task CreateAsync(TEntity entity, CancellationToken cancellationToken = default);
    Task CreateAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default);

    // Read operations
    IEnumerable<TEntity> GetAll(bool ignoreQueryFilters = false, bool trackChanges = false);
    Task<IEnumerable<TEntity>> GetAllAsync(bool ignoreQueryFilters = false, bool trackChanges = false, CancellationToken token = default);

    // Update operations
    void Update(TEntity entity);
    void Update(IEnumerable<TEntity> entities);
    Task UpdateAsync(TEntity entity);
    Task UpdateAsync(IEnumerable<TEntity> entities);

    // Delete operations
    void Delete(TEntity entity);
    void Delete(IEnumerable<TEntity> entities);
    Task DeleteAsync(TEntity entity);
    Task DeleteAsync(IEnumerable<TEntity> entities);

    // Query operations
    int CountAll(bool ignoreQueryFilters = false);
    Task<int> CountAllAsync(bool ignoreQueryFilters = false, CancellationToken token = default);
}
```

### Identity Repository

Specialized repository for identity-based entities:

```csharp
public interface IIdentityRepository<TEntity, TKey> : IGenericRepository<TEntity>
    where TEntity : class, IIdentityEntity<TKey>
    where TKey : IEquatable<TKey>
{
    // Strongly-typed ID operations
    TEntity? GetById(TKey id);
    Task<TEntity?> GetByIdAsync(TKey id, CancellationToken cancellationToken = default);
    void Delete(TKey id);
    Task DeleteAsync(TKey id, CancellationToken cancellationToken = default);
}
```

## ⭐ Configuration Classes

The SQL Server package provides base configuration classes for Entity Framework Core entity type configurations.

### Identity Configuration

```csharp
public abstract class IdentityConfiguration<TEntity, TKey> : IEntityTypeConfiguration<TEntity>
    where TEntity : class, IIdentityEntity<TKey>
    where TKey : IEquatable<TKey>
{
    public virtual void Configure(EntityTypeBuilder<TEntity> builder)
    {
        // Configures primary key, timestamps, and common properties
    }
}
```

### Audited Configuration

```csharp
public abstract class AuditedConfiguration<TEntity, TKey, TCreator, TEdited> : IEntityTypeConfiguration<TEntity>
    where TEntity : class, IAuditedEntity<TKey, TCreator, TEdited>
    where TKey : IEquatable<TKey>
    where TCreator : notnull
{
    public virtual void Configure(EntityTypeBuilder<TEntity> builder)
    {
        // Configures auditing fields, required properties, and indexes
    }
}
```

### Full Audited Configuration

```csharp
public abstract class FullAuditedConfiguration<TEntity, TKey, TCreator, TEdited> : IEntityTypeConfiguration<TEntity>
    where TEntity : class, IFullAuditedEntity<TKey, TCreator, TEdited>
    where TKey : IEquatable<TKey>
    where TCreator : notnull
{
    public virtual void Configure(EntityTypeBuilder<TEntity> builder)
    {
        // Configures full auditing including soft delete capabilities
    }
}
```

## 🔮 Interceptors

The library provides Entity Framework Core interceptors for automatic handling of common scenarios.

### Time Audited Interceptor

Automatically sets `Created` and `Edited` timestamps:

```csharp
public class TimeAuditedInterceptor : SaveChangesInterceptor
{
    // Automatically updates timestamps on save operations
}
```

### Soft Deletable Interceptor

Handles soft delete operations by setting `IsDeleted`:

```csharp
public class SoftDeletableInterceptor : SaveChangesInterceptor
{
    // Converts hard deletes to soft deletes for applicable entities
}
```

## 💻 Getting Started

### Installation

Install the packages via NuGet Package Manager or .NET CLI:

```powershell
# For entity abstractions only
dotnet add package BB84.EntityFrameworkCore.Entities.Abstractions

# For entity implementations
dotnet add package BB84.EntityFrameworkCore.Entities

# For repository abstractions
dotnet add package BB84.EntityFrameworkCore.Repositories.Abstractions

# For repository implementations
dotnet add package BB84.EntityFrameworkCore.Repositories

# For SQL Server specific features
dotnet add package BB84.EntityFrameworkCore.Repositories.SqlServer
```

### Basic Setup

1. **Define your entities**:

```csharp
public class User : AuditedEntity
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
}

public class Product : FullAuditedEntity
{
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public string? Description { get; set; }
}
```

2. **Create repository interfaces**:

```csharp
public interface IUserRepository : IIdentityRepository<User>
{
    Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);
}

public interface IProductRepository : IIdentityRepository<Product>
{
    Task<IEnumerable<Product>> GetActiveProductsAsync(CancellationToken cancellationToken = default);
}
```

3. **Implement repositories**:

```csharp
public class UserRepository : IdentityRepository<User>, IUserRepository
{
    public UserRepository(IDbContext dbContext) : base(dbContext) { }

    public async Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        return await GetByConditionAsync(u => u.Email == email, token: cancellationToken);
    }
}

public class ProductRepository : IdentityRepository<Product>, IProductRepository
{
    public ProductRepository(IDbContext dbContext) : base(dbContext) { }

    public async Task<IEnumerable<Product>> GetActiveProductsAsync(CancellationToken cancellationToken = default)
    {
        return await GetManyByConditionAsync(p => !p.IsDeleted, token: cancellationToken);
    }
}
```

4. **Configure Entity Framework**:

```csharp
public class ApplicationDbContext : DbContext, IDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    public DbSet<User> Users { get; set; }
    public DbSet<Product> Products { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
    }
}

// Entity configurations
public class UserConfiguration : AuditedConfiguration<User>
{
    public override void Configure(EntityTypeBuilder<User> builder)
    {
        builder.Property(u => u.Email).IsRequired().HasMaxLength(255);
        builder.HasIndex(u => u.Email).IsUnique();

        base.Configure(builder);
    }
}

public class ProductConfiguration : FullAuditedConfiguration<Product>
{
    public override void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.Property(p => p.Name).IsRequired().HasMaxLength(200);
        builder.Property(p => p.Price).HasPrecision(18, 2);

        base.Configure(builder);
    }
}
```

5. **Register services**:

```csharp
public void ConfigureServices(IServiceCollection services)
{
    services.AddDbContext<ApplicationDbContext>(options =>
        options.UseSqlServer(connectionString));

    services.AddScoped<IDbContext>(provider => provider.GetService<ApplicationDbContext>());
    services.AddScoped<IUserRepository, UserRepository>();
    services.AddScoped<IProductRepository, ProductRepository>();

    // Register interceptors
    services.AddScoped<TimeAuditedInterceptor>();
    services.AddScoped<SoftDeletableInterceptor>();
}
```

## 🧰 Usage Examples

### Basic CRUD Operations

```csharp
public class UserService
{
    private readonly IUserRepository _userRepository;
    private readonly IDbContext _dbContext;

    public UserService(IUserRepository userRepository, IDbContext dbContext)
    {
        _userRepository = userRepository;
        _dbContext = dbContext;
    }

    public async Task<User> CreateUserAsync(string firstName, string lastName, string email)
    {
        var user = new User
        {
            FirstName = firstName,
            LastName = lastName,
            Email = email,
            CreatedBy = "system" // Will be set automatically by interceptor
        };

        await _userRepository.CreateAsync(user);
        await _dbContext.SaveChangesAsync();

        return user;
    }

    public async Task<User?> GetUserByEmailAsync(string email)
    {
        return await _userRepository.GetByEmailAsync(email);
    }

    public async Task UpdateUserAsync(User user)
    {
        _userRepository.Update(user);
        await _dbContext.SaveChangesAsync();
    }

    public async Task DeleteUserAsync(Guid userId)
    {
        await _userRepository.DeleteByIdAsync(userId);
        await _dbContext.SaveChangesAsync();
    }
}
```

### Advanced Querying

```csharp
public class ProductService
{
    private readonly IProductRepository _productRepository;

    public ProductService(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<IEnumerable<Product>> SearchProductsAsync(string searchTerm, decimal? minPrice = null)
    {
        return await _productRepository.GetManyByConditionAsync(p =>
            !p.IsDeleted &&
            p.Name.Contains(searchTerm) &&
            (minPrice == null || p.Price >= minPrice));
    }
}
```

## 🩺 Testing

The project includes comprehensive unit tests for all components:

### Test Projects

- **BB84.EntityFrameworkCore.Entities.Tests**: Tests for entity implementations
- **BB84.EntityFrameworkCore.Repositories.Tests**: Tests for repository implementations

### Test Coverage Areas

1. **Entity Tests**: Verify entity behavior and property assignments
2. **Repository Tests**: Test CRUD operations and query functionality
3. **Configuration Tests**: Validate Entity Framework configurations
4. **Interceptor Tests**: Test automatic auditing and soft delete behavior

### Example Test

```csharp
[TestClass]
public sealed class IdentityEntityTests
{
    [TestMethod]
    public void IdentityEntityTest()
    {
        IIdentityEntity? entity;
        Guid id = Guid.NewGuid();

        entity = new TestClass()
        {
            Id = id
        };

        Assert.IsNotNull(entity);
        Assert.AreEqual(id, entity.Id);
        Assert.IsNull(entity.Timestamp);
    }

    private sealed class TestClass : IdentityEntity
    { }
}
```

### Running Tests

```powershell
# Run all tests
dotnet test

# Run tests with coverage
dotnet test --collect:"XPlat Code Coverage"

# Run specific test project
dotnet test tests/BB84.EntityFrameworkCore.Entities.Tests/
```

## 🛠 Build and Deployment

### Build Configuration

The project uses MSBuild properties defined in `Directory.Build.props`:

- **Target Framework**: .NET 8.0
- **Language Version**: Latest C#
- **Nullable**: Enabled
- **Implicit Usings**: Enabled
- **Documentation**: Generated for all projects

### Version Management

Versions are automatically generated based on:

- Major: 3
- Minor: 1
- Patch: Current date (MMDD format)
- Revision: Current hour

### Package Information

- **Author**: BoBoBaSs84
- **License**: MIT
- **Repository**: https://github.com/BoBoBaSs84/BB84.EntityFrameworkCore

### Build Commands

```powershell
# Restore packages
dotnet restore

# Build solution
dotnet build

# Build release
dotnet build --configuration Release

# Pack NuGet packages
dotnet pack --configuration Release
```

### CI/CD Pipeline

The project uses GitHub Actions for:

- **Continuous Integration (CI)**: Automated building and testing
- **Continuous Deployment (CD)**: Automated package publishing
- **Code Analysis**: CodeQL security scanning
- **Dependency Updates**: Dependabot automated updates

## 📚 API Documentation

Complete API documentation is available [here](https://bobobass84.github.io/BB84.EntityFrameworkCore).

The documentation is generated using DocFX and includes:

- Complete API reference for all public types
- Code examples and usage patterns
- Cross-references between related types
- Inheritance hierarchies

### Documentation Structure

```
docs/
├── docfx.json   # DocFX configuration
├── index.md     # Documentation homepage
├── toc.yml      # Table of contents
└── api/
    └── index.md # API reference index
```

## 🤝 Contributing

### Development Guidelines

1. **Code Style**: Follow Microsoft C# coding conventions
2. **Documentation**: All public APIs must be documented with XML comments
3. **Testing**: All new features must include corresponding unit tests
4. **Breaking Changes**: Follow semantic versioning principles

### Contribution Process

1. Fork the repository
2. Create a feature branch
3. Implement changes with tests
4. Update documentation as needed
5. Submit a pull request

### Code of Conduct

This project adheres to the Contributor Covenant Code of Conduct. See the [CODE_OF_CONDUCT](CODE_OF_CONDUCT.md) file for details.

## ⚖ License

This project is licensed under the MIT License. See the [LICENSE](LICENSE) file for details.

---
