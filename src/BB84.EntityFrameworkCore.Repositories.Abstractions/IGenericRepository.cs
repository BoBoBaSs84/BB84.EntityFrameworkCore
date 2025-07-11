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
/// This interface provides synchronous and asynchronous methods for creating, reading,
/// updating, and deleting entities, as well as methods for querying entities based on
/// conditions. It is designed to abstract data access logic, making it easier to work
/// with different data sources or implement unit testing.
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
	/// changes to the entities will be persisted to the database during the next save operation.
	/// </remarks>
	/// <param name="entities">The collection of entities to add.</param>
	void Create(IEnumerable<TEntity> entities);

	/// <summary>
	/// Adds the specified entity to the underlying data store.
	/// </summary>
	/// <remarks>
	/// This method marks the provided entity as added in the database context, so that
	/// changes to the entity will be persisted to the database during the next save operation.
	/// </remarks>
	/// <param name="entity">The entity to add.</param>
	/// <param name="token">The cancellation token to cancel the request.</param>
	/// <returns>The <see cref="Task"/> representing the asynchronous operation.</returns>
	Task CreateAsync(TEntity entity, CancellationToken token = default);

	/// <summary>
	/// Adds the specified collection of entities to the underlying data store.
	/// </summary>
	/// <remarks>
	/// This method marks the provided entities as added in the database context. so that
	/// changes to the entities will be persisted to the database during the next save operation.
	/// </remarks>
	/// <param name="entities">The collection of entities to add.</param>
	/// <param name="token">The cancellation token to cancel the request.</param>
	/// <returns>The <see cref="Task"/> representing the asynchronous operation.</returns>
	Task CreateAsync(IEnumerable<TEntity> entities, CancellationToken token = default);

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
	/// This operation executes immediately against the database, rather than being deferred
	/// until save changes is called. It also does not interact with the EF change tracker in
	/// any way: entity instances which happen to be tracked when this operation is invoked
	/// aren't taken into account, and aren't updated to reflect the changes.
	/// </remarks>
	/// <param name="expression">The condition to fulfill to be deleted.</param>
	/// <returns>The total number of rows deleted in the database.</returns>
	int Delete(Expression<Func<TEntity, bool>>? expression);

	/// <summary>
	/// Deletes the specified entity from the underlying data store.
	/// </summary>
	/// <remarks>
	/// This method marks the provided entity as deleted in the database context, so that
	/// changes to the entity will be persisted to the database during the next save operation.
	/// </remarks>
	/// <param name="entity">The entity to delete.</param>
	/// <param name="token">The cancellation token to cancel the request.</param>
	/// <returns>The <see cref="Task"/> representing the asynchronous operation.</returns>
	Task DeleteAsync(TEntity entity, CancellationToken token = default);

	/// <summary>
	/// Deletes the specified collection of entities from the underlying data store.
	/// </summary>
	/// <remarks>
	/// This method marks the provided entities as deleted in the database context, so that
	/// changes to the entity will be persisted to the database during the next save operation.
	/// </remarks>
	/// <param name="entities">The collection of entities to delete.</param>
	/// <param name="token">The cancellation token to cancel the request.</param>
	/// <returns>The <see cref="Task"/> representing the asynchronous operation.</returns>
	Task DeleteAsync(IEnumerable<TEntity> entities, CancellationToken token = default);

	/// <summary>
	/// Deletes all database rows for the <typeparamref name="TEntity"/> instances which match
	/// the <paramref name="expression"/> from the database.
	/// </summary>
	/// <remarks>
	/// This operation executes immediately against the database, rather than being deferred
	/// until save changes is called. It also does not interact with the EF change tracker in
	/// any way: entity instances which happen to be tracked when this operation is invoked
	/// aren't taken into account, and aren't updated to reflect the changes.
	/// </remarks>	
	/// <param name="expression">The condition to fulfill to be deleted.</param>
	/// <param name="token">The cancellation token to cancel the request.</param>
	/// <returns>The total number of rows deleted in the database.</returns>
	Task<int> DeleteAsync(Expression<Func<TEntity, bool>>? expression, CancellationToken token = default);

	/// <summary>
	/// Counts the total number of entities in the data source.
	/// </summary>
	/// <param name="ignoreQueryFilters">
	/// A value indicating whether to ignore any query filters applied to the entity type.
	/// <see langword="true"/> to ignore query filters; otherwise, <see langword="false"/>.
	/// </param>
	/// <returns>The total number of entities in the data source.</returns>
	int CountAll(bool ignoreQueryFilters = false);

	/// <summary>
	/// Counts the number of entities in the data source that satisfy the specified conditions.
	/// </summary>
	/// <remarks>
	/// This method allows for flexible filtering and customization of the query through the
	/// <paramref name="expression"/> and <paramref name="queryFilter"/> parameters.
	/// Use <paramref name="ignoreQueryFilters"/> to bypass global filters  such as soft delete
	/// or multi-tenancy filters.
	/// </remarks>
	/// <param name="expression">
	/// An optional LINQ expression used to filter the entities to be counted.
	/// </param>
	/// <param name="queryFilter">
	/// An optional function to apply additional transformations or filters to the query.
	/// </param>
	/// <param name="ignoreQueryFilters">
	/// A value indicating whether to ignore any global query filters applied to the entity type.
	/// </param>
	/// <returns>The total number of entities that match the specified conditions.</returns>
	int Count(
		Expression<Func<TEntity, bool>>? expression = null,
		Func<IQueryable<TEntity>, IQueryable<TEntity>>? queryFilter = null,
		bool ignoreQueryFilters = false
		);

	/// <summary>
	/// Counts the total number of entities in the data source.
	/// </summary>
	/// <param name="ignoreQueryFilters">
	/// A value indicating whether to ignore any query filters applied to the entity type.
	/// <see langword="true"/> to ignore query filters; otherwise, <see langword="false"/>.
	/// </param>
	/// <param name="token">The cancellation token to cancel the request.</param>
	/// <returns>The total number of entities in the data source.</returns>
	Task<int> CountAllAsync(bool ignoreQueryFilters = false, CancellationToken token = default);

	/// <summary>
	/// Counts the number of entities in the data source that satisfy the specified conditions.
	/// </summary>
	/// <remarks>
	/// This method allows for flexible filtering and customization of the query through the
	/// <paramref name="expression"/> and <paramref name="queryFilter"/> parameters.
	/// Use <paramref name="ignoreQueryFilters"/> to bypass global filters  such as soft delete
	/// or multi-tenancy filters.
	/// </remarks>
	/// <param name="expression">
	/// An optional LINQ expression used to filter the entities to be counted.
	/// </param>
	/// <param name="queryFilter">
	/// An optional function to apply additional transformations or filters to the query.
	/// </param>
	/// <param name="ignoreQueryFilters">
	/// A value indicating whether to ignore any global query filters applied to the entity type.
	/// </param>
	/// <param name="token">The cancellation token to cancel the request.</param>
	/// <returns>The total number of entities that match the specified conditions.</returns>
	Task<int> CountAsync(
		Expression<Func<TEntity, bool>>? expression = null,
		Func<IQueryable<TEntity>, IQueryable<TEntity>>? queryFilter = null,
		bool ignoreQueryFilters = false,
		CancellationToken token = default
		);

	/// <summary>
	/// Retrieves all entities of type <typeparamref name="TEntity"/> from the data source.
	/// </summary>
	/// <remarks>
	/// Use the <paramref name="ignoreQueryFilters"/> parameter to bypass global query filters,
	/// such as soft delete filters, when retrieving entities. The <paramref name="trackChanges"/>
	/// parameter determines whether the returned entities are tracked by the context, which can
	/// impact performance and memory usage.
	/// </remarks>
	/// <param name="ignoreQueryFilters">
	/// A value indicating whether to ignore any query filters applied to the entity type.
	/// </param>
	/// <param name="trackChanges">
	/// A value indicating whether the retrieved entities should be tracked by the context.
	/// </param>
	/// <returns>
	/// An <see cref="IEnumerable{T}"/> containing all entities of type <typeparamref name="TEntity"/>
	/// that match the query criteria.
	/// </returns>
	IEnumerable<TEntity> GetAll(bool ignoreQueryFilters = false, bool trackChanges = false);

	/// <summary>
	/// Retrieves all entities of type <typeparamref name="TEntity"/> from the data source.
	/// </summary>
	/// <remarks>
	/// Use the <paramref name="ignoreQueryFilters"/> parameter to bypass global query filters,
	/// such as soft delete filters, when retrieving entities. The <paramref name="trackChanges"/>
	/// parameter determines whether the returned entities are tracked by the context, which can
	/// impact performance and memory usage.
	/// </remarks>
	/// <param name="ignoreQueryFilters">
	/// A value indicating whether to ignore any query filters applied to the entity type.
	/// </param>
	/// <param name="trackChanges">
	/// A value indicating whether the retrieved entities should be tracked by the context.
	/// </param>
	/// <param name="token">The cancellation token to cancel the request.</param>
	/// <returns>
	/// An <see cref="IEnumerable{T}"/> containing all entities of type <typeparamref name="TEntity"/>
	/// that match the query criteria.
	/// </returns>
	Task<IEnumerable<TEntity>> GetAllAsync(
		bool ignoreQueryFilters = false,
		bool trackChanges = false,
		CancellationToken token = default
		);

	/// <summary>
	/// Returns a collection of <typeparamref name="TEntity"/> based on the specified <paramref name="expression"/>.
	/// </summary>
	/// <param name="expression">The condition to fulfill to be returned.</param>
	/// <param name="queryFilter">The function used to filter the entities.</param>
	/// <param name="ignoreQueryFilters">Should model-level entity query filters be applied?</param>
	/// <param name="orderBy">The function used to order the entities.</param>
	/// <param name="skip">The number of records to skip.</param>
	/// <param name="take">The number of records to limit the results to.</param>
	/// <param name="trackChanges">Should the fetched entities be tracked?</param>
	/// <param name="includeProperties">Any other navigation properties to include when returning the collection.</param>
	/// <returns>A collection of <typeparamref name="TEntity"/>.</returns>
	IEnumerable<TEntity> GetManyByCondition(
		Expression<Func<TEntity, bool>>? expression = null,
		Func<IQueryable<TEntity>, IQueryable<TEntity>>? queryFilter = null,
		bool ignoreQueryFilters = false,
		Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
		int? skip = null,
		int? take = null,
		bool trackChanges = false,
		params string[] includeProperties
		);

	/// <summary>
	/// Returns a collection of <typeparamref name="TEntity"/> based on the specified <paramref name="expression"/>.
	/// </summary>
	/// <param name="expression">The condition to fulfill to be returned.</param>
	/// <param name="queryFilter">The function used to filter the entities.</param>
	/// <param name="ignoreQueryFilters">Should model-level entity query filters be applied?</param>
	/// <param name="orderBy">The function used to order the entities.</param>
	/// <param name="skip">The number of records to skip.</param>
	/// <param name="take">The number of records to limit the results to.</param>
	/// <param name="trackChanges">Should the fetched entities be tracked?</param>
	/// <param name="token">The cancellation token.</param>
	/// <param name="includeProperties">Any other navigation properties to include when returning the collection.</param>
	/// <returns>A collection of <typeparamref name="TEntity"/>.</returns>
	Task<IEnumerable<TEntity>> GetManyByConditionAsync(
		Expression<Func<TEntity, bool>>? expression = null,
		Func<IQueryable<TEntity>, IQueryable<TEntity>>? queryFilter = null,
		bool ignoreQueryFilters = false,
		Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
		int? skip = null,
		int? take = null,
		bool trackChanges = false,
		CancellationToken token = default,
		params string[] includeProperties
		);

	/// <summary>
	/// Returns a <typeparamref name="TEntity"/> by a certain <paramref name="expression"/>.
	/// </summary>
	/// <param name="expression">The search condition.</param>
	/// <param name="queryFilter">The function used to filter the entities.</param>
	/// <param name="ignoreQueryFilters">Should model-level entity query filters be applied?</param>
	/// <param name="trackChanges">Should the fetched entity be tracked?</param>
	/// <param name="includeProperties">Any other navigation properties to include when returning the entity.</param>
	/// <returns>The found <typeparamref name="TEntity"/> or <see langword="null"/>.</returns>
	TEntity? GetByCondition(
		Expression<Func<TEntity, bool>> expression,
		Func<IQueryable<TEntity>, IQueryable<TEntity>>? queryFilter = null,
		bool ignoreQueryFilters = false,
		bool trackChanges = false,
		params string[] includeProperties
		);

	/// <summary>
	/// Returns a <typeparamref name="TEntity"/> by a certain <paramref name="expression"/>.
	/// </summary>
	/// <param name="expression">The search condition.</param>
	/// <param name="queryFilter">The function used to filter the entities.</param>
	/// <param name="ignoreQueryFilters">Should model-level entity query filters be applied?</param>
	/// <param name="trackChanges">Should the fetched entity be tracked?</param>
	/// <param name="token">The cancellation token to cancel the request.</param>
	/// <param name="includeProperties">Any other navigation properties to include when returning the entity.</param>
	/// <returns>The found <typeparamref name="TEntity"/> or <see langword="null"/>.</returns>
	Task<TEntity?> GetByConditionAsync(
		Expression<Func<TEntity, bool>> expression,
		Func<IQueryable<TEntity>, IQueryable<TEntity>>? queryFilter = null,
		bool ignoreQueryFilters = false,
		bool trackChanges = false,
		CancellationToken token = default,
		params string[] includeProperties
		);

	/// <summary>
	/// Updates the specified entity in the underlying data store.
	/// </summary>
	/// <remarks>
	/// This method marks the provided entity as modified in the database context, so that
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
	/// This operation executes immediately against the database, rather than being deferred
	/// until save changes is called. It also does not interact with the EF change tracker in
	/// any way: entity instances which happen to be tracked when this operation is invoked
	/// aren't taken into account, and aren't updated to reflect the changes.
	/// </remarks>	
	/// <param name="expression">The condition to fulfill to be updated.</param>
	/// <param name="setPropertyCalls">A collection of set property statements specifying properties to update.</param>
	/// <returns>The total number of rows updated in the database.</returns>
	int Update(
		Expression<Func<TEntity, bool>> expression,
		Expression<Func<SetPropertyCalls<TEntity>, SetPropertyCalls<TEntity>>> setPropertyCalls
		);

	/// <summary>
	/// Updates the specified entity in the underlying data store.
	/// </summary>
	/// <remarks>
	/// This method marks the provided entity as modified in the database context, so that
	/// changes to the entity will be persisted to the database during the next save operation.
	/// </remarks>
	/// <param name="entity">The entity to update.</param>
	/// <param name="token">The cancellation token to cancel the request.</param>
	/// <returns>The <see cref="Task"/> representing the asynchronous operation.</returns>
	Task UpdateAsync(TEntity entity, CancellationToken token = default);

	/// <summary>
	/// Updates the specified collection of entities in the underlying data store.
	/// </summary>
	/// <remarks>
	/// This method marks the provided entities as modified in the database context. so that
	/// changes to the entities will be persisted to the database during the next save operation.
	/// </remarks>
	/// <param name="entities">The collection of entities to update.</param>
	/// <param name="token">The cancellation token to cancel the request.</param>
	/// <returns>The <see cref="Task"/> representing the asynchronous operation.</returns>
	Task UpdateAsync(IEnumerable<TEntity> entities, CancellationToken token = default);

	/// <summary>
	/// Updates all database rows for the <typeparamref name="TEntity"/> instances which match
	/// the <paramref name="expression"/> from the database.
	/// </summary>
	/// <remarks>
	/// This operation executes immediately against the database, rather than being deferred
	/// until save changes is called. It also does not interact with the EF change tracker in
	/// any way: entity instances which happen to be tracked when this operation is invoked
	/// aren't taken into account, and aren't updated to reflect the changes.
	/// </remarks>	
	/// <param name="expression">The condition to fulfill to be updated.</param>
	/// <param name="setPropertyCalls">A collection of set property statements specifying properties to update.</param>
	/// <param name="token">The cancellation token to cancel the request.</param>
	/// <returns>The total number of rows updated in the database.</returns>
	Task<int> UpdateAsync(
		Expression<Func<TEntity, bool>> expression,
		Expression<Func<SetPropertyCalls<TEntity>, SetPropertyCalls<TEntity>>> setPropertyCalls,
		CancellationToken token = default
		);
}
