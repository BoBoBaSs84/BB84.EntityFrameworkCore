// Copyright: 2024 Robert Peter Meyer
// License: MIT
//
// This source code is licensed under the MIT license found in the
// LICENSE file in the root directory of this source tree.
using System.Linq.Expressions;

using BB84.EntityFrameworkCore.Entities.Abstractions;

using Microsoft.EntityFrameworkCore.Query;

namespace BB84.EntityFrameworkCore.Repositories.Abstractions;

/// <summary>
/// Defines a repository interface for managing entities of type <see cref="IIdentityEntity{TKey}"/>
/// with a primary key of type <typeparamref name="TKey"/>.
/// </summary>
/// <remarks>
/// This interface extends <see cref="IGenericRepository{TEntity}"/> and provides additional
/// methods for CRUD operations specifically tailored to entities with identity-based primary
/// keys.
/// </remarks>
/// <typeparam name="TEntity">The type of the entity managed by the repository.</typeparam>
/// <typeparam name="TKey">The type of the primary key for the entity.</typeparam>
public interface IIdentityRepository<TEntity, TKey> : IGenericRepository<TEntity>
	where TEntity : class, IIdentityEntity<TKey>
	where TKey : IEquatable<TKey>
{
	/// <summary>
	/// Deletes the database row for the <typeparamref name="TEntity"/> instance which matches
	/// the <paramref name="id"/> from the database.
	/// </summary>
	/// <remarks>
	/// This operation executes immediately against the database, rather than being deferred
	/// until save changes is called. It also does not interact with the EF change tracker in
	/// any way: entity instances which happen to be tracked when this operation is invoked
	/// aren't taken into account, and aren't updated to reflect the changes.
	/// </remarks>	
	/// <param name="id">The primary key of the <typeparamref name="TEntity"/>.</param>
	/// <returns>The total number of rows deleted in the database.</returns>
	int Delete(TKey id);

	/// <summary>
	/// Deletes all database rows for the <typeparamref name="TEntity"/> instances which matches
	/// the <paramref name="ids"/> from the database.
	/// </summary>
	/// <remarks>
	/// This operation executes immediately against the database, rather than being deferred
	/// until save changes is called. It also does not interact with the EF change tracker in
	/// any way: entity instances which happen to be tracked when this operation is invoked
	/// aren't taken into account, and aren't updated to reflect the changes.
	/// </remarks>	
	/// <param name="ids">The primary keys of the <typeparamref name="TEntity"/>.</param>
	/// <returns>The total number of rows deleted in the database.</returns>
	int Delete(IEnumerable<TKey> ids);

	/// <summary>
	/// Deletes the database row for the <typeparamref name="TEntity"/> instance which matches
	/// the <paramref name="id"/> from the database.
	/// </summary>
	/// <remarks>
	/// This operation executes immediately against the database, rather than being deferred
	/// until save changes is called. It also does not interact with the EF change tracker in
	/// any way: entity instances which happen to be tracked when this operation is invoked
	/// aren't taken into account, and aren't updated to reflect the changes.
	/// </remarks>
	/// <param name="id">The primary key of the <typeparamref name="TEntity"/>.</param>
	/// <param name="token">The cancellation token to cancel the request.</param>
	/// <returns>The total number of rows deleted in the database.</returns>
	Task<int> DeleteAsync(TKey id, CancellationToken token = default);

	/// <summary>
	/// Deletes all database rows for the <typeparamref name="TEntity"/> instances which matches
	/// the <paramref name="ids"/> from the database.
	/// </summary>
	/// <remarks>
	/// This operation executes immediately against the database, rather than being deferred
	/// until save changes is called. It also does not interact with the EF change tracker in
	/// any way: entity instances which happen to be tracked when this operation is invoked
	/// aren't taken into account, and aren't updated to reflect the changes.
	/// </remarks>	
	/// <param name="ids">The primary keys of the <typeparamref name="TEntity"/>.</param>
	/// <param name="token">The cancellation token to cancel the request.</param>
	/// <returns>The total number of rows deleted in the database.</returns>
	Task<int> DeleteAsync(IEnumerable<TKey> ids, CancellationToken token = default);

	/// <summary>
	/// Retrieves an entity by its unique identifier.
	/// </summary>
	/// <param name="id">The unique identifier of the entity to retrieve.</param>
	/// <param name="ignoreQueryFilters">
	/// A value indicating whether to ignore query filters, such as global filters or soft delete filters.
	/// </param>
	/// <param name="trackChanges">
	/// A value indicating whether the retrieved entity should be tracked by the context.
	/// </param>
	/// <param name="includeProperties">
	/// An array of related entity property names to include in the query.
	/// </param>
	/// <returns>
	/// The entity that matches the specified identifier, or <see langword="null"/> if no such entity exists.
	/// </returns>
	TEntity? GetById(
		TKey id,
		bool ignoreQueryFilters = false,
		bool trackChanges = false,
		params string[] includeProperties
		);

	/// <summary>
	/// Retrieves an entity by its unique identifier.
	/// </summary>
	/// <param name="id">The unique identifier of the entity to retrieve.</param>
	/// <param name="ignoreQueryFilters">
	/// A value indicating whether to ignore query filters, such as global filters or soft delete filters.
	/// </param>
	/// <param name="trackChanges">
	/// A value indicating whether the retrieved entity should be tracked by the context.
	/// </param>
	/// <param name="token">The cancellation token to cancel the request.</param>
	/// <param name="includeProperties">
	/// An array of related entity property names to include in the query.
	/// </param>
	/// <returns>
	/// The entity of type <typeparamref name="TEntity"/> that matches the specified identifier,
	/// or <see langword="null"/> if no such entity exists.
	/// </returns>
	Task<TEntity?> GetByIdAsync(
		TKey id,
		bool ignoreQueryFilters = false,
		bool trackChanges = false,
		CancellationToken token = default,
		params string[] includeProperties
		);

	/// <summary>
	/// Retrieves a collection of entities that match the specified identifiers.
	/// </summary>
	/// <param name="ids">A collection of identifiers used to filter the entities.</param>
	/// <param name="ignoreQueryFilters">
	/// A value indicating whether to ignore any query filters applied to the entity type.
	/// </param>
	/// <param name="trackChanges">
	/// A value indicating whether the returned entities should be tracked by the context.
	/// </param>
	/// <param name="includeProperties">
	/// An array of related entity property names to include in the query results.
	/// </param>
	/// <returns>
	/// A collection of entities of type <typeparamref name="TEntity"/> that match the specified identifiers.
	/// If no entities match, an empty collection is returned.
	/// </returns>
	IEnumerable<TEntity> GetByIds(
		IEnumerable<TKey> ids,
		bool ignoreQueryFilters = false,
		bool trackChanges = false,
		params string[] includeProperties
		);

	/// <summary>
	/// Retrieves a collection of entities that match the specified identifiers.
	/// </summary>
	/// <param name="ids">A collection of identifiers used to filter the entities.</param>
	/// <param name="ignoreQueryFilters">
	/// A value indicating whether to ignore any query filters applied to the entity type.
	/// </param>
	/// <param name="trackChanges">
	/// A value indicating whether the returned entities should be tracked by the context.
	/// </param>
	/// <param name="token">The cancellation token to cancel the request.</param>
	/// <param name="includeProperties">
	/// An array of related entity property names to include in the query results.
	/// </param>
	/// <returns>
	/// A collection of entities of type <typeparamref name="TEntity"/> that match the specified identifiers.
	/// If no entities match, an empty collection is returned.
	/// </returns>
	Task<IEnumerable<TEntity>> GetByIdsAsync(
		IEnumerable<TKey> ids,
		bool ignoreQueryFilters = false,
		bool trackChanges = false,
		CancellationToken token = default,
		params string[] includeProperties
		);

	/// <summary>
	/// Updates the entity identified by the specified identifier with the provided property changes.
	/// </summary>
	/// <remarks>
	/// This operation executes immediately against the database, rather than being deferred
	/// until save changes is called. It also does not interact with the EF change tracker in
	/// any way: entity instances which happen to be tracked when this operation is invoked
	/// aren't taken into account, and aren't updated to reflect the changes.
	/// </remarks>	
	/// <param name="id">The unique identifier of the entity to update.</param>
	/// <param name="setPropertyCalls">A lambda expression specifying the properties to update and their new values.</param>
	/// <returns>
	/// The number of entities updated. Typically, this will be 1 if the update is successful,
	/// or 0 if no entity matches the specified <paramref name="id"/>.
	/// </returns>
	int Update(
		TKey id,
		Expression<Func<SetPropertyCalls<TEntity>, SetPropertyCalls<TEntity>>> setPropertyCalls
		);

	/// <summary>
	/// Updates the entities identified by the specified identifiers with the provided property changes.
	/// </summary>
	/// <remarks>
	/// This operation executes immediately against the database, rather than being deferred
	/// until save changes is called. It also does not interact with the EF change tracker in
	/// any way: entity instances which happen to be tracked when this operation is invoked
	/// aren't taken into account, and aren't updated to reflect the changes.
	/// </remarks>	
	/// <param name="ids">The unique identifiers of the entities to update.</param>
	/// <param name="setPropertyCalls">A lambda expression specifying the properties to update and their new values.</param>
	/// <returns>
	/// The number of entities updated. Typically, this will be 1 if the update is successful,
	/// or 0 if no entity matches the specified <paramref name="ids"/>.
	/// </returns>
	int Update(
		IEnumerable<TKey> ids,
		Expression<Func<SetPropertyCalls<TEntity>, SetPropertyCalls<TEntity>>> setPropertyCalls
		);

	/// <summary>
	/// Updates the entity identified by the specified identifier with the provided property changes.
	/// </summary>
	/// <remarks>
	/// This operation executes immediately against the database, rather than being deferred
	/// until save changes is called. It also does not interact with the EF change tracker in
	/// any way: entity instances which happen to be tracked when this operation is invoked
	/// aren't taken into account, and aren't updated to reflect the changes.
	/// </remarks>	
	/// <param name="id">The unique identifier of the entity to update.</param>
	/// <param name="setPropertyCalls">A lambda expression specifying the properties to update and their new values.</param>
	/// <param name="token">The cancellation token to cancel the request.</param>
	/// <returns>
	/// The number of entities updated. Typically, this will be 1 if the update is successful, or 0 if no entity matches
	/// the specified <paramref name="id"/>.
	/// </returns>
	Task<int> UpdateAsync(
		TKey id,
		Expression<Func<SetPropertyCalls<TEntity>, SetPropertyCalls<TEntity>>> setPropertyCalls,
		CancellationToken token = default
		);

	/// <summary>
	/// Updates the entities identified by the specified identifiers with the provided property changes.
	/// </summary>
	/// <remarks>
	/// This operation executes immediately against the database, rather than being deferred
	/// until save changes is called. It also does not interact with the EF change tracker in
	/// any way: entity instances which happen to be tracked when this operation is invoked
	/// aren't taken into account, and aren't updated to reflect the changes.
	/// </remarks>	
	/// <param name="ids">The unique identifiers of the entities to update.</param>
	/// <param name="setPropertyCalls">A lambda expression specifying the properties to update and their new values.</param>
	/// <param name="token">The cancellation token to cancel the request.</param>
	/// <returns>
	/// The number of entities updated. Typically, this will be 1 if the update is successful,
	/// or 0 if no entity matches the specified <paramref name="ids"/>.
	/// </returns>
	Task<int> UpdateAsync(
		IEnumerable<TKey> ids,
		Expression<Func<SetPropertyCalls<TEntity>, SetPropertyCalls<TEntity>>> setPropertyCalls,
		CancellationToken token = default
		);
}

/// <inheritdoc cref="IIdentityRepository{TEntity, TKey}"/>
public interface IIdentityRepository<TEntity> : IIdentityRepository<TEntity, Guid>
	where TEntity : class, IIdentityEntity
{ }
