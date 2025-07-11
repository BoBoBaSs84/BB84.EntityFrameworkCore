// Copyright: 2024 Robert Peter Meyer
// License: MIT
//
// This source code is licensed under the MIT license found in the
// LICENSE file in the root directory of this source tree.
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;

namespace BB84.EntityFrameworkCore.Repositories.Abstractions;

/// <summary>
/// Defines the contract for a database context, providing functionality for
/// querying and saving data.
/// </summary>
/// <remarks>
/// This interface represents the core abstraction for interacting with a database
/// in an Entity Framework Core application. It provides access to the change tracker,
/// database connection, and model metadata, as well as methods for saving changes and
/// managing entity sets. Implementations of this interface are typically used to
/// encapsulate database operations and enforce a unit of work pattern.
/// </remarks>
public interface IDbContext : IAsyncDisposable, IDisposable
{
	/// <inheritdoc cref="DbContext.SavingChanges"/>
	event EventHandler<SavingChangesEventArgs>? SavingChanges;

	/// <inheritdoc cref="DbContext.SavedChanges"/>
	event EventHandler<SavedChangesEventArgs>? SavedChanges;

	/// <inheritdoc cref="DbContext.SaveChangesFailed"/>
	event EventHandler<SaveChangesFailedEventArgs>? SaveChangesFailed;

	/// <inheritdoc cref="DbContext.ChangeTracker"/>
	ChangeTracker ChangeTracker { get; }

	/// <inheritdoc cref="DbContext.ContextId"/>
	DbContextId ContextId { get; }

	/// <inheritdoc cref="DbContext.Database"/>
	DatabaseFacade Database { get; }

	/// <inheritdoc cref="DbContext.Model"/>
	IModel Model { get; }

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
