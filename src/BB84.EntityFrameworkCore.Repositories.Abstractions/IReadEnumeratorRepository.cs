// Copyright: 2024 Robert Peter Meyer
// License: MIT
//
// This source code is licensed under the MIT license found in the
// LICENSE file in the root directory of this source tree.
using BB84.EntityFrameworkCore.Entities.Abstractions;

namespace BB84.EntityFrameworkCore.Repositories.Abstractions;

/// <summary>
/// Defines the read half of a repository for entities of type
/// <see cref="IEnumeratorEntity{TKey}"/> with a primary key of type <typeparamref name="TKey"/>.
/// </summary>
/// <remarks>
/// Extends <see cref="IReadIdentityRepository{TEntity, TKey}"/> with name based lookups. There
/// is no write counterpart, because an enumerator repository adds no write operations of its
/// own beyond the key based ones it already inherits.
/// </remarks>
/// <typeparam name="TEntity">The type of the entity managed by the repository.</typeparam>
/// <typeparam name="TKey">The type of the primary key for the entity.</typeparam>
public interface IReadEnumeratorRepository<TEntity, TKey> : IReadIdentityRepository<TEntity, TKey>
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

/// <inheritdoc cref="IReadEnumeratorRepository{TEntity, TKey}"/>
/// <remarks>
/// The primary key is of type <see cref="int"/>.
/// </remarks>
public interface IReadEnumeratorRepository<TEntity> : IReadEnumeratorRepository<TEntity, int>
	where TEntity : class, IEnumeratorEntity
{ }
