// Copyright: 2024 Robert Peter Meyer
// License: MIT
//
// This source code is licensed under the MIT license found in the
// LICENSE file in the root directory of this source tree.
using System.Linq.Expressions;

using Microsoft.EntityFrameworkCore.Query;

namespace BB84.EntityFrameworkCore.Repositories.Abstractions;

/// <summary>
/// Defines a generic repository interface for performing CRUD operations and querying
/// entities of type <typeparamref name="TEntity"/>.
/// </summary>
/// <remarks>
/// <para>
/// This interface provides synchronous and asynchronous methods for creating, reading,
/// updating, and deleting entities, as well as methods for querying entities based on
/// conditions. It is designed to abstract data access logic, making it easier to work
/// with different data sources or implement unit testing.
/// </para>
/// <para>
/// The read methods take their options as a single <see cref="Query{TEntity}"/>. Passing
/// none reads the whole set, so there is no separate "all" method.
/// </para>
/// </remarks>
/// <typeparam name="TEntity">
/// The type of the entity for which the repository provides data access functionality.
/// </typeparam>
public interface IGenericRepository<TEntity>
	where TEntity : class
{
	/// <summary>
	/// Adds the specified entity to the underlying data store.
	/// </summary>
	/// <remarks>
	/// This method marks the provided entity as added in the database context, so that
	/// changes to the entity will be persisted to the database during the next save operation.
	/// </remarks>
	/// <param name="entity">The entity to add.</param>
	void Create(TEntity entity);

	/// <summary>
	/// Adds the specified collection of entities to the underlying data store.
	/// </summary>
	/// <remarks>
	/// This method marks the provided entities as added in the database context. so that
	/// changes to the entity will be persisted to the database during the next save operation.
	/// </remarks>
	/// <param name="entities">The collection of entities to add.</param>
	void Create(IEnumerable<TEntity> entities);

	/// <inheritdoc cref="Create(TEntity)"/>
	/// <param name="cancellationToken">The cancellation token to cancel the request.</param>
	/// <returns>The <see cref="Task"/> representing the asynchronous operation.</returns>
	Task CreateAsync(
		TEntity entity,
		CancellationToken cancellationToken = default);

	/// <inheritdoc cref="Create(IEnumerable{TEntity})"/>
	/// <param name="cancellationToken">The cancellation token to cancel the request.</param>
	/// <returns>The <see cref="Task"/> representing the asynchronous operation.</returns>
	Task CreateAsync(
		IEnumerable<TEntity> entities,
		CancellationToken cancellationToken = default);

	/// <summary>
	/// Deletes the specified entity from the underlying data store.
	/// </summary>
	/// <remarks>
	/// This method marks the provided entity as deleted in the database context, so that
	/// changes to the entity will be persisted to the database during the next save operation.
	/// </remarks>
	/// <param name="entity">The entity to delete.</param>
	void Delete(TEntity entity);

	/// <summary>
	/// Deletes the specified collection of entities from the underlying data store.
	/// </summary>
	/// <remarks>
	/// This method marks the provided entities as deleted in the database context, so that
	/// changes to the entity will be persisted to the database during the next save operation.
	/// </remarks>
	/// <param name="entities">The collection of entities to delete.</param>
	void Delete(IEnumerable<TEntity> entities);

	/// <summary>
	/// Deletes all database rows for the <typeparamref name="TEntity"/> instances which match
	/// the <paramref name="expression"/> from the database.
	/// </summary>
	/// <remarks>
	/// <para>
	/// This operation executes immediately against the database, rather than being deferred
	/// until save changes is called. It also does not interact with the EF change tracker in
	/// any way: entity instances which happen to be tracked when this operation is invoked
	/// aren't taken into account, and aren't updated to reflect the changes.
	/// </para>
	/// <para>
	/// <b>This deletes permanently, even for soft deletable entity types.</b> Because no save
	/// operation takes place, save changes interceptors never run, and the soft deletable
	/// interceptor therefore cannot turn the deletion into a flag update. The rows are gone.
	/// Use <see cref="Delete(TEntity)"/> together with a save operation to soft delete.
	/// </para>
	/// </remarks>
	/// <param name="expression">The condition to fulfill to be deleted.</param>
	/// <returns>The total number of rows deleted in the database.</returns>
	int Delete(Expression<Func<TEntity, bool>> expression);

	/// <inheritdoc cref="Delete(TEntity)"/>
	/// <param name="cancellationToken">The cancellation token to cancel the request.</param>
	/// <returns>The <see cref="Task"/> representing the asynchronous operation.</returns>
	Task DeleteAsync(
		TEntity entity,
		CancellationToken cancellationToken = default);

	/// <inheritdoc cref="Delete(IEnumerable{TEntity})"/>
	/// <param name="cancellationToken">The cancellation token to cancel the request.</param>
	/// <returns>The <see cref="Task"/> representing the asynchronous operation.</returns>
	Task DeleteAsync(
		IEnumerable<TEntity> entities,
		CancellationToken cancellationToken = default);

	/// <inheritdoc cref="Delete(Expression{Func{TEntity, bool}})"/>
	/// <param name="cancellationToken">The cancellation token to cancel the request.</param>
	Task<int> DeleteAsync(
		Expression<Func<TEntity, bool>> expression,
		CancellationToken cancellationToken = default);

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

	/// <summary>
	/// Updates the specified entity in the underlying data store.
	/// </summary>
	/// <remarks>
	/// This method marks the provided entity as modified in the database context. so that
	/// changes to the entity will be persisted to the database during the next save operation.
	/// </remarks>
	/// <param name="entity">The entity to update.</param>
	void Update(TEntity entity);

	/// <summary>
	/// Updates the specified collection of entities in the underlying data store.
	/// </summary>
	/// <remarks>
	/// This method marks the provided entities as modified in the database context. so that
	/// changes to the entities will be persisted to the database during the next save operation.
	/// </remarks>
	/// <param name="entities">The collection of entities to update.</param>
	void Update(IEnumerable<TEntity> entities);

	/// <summary>
	/// Updates all database rows for the <typeparamref name="TEntity"/> instances which match
	/// the <paramref name="expression"/> from the database.
	/// </summary>
	/// <remarks>
	/// <para>
	/// This operation executes immediately against the database, rather than being deferred
	/// until save changes is called. It also does not interact with the EF change tracker in
	/// any way: entity instances which happen to be tracked when this operation is invoked
	/// aren't taken into account, and aren't updated to reflect the changes.
	/// </para>
	/// <para>
	/// <b>Auditing does not happen for this operation.</b> Because no save operation takes
	/// place, save changes interceptors never run, so the audit columns are left untouched
	/// unless <paramref name="setPropertyCalls"/> sets them explicitly. Use
	/// <see cref="Update(TEntity)"/> together with a save operation to have them maintained.
	/// </para>
	/// </remarks>
	/// <param name="expression">The condition to fulfill to be updated.</param>
	/// <param name="setPropertyCalls">A collection of set property statements specifying properties to update.</param>
	/// <returns>The total number of rows updated in the database.</returns>
	int Update(
		Expression<Func<TEntity, bool>> expression,
		Action<UpdateSettersBuilder<TEntity>> setPropertyCalls);

	/// <inheritdoc cref="Update(TEntity)"/>
	/// <param name="cancellationToken">The cancellation token to cancel the request.</param>
	/// <returns>The <see cref="Task"/> representing the asynchronous operation.</returns>
	Task UpdateAsync(
		TEntity entity,
		CancellationToken cancellationToken = default);

	/// <inheritdoc cref="Update(IEnumerable{TEntity})"/>
	/// <param name="cancellationToken">The cancellation token to cancel the request.</param>
	/// <returns>The <see cref="Task"/> representing the asynchronous operation.</returns>
	Task UpdateAsync(
		IEnumerable<TEntity> entities,
		CancellationToken cancellationToken = default);

	/// <inheritdoc cref="Update(Expression{Func{TEntity, bool}}, Action{UpdateSettersBuilder{TEntity}})"/>
	/// <param name="cancellationToken">The cancellation token to cancel the request.</param>
	Task<int> UpdateAsync(
		Expression<Func<TEntity, bool>> expression,
		Action<UpdateSettersBuilder<TEntity>> setPropertyCalls,
		CancellationToken cancellationToken = default);
}
