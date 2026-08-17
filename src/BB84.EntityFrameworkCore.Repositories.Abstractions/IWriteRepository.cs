// Copyright: 2024 Robert Peter Meyer
// License: MIT
//
// This source code is licensed under the MIT license found in the
// LICENSE file in the root directory of this source tree.
using System.Linq.Expressions;

using Microsoft.EntityFrameworkCore.Query;

namespace BB84.EntityFrameworkCore.Repositories.Abstractions;

/// <summary>
/// Defines the write half of a repository for entities of type <typeparamref name="TEntity"/>.
/// </summary>
/// <remarks>
/// <para>
/// The entity based methods stage their change in the change tracker and take effect on the
/// next save operation, which is the caller's responsibility. The <c>Execute</c> prefixed
/// methods execute immediately and bypass the change tracker entirely, so no interceptor runs
/// for them.
/// </para>
/// <para>
/// Depend on this rather than on <see cref="IGenericRepository{TEntity}"/> wherever a component
/// only writes, such as an importer or the command side of a CQRS split.
/// </para>
/// </remarks>
/// <typeparam name="TEntity">
/// The type of the entity for which the repository provides write access.
/// </typeparam>
public interface IWriteRepository<TEntity>
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
	int ExecuteDelete(Expression<Func<TEntity, bool>> expression);

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

	/// <inheritdoc cref="ExecuteDelete(Expression{Func{TEntity, bool}})"/>
	/// <param name="cancellationToken">The cancellation token to cancel the request.</param>
	Task<int> ExecuteDeleteAsync(
		Expression<Func<TEntity, bool>> expression,
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
	int ExecuteUpdate(
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

	/// <inheritdoc cref="ExecuteUpdate(Expression{Func{TEntity, bool}}, Action{UpdateSettersBuilder{TEntity}})"/>
	/// <param name="cancellationToken">The cancellation token to cancel the request.</param>
	Task<int> ExecuteUpdateAsync(
		Expression<Func<TEntity, bool>> expression,
		Action<UpdateSettersBuilder<TEntity>> setPropertyCalls,
		CancellationToken cancellationToken = default);
}
