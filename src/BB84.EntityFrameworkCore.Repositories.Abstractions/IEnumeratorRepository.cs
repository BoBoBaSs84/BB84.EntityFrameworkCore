// Copyright: 2024 Robert Peter Meyer
// License: MIT
//
// This source code is licensed under the MIT license found in the
// LICENSE file in the root directory of this source tree.
using BB84.EntityFrameworkCore.Entities.Abstractions;

namespace BB84.EntityFrameworkCore.Repositories.Abstractions;

/// <summary>
/// Defines a repository interface for managing entities of type <see cref="IEnumeratorEntity{TKey}"/>
/// with a primary key of type <typeparamref name="TKey"/>.
/// </summary>
/// <remarks>
/// This interface extends the <see cref="IIdentityRepository{TEntity, TKey}"/> and adds functionality
/// specific to retrieving entities by their names. It supports both synchronous and asynchronous
/// operations, with options to ignore query filters and enable or disable change tracking.
/// </remarks>
/// <typeparam name="TEntity">The type of the entity managed by the repository.</typeparam>
/// <typeparam name="TKey">The type of the primary key for the entity.</typeparam>
public interface IEnumeratorRepository<TEntity, TKey> : IIdentityRepository<TEntity, TKey>
	where TEntity : class, IEnumeratorEntity<TKey>
	where TKey : IEquatable<TKey>
{
	/// <summary>
	/// Retrieves an entity by its name.
	/// </summary>
	/// <param name="name">The name of the entity to retrieve.</param>
	/// <param name="ignoreQueryFilters">A value indicating whether to ignore any query filters applied to the entity.</param>
	/// <param name="trackChanges">A value indicating whether the retrieved entity should be tracked by the context.</param>
	/// <returns>The entity that matches the specified name, or <see langword="null"/> if no such entity is found.</returns>
	TEntity? GetByName(
		string name,
		bool ignoreQueryFilters = false,
		bool trackChanges = false
		);

	/// <summary>
	/// Retrieves an entity by its name.
	/// </summary>
	/// <param name="name">The name of the entity to retrieve.</param>
	/// <param name="ignoreQueryFilters">A value indicating whether to ignore any query filters applied to the entity.</param>
	/// <param name="trackChanges">A value indicating whether the retrieved entity should be tracked by the context.</param>
	/// <param name="cancellationToken">The cancellation token to cancel the request.</param>
	/// <returns>The entity that matches the specified name, or <see langword="null"/> if no such entity is found.</returns>
	Task<TEntity?> GetByNameAsync(
		string name,
		bool ignoreQueryFilters = false,
		bool trackChanges = false,
		CancellationToken cancellationToken = default
		);

	/// <summary>
	/// Retrieves a collection of entities that match the specified names.
	/// </summary>
	/// <param name="names">A collection of names to filter the entities by.</param>
	/// <param name="ignoreQueryFilters">A value indicating whether to ignore any query filters applied to the entity type.</param>
	/// <param name="trackChanges">A value indicating whether the returned entities should be tracked by the context.</param>
	/// <returns>
	/// An <see cref="IEnumerable{TEntity}"/> containing the entities that match the specified names.
	/// If no entities match, an empty collection is returned.
	/// </returns>
	IEnumerable<TEntity> GetByNames(
		IEnumerable<string> names,
		bool ignoreQueryFilters = false,
		bool trackChanges = false
		);

	/// <summary>
	/// Retrieves a collection of entities that match the specified names.
	/// </summary>
	/// <param name="names">A collection of names to filter the entities by.</param>
	/// <param name="ignoreQueryFilters">A value indicating whether to ignore any query filters applied to the entity type.</param>
	/// <param name="trackChanges">A value indicating whether the returned entities should be tracked by the context.</param>
	/// <param name="cancellationToken">The cancellation token.</param>
	/// <returns>
	/// An <see cref="IEnumerable{TEntity}"/> containing the entities that match the specified names.
	/// If no entities match, an empty collection is returned.
	/// </returns>
	Task<IEnumerable<TEntity>> GetByNamesAsync(
		IEnumerable<string> names,
		bool ignoreQueryFilters = false,
		bool trackChanges = false,
		CancellationToken cancellationToken = default
		);
}

/// <inheritdoc cref="IEnumeratorRepository{TEntity, TKey}"/>
public interface IEnumeratorRepository<TEntity> : IEnumeratorRepository<TEntity, int>
	where TEntity : class, IEnumeratorEntity
{ }
