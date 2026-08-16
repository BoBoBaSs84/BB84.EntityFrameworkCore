// Copyright: 2024 Robert Peter Meyer
// License: MIT
//
// This source code is licensed under the MIT license found in the
// LICENSE file in the root directory of this source tree.
using System.Linq.Expressions;

namespace BB84.EntityFrameworkCore.Repositories.Abstractions;

/// <summary>
/// Defines the read half of a repository for entities of type <typeparamref name="TEntity"/>.
/// </summary>
/// <remarks>
/// <para>
/// Depend on this rather than on <see cref="IGenericRepository{TEntity}"/> wherever a component
/// only reads: a query handler, a reporting service, the read side of a CQRS split. The
/// dependency then states in its type that it cannot write.
/// </para>
/// <para>
/// The read methods take their options as a single <see cref="Query{TEntity}"/>. Passing none
/// reads the whole set, so there is no separate "all" method.
/// </para>
/// </remarks>
/// <typeparam name="TEntity">
/// The type of the entity for which the repository provides read access.
/// </typeparam>
public interface IReadRepository<TEntity>
	where TEntity : class
{
	/// <summary>
	/// Counts the entities matching the specified <paramref name="query"/>.
	/// </summary>
	/// <remarks>
	/// Only the filtering options of the query take part in a count. Ordering, paging,
	/// includes and tracking are ignored, because none of them can change how many rows match.
	/// </remarks>
	/// <param name="query">The query describing what to count. Counts everything when omitted.</param>
	/// <returns>The number of matching entities.</returns>
	int Count(Query<TEntity>? query = null);

	/// <inheritdoc cref="Count(Query{TEntity})"/>
	/// <param name="cancellationToken">The cancellation token to cancel the request.</param>
	Task<int> CountAsync(
		Query<TEntity>? query = null,
		CancellationToken cancellationToken = default);

	/// <summary>
	/// Returns the single entity matching the specified <paramref name="query"/>, or
	/// <see langword="null"/> when nothing matches.
	/// </summary>
	/// <remarks>
	/// Matching more than one entity is an error and throws. Use
	/// <see cref="GetList(Query{TEntity})"/> when several matches are expected.
	/// </remarks>
	/// <param name="query">The query describing what to read.</param>
	/// <returns>The matching entity, or <see langword="null"/>.</returns>
	/// <exception cref="InvalidOperationException">Thrown when more than one entity matches.</exception>
	TEntity? GetSingle(Query<TEntity>? query = null);

	/// <inheritdoc cref="GetSingle(Query{TEntity})"/>
	/// <typeparam name="TResult">The type the entity is projected into.</typeparam>
	/// <param name="selector">The projection applied to the matching entity.</param>
	/// <param name="query">The query describing what to read.</param>
	TResult? GetSingle<TResult>(
		Expression<Func<TEntity, TResult>> selector,
		Query<TEntity>? query = null);

	/// <inheritdoc cref="GetSingle(Query{TEntity})"/>
	/// <param name="cancellationToken">The cancellation token to cancel the request.</param>
	Task<TEntity?> GetSingleAsync(
		Query<TEntity>? query = null,
		CancellationToken cancellationToken = default);

	/// <inheritdoc cref="GetSingle{TResult}(Expression{Func{TEntity, TResult}}, Query{TEntity})"/>
	/// <param name="cancellationToken">The cancellation token to cancel the request.</param>
	Task<TResult?> GetSingleAsync<TResult>(
		Expression<Func<TEntity, TResult>> selector,
		Query<TEntity>? query = null,
		CancellationToken cancellationToken = default);

	/// <summary>
	/// Returns the entities matching the specified <paramref name="query"/>.
	/// </summary>
	/// <remarks>
	/// The result is buffered. Use <see cref="Stream(Query{TEntity}, CancellationToken)"/> for
	/// reads large enough that holding every row in memory is a problem.
	/// </remarks>
	/// <param name="query">The query describing what to read. Reads everything when omitted.</param>
	/// <returns>The matching entities.</returns>
	IReadOnlyList<TEntity> GetList(Query<TEntity>? query = null);

	/// <inheritdoc cref="GetList(Query{TEntity})"/>
	/// <typeparam name="TResult">The type the entities are projected into.</typeparam>
	/// <param name="selector">The projection applied to each matching entity.</param>
	/// <param name="query">The query describing what to read.</param>
	IReadOnlyList<TResult> GetList<TResult>(
		Expression<Func<TEntity, TResult>> selector,
		Query<TEntity>? query = null);

	/// <inheritdoc cref="GetList(Query{TEntity})"/>
	/// <param name="cancellationToken">The cancellation token to cancel the request.</param>
	Task<IReadOnlyList<TEntity>> GetListAsync(
		Query<TEntity>? query = null,
		CancellationToken cancellationToken = default);

	/// <inheritdoc cref="GetList{TResult}(Expression{Func{TEntity, TResult}}, Query{TEntity})"/>
	/// <param name="cancellationToken">The cancellation token to cancel the request.</param>
	Task<IReadOnlyList<TResult>> GetListAsync<TResult>(
		Expression<Func<TEntity, TResult>> selector,
		Query<TEntity>? query = null,
		CancellationToken cancellationToken = default);

	/// <summary>
	/// Streams the entities matching the specified <paramref name="query"/>.
	/// </summary>
	/// <remarks>
	/// <para>
	/// Rows are yielded as they arrive instead of being buffered, which suits exports, batch
	/// jobs and other unbounded reads.
	/// </para>
	/// <para>
	/// The sequence is lazy. It has to be enumerated within the lifetime of the context, and
	/// EF Core allows only one active stream per context at a time. Leave
	/// <see cref="Query{TEntity}.TrackChanges"/> off, or the change tracker grows with every
	/// entity yielded and the point of streaming is lost.
	/// </para>
	/// </remarks>
	/// <param name="query">The query describing what to read. Reads everything when omitted.</param>
	/// <param name="cancellationToken">The cancellation token to cancel the request.</param>
	/// <returns>The matching entities, yielded as they arrive.</returns>
	IAsyncEnumerable<TEntity> Stream(
		Query<TEntity>? query = null,
		CancellationToken cancellationToken = default);

	/// <inheritdoc cref="Stream(Query{TEntity}, CancellationToken)"/>
	/// <typeparam name="TResult">The type the entities are projected into.</typeparam>
	/// <param name="selector">The projection applied to each matching entity.</param>
	/// <param name="query">The query describing what to read.</param>
	/// <param name="cancellationToken">The cancellation token to cancel the request.</param>
	IAsyncEnumerable<TResult> Stream<TResult>(
		Expression<Func<TEntity, TResult>> selector,
		Query<TEntity>? query = null,
		CancellationToken cancellationToken = default);
}
