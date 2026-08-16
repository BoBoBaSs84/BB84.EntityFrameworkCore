// Copyright: 2024 Robert Peter Meyer
// License: MIT
//
// This source code is licensed under the MIT license found in the
// LICENSE file in the root directory of this source tree.
using System.Linq.Expressions;

using BB84.EntityFrameworkCore.Entities.Abstractions;

namespace BB84.EntityFrameworkCore.Repositories.Abstractions;

/// <summary>
/// Defines the read half of a repository for entities of type
/// <see cref="IIdentityEntity{TKey}"/> with a primary key of type <typeparamref name="TKey"/>.
/// </summary>
/// <remarks>
/// Extends <see cref="IReadRepository{TEntity}"/> with key based lookups.
/// </remarks>
/// <typeparam name="TEntity">The type of the entity managed by the repository.</typeparam>
/// <typeparam name="TKey">The type of the primary key for the entity.</typeparam>
public interface IReadIdentityRepository<TEntity, TKey> : IReadRepository<TEntity>
	where TEntity : class, IIdentityEntity<TKey>
	where TKey : IEquatable<TKey>
{
	/// <summary>
	/// Returns the entity with the specified <paramref name="id"/>, or <see langword="null"/>
	/// when no such entity exists.
	/// </summary>
	/// <remarks>
	/// The identifier acts as an additional condition. Anything the <paramref name="query"/>
	/// already filters by still applies.
	/// </remarks>
	/// <param name="id">The primary key of the <typeparamref name="TEntity"/>.</param>
	/// <param name="query">The query describing how to read.</param>
	/// <returns>The matching entity, or <see langword="null"/>.</returns>
	TEntity? GetById(TKey id, Query<TEntity>? query = null);

	/// <inheritdoc cref="GetById(TKey, Query{TEntity})"/>
	/// <typeparam name="TResult">The type the entity is projected into.</typeparam>
	/// <param name="id">The primary key of the <typeparamref name="TEntity"/>.</param>
	/// <param name="selector">The projection applied to the matching entity.</param>
	/// <param name="query">The query describing how to read.</param>
	TResult? GetById<TResult>(
		TKey id,
		Expression<Func<TEntity, TResult>> selector,
		Query<TEntity>? query = null);

	/// <inheritdoc cref="GetById(TKey, Query{TEntity})"/>
	/// <param name="cancellationToken">The cancellation token to cancel the request.</param>
	Task<TEntity?> GetByIdAsync(
		TKey id,
		Query<TEntity>? query = null,
		CancellationToken cancellationToken = default);

	/// <inheritdoc cref="GetById{TResult}(TKey, Expression{Func{TEntity, TResult}}, Query{TEntity})"/>
	/// <param name="cancellationToken">The cancellation token to cancel the request.</param>
	Task<TResult?> GetByIdAsync<TResult>(
		TKey id,
		Expression<Func<TEntity, TResult>> selector,
		Query<TEntity>? query = null,
		CancellationToken cancellationToken = default);

	/// <summary>
	/// Returns the entities matching the specified <paramref name="ids"/>.
	/// </summary>
	/// <remarks>
	/// The order of the results is not guaranteed to match the order of the identifiers, and an
	/// identifier without a matching row is simply absent from the result. The identifiers act
	/// as an additional condition, so anything the <paramref name="query"/> already filters by
	/// still applies.
	/// </remarks>
	/// <param name="ids">The primary keys of the <typeparamref name="TEntity"/>.</param>
	/// <param name="query">The query describing how to read.</param>
	/// <returns>The matching entities.</returns>
	IReadOnlyList<TEntity> GetByIds(IEnumerable<TKey> ids, Query<TEntity>? query = null);

	/// <inheritdoc cref="GetByIds(IEnumerable{TKey}, Query{TEntity})"/>
	/// <typeparam name="TResult">The type the entities are projected into.</typeparam>
	/// <param name="ids">The primary keys of the <typeparamref name="TEntity"/>.</param>
	/// <param name="selector">The projection applied to each matching entity.</param>
	/// <param name="query">The query describing how to read.</param>
	IReadOnlyList<TResult> GetByIds<TResult>(
		IEnumerable<TKey> ids,
		Expression<Func<TEntity, TResult>> selector,
		Query<TEntity>? query = null);

	/// <inheritdoc cref="GetByIds(IEnumerable{TKey}, Query{TEntity})"/>
	/// <param name="cancellationToken">The cancellation token to cancel the request.</param>
	Task<IReadOnlyList<TEntity>> GetByIdsAsync(
		IEnumerable<TKey> ids,
		Query<TEntity>? query = null,
		CancellationToken cancellationToken = default);

	/// <inheritdoc cref="GetByIds{TResult}(IEnumerable{TKey}, Expression{Func{TEntity, TResult}}, Query{TEntity})"/>
	/// <param name="cancellationToken">The cancellation token to cancel the request.</param>
	Task<IReadOnlyList<TResult>> GetByIdsAsync<TResult>(
		IEnumerable<TKey> ids,
		Expression<Func<TEntity, TResult>> selector,
		Query<TEntity>? query = null,
		CancellationToken cancellationToken = default);
}

/// <inheritdoc cref="IReadIdentityRepository{TEntity, TKey}"/>
/// <remarks>
/// The primary key is of type <see cref="Guid"/>.
/// </remarks>
public interface IReadIdentityRepository<TEntity> : IReadIdentityRepository<TEntity, Guid>
	where TEntity : class, IIdentityEntity
{ }
