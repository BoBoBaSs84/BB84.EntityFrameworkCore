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
		params string[] includeProperties);

	/// <summary>
	/// Retrieves a projection of an entity identified by the specified key.
	/// </summary>
	/// <remarks>
	/// Use <paramref name="ignoreQueryFilters"/> with caution, as ignoring query filters may expose entities that
	/// are normally excluded (such as soft-deleted records or tenant-specific data). The <paramref name="fieldSelector"/>
	/// parameter allows for additional shaping of the result after the initial projection.
	/// </remarks>
	/// <typeparam name="TResult">The type of the result returned by the selector expression.</typeparam>
	/// <param name="id">The unique identifier of the entity to retrieve.</param>
	/// <param name="selector">
	/// An expression that defines the projection to apply to the entity. This determines which fields are included in the result.
	/// </param>
	/// <param name="fieldSelector">
	/// An optional expression to further select or transform the projected result. If null, the entire projection defined by
	/// <paramref name="selector"/> is returned.
	/// </param>
	/// <param name="ignoreQueryFilters">true to ignore any global query filters applied to the entity type; otherwise, false.</param>
	/// <returns>
	/// The projected result of type <typeparamref name="TResult"/> if an entity with the specified identifier exists; otherwise, null.
	/// </returns>
	TResult? GetById<TResult>(
		TKey id,
		Expression<Func<TEntity, TResult>> selector,
		Expression<Func<TResult, TResult>>? fieldSelector = null,
		bool ignoreQueryFilters = false);

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
		params string[] includeProperties);

	/// <summary>
	/// Asynchronously retrieves an entity by its identifier and projects it to a specified result type.
	/// </summary>
	/// <remarks>
	/// Use this method to efficiently retrieve and project a single entity by its key, minimizing data transfer by
	/// selecting only required fields. If the entity is not found, the result is null. When ignoreQueryFilters is
	/// set to true, any global query filters (such as soft-delete or multi-tenancy filters) are bypassed for this query.
	/// </remarks>
	/// <typeparam name="TResult">The type of the result returned by the selector expression.</typeparam>
	/// <param name="id">The unique identifier of the entity to retrieve.</param>
	/// <param name="selector">
	/// An expression that defines the projection to apply to the entity. This determines which fields are included in the result.
	/// </param>
	/// <param name="fieldSelector">
	/// An optional expression to further select or transform the projected result. If null, the entire projection defined by
	/// <paramref name="selector"/> is returned.
	/// </param>
	/// <param name="ignoreQueryFilters">true to ignore any global query filters applied to the entity type; otherwise, false.</param>
	/// <param name="token">A cancellation token that can be used to cancel the asynchronous operation.</param>
	/// <returns>
	/// A task that represents the asynchronous operation. The task result contains the projected entity if found; otherwise, null.
	/// </returns>
	Task<TResult?> GetByIdAsync<TResult>(
		TKey id,
		Expression<Func<TEntity, TResult>> selector,
		Expression<Func<TResult, TResult>>? fieldSelector = null,
		bool ignoreQueryFilters = false,
		CancellationToken token = default);

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
	/// A read only collection of entities of type <typeparamref name="TEntity"/> that match the
	/// specified identifiers. If no entities match, an empty collection is returned.
	/// </returns>
	IReadOnlyList<TEntity> GetByIds(
		IEnumerable<TKey> ids,
		bool ignoreQueryFilters = false,
		bool trackChanges = false,
		params string[] includeProperties);

	/// <summary>
	/// Retrieves entities by their identifiers and projects them into the specified result type.
	/// </summary>
	/// <remarks>
	/// The order of the returned results is not guaranteed to match the order of the provided identifiers.
	/// If an identifier does not correspond to an existing entity, it is omitted from the results. This
	/// method is typically used to efficiently fetch and project multiple entities in a single query.
	/// </remarks>
	/// <typeparam name="TResult">The type of the result returned for each entity.</typeparam>
	/// <param name="ids">A collection of entity identifiers to retrieve.</param>
	/// <param name="selector">An expression that defines the projection from the entity to the result type.</param>
	/// <param name="fieldSelector">
	/// An optional expression to further select or shape the projected result. If null, the full result from the selector is returned.
	/// </param>
	/// <param name="ignoreQueryFilters">true to ignore any global query filters applied to the entity type; otherwise, false.</param>
	/// <returns>
	/// An read only collection of projected results of <typeparamref name="TResult"/> corresponding to the specified identifiers.
	/// The collection may be empty if no entities are found.
	/// </returns>
	IReadOnlyList<TResult> GetByIds<TResult>(
		IEnumerable<TKey> ids,
		Expression<Func<TEntity, TResult>> selector,
		Expression<Func<TResult, TResult>>? fieldSelector = null,
		bool ignoreQueryFilters = false);

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
	/// A read only collection of entities of type <typeparamref name="TEntity"/> that match the
	/// specified identifiers. If no entities match, an empty collection is returned.
	/// </returns>
	Task<IReadOnlyList<TEntity>> GetByIdsAsync(
		IEnumerable<TKey> ids,
		bool ignoreQueryFilters = false,
		bool trackChanges = false,
		CancellationToken token = default,
		params string[] includeProperties);

	/// <summary>
	/// Asynchronously retrieves entities by their identifiers and projects them into the specified result type.
	/// </summary>
	/// <remarks>
	/// The order of the returned results is not guaranteed to match the order of the provided identifiers.
	/// If an identifier does not correspond to an existing entity, it is omitted from the result.
	/// </remarks>
	/// <typeparam name="TResult">The type to which each entity is projected.</typeparam>
	/// <param name="ids">A collection of entity identifiers to retrieve.</param>
	/// <param name="selector">An expression that defines the projection from the entity to the result type.</param>
	/// <param name="fieldSelector">An optional expression to further select or shape the projected result.</param>
	/// <param name="ignoreQueryFilters">true to ignore any global query filters applied to the entity type; otherwise, false.</param>
	/// <param name="token">A cancellation token that can be used to cancel the asynchronous operation.</param>
	/// <returns>
	/// A task that represents the asynchronous operation. The task result contains a read-only list of projected results
	/// corresponding to the provided identifiers. The list may be empty if no entities are found.
	/// </returns>
	Task<IReadOnlyList<TResult>> GetByIdsAsync<TResult>(
		IEnumerable<TKey> ids,
		Expression<Func<TEntity, TResult>> selector,
		Expression<Func<TResult, TResult>>? fieldSelector = null,
		bool ignoreQueryFilters = false,
		CancellationToken token = default);

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
#if NET8_0
		Expression<Func<SetPropertyCalls<TEntity>, SetPropertyCalls<TEntity>>> setPropertyCalls);
#else
		Action<UpdateSettersBuilder<TEntity>> setPropertyCalls);
#endif

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
#if NET8_0
		Expression<Func<SetPropertyCalls<TEntity>, SetPropertyCalls<TEntity>>> setPropertyCalls);
#else
		Action<UpdateSettersBuilder<TEntity>> setPropertyCalls);
#endif

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
#if NET8_0
		Expression<Func<SetPropertyCalls<TEntity>, SetPropertyCalls<TEntity>>> setPropertyCalls,
#else
		Action<UpdateSettersBuilder<TEntity>> setPropertyCalls,
#endif
		CancellationToken token = default);

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
#if NET8_0
		Expression<Func<SetPropertyCalls<TEntity>, SetPropertyCalls<TEntity>>> setPropertyCalls,
#else
		Action<UpdateSettersBuilder<TEntity>> setPropertyCalls,
#endif
		CancellationToken token = default);
}

/// <inheritdoc cref="IIdentityRepository{TEntity, TKey}"/>
public interface IIdentityRepository<TEntity> : IIdentityRepository<TEntity, Guid>
	where TEntity : class, IIdentityEntity
{ }
