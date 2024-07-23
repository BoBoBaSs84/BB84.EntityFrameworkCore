#pragma warning disable CA1716 // Identifiers should not match keywords
using Microsoft.EntityFrameworkCore;

namespace BB84.EntityFrameworkCore.Repositories.Abstractions;

/// <summary>
/// The interface fo the database context.
/// </summary>
/// <remarks>
/// Every custom database context should inherit from this interface.
/// </remarks>
public interface IDbContext : IDisposable
{
	/// <inheritdoc cref="DbContext.Set{TEntity}()"/>
	DbSet<TEntity> Set<TEntity>() where TEntity : class;

	/// <inheritdoc cref="DbContext.SaveChanges(bool)"/>
	int SaveChanges(bool acceptAllChangesOnSuccess);

	/// <inheritdoc cref="DbContext.SaveChanges()"/>
	int SaveChanges();

	/// <inheritdoc cref="DbContext.SaveChangesAsync(bool, CancellationToken)"/>
	Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default);

	/// <inheritdoc cref="DbContext.SaveChangesAsync(CancellationToken)"/>
	Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
