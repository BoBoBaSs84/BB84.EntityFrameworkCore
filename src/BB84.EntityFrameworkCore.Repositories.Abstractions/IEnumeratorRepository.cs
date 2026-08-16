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
	/// Returns the entity with the specified <paramref name="name"/>, or <see langword="null"/>
	/// when no such entity exists.
	/// </summary>
	/// <remarks>
	/// The name acts as an additional condition. Anything the <paramref name="query"/> already
	/// filters by still applies.
	/// </remarks>
	/// <param name="name">The name of the <typeparamref name="TEntity"/>.</param>
	/// <param name="query">The query describing how to read.</param>
	/// <returns>The matching entity, or <see langword="null"/>.</returns>
	TEntity? GetByName(string name, Query<TEntity>? query = null);

	/// <inheritdoc cref="GetByName(string, Query{TEntity})"/>
	/// <param name="cancellationToken">The cancellation token to cancel the request.</param>
	Task<TEntity?> GetByNameAsync(
		string name,
		Query<TEntity>? query = null,
		CancellationToken cancellationToken = default);

	/// <summary>
	/// Returns the entities matching the specified <paramref name="names"/>.
	/// </summary>
	/// <remarks>
	/// The names act as an additional condition. Anything the <paramref name="query"/> already
	/// filters by still applies.
	/// </remarks>
	/// <param name="names">The names of the <typeparamref name="TEntity"/>.</param>
	/// <param name="query">The query describing how to read.</param>
	/// <returns>The matching entities.</returns>
	IReadOnlyList<TEntity> GetByNames(IEnumerable<string> names, Query<TEntity>? query = null);

	/// <inheritdoc cref="GetByNames(IEnumerable{string}, Query{TEntity})"/>
	/// <param name="cancellationToken">The cancellation token to cancel the request.</param>
	Task<IReadOnlyList<TEntity>> GetByNamesAsync(
		IEnumerable<string> names,
		Query<TEntity>? query = null,
		CancellationToken cancellationToken = default);
}

/// <inheritdoc cref="IEnumeratorRepository{TEntity, TKey}"/>
public interface IEnumeratorRepository<TEntity> : IEnumeratorRepository<TEntity, int>
	where TEntity : class, IEnumeratorEntity
{ }
