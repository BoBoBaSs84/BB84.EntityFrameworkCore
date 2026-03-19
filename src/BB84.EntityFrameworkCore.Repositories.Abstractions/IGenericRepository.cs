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
	Task CreateAsync(
		TEntity entity,
		CancellationToken token = default);

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
	Task CreateAsync(
		IEnumerable<TEntity> entities,
		CancellationToken token = default);

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
	Task DeleteAsync(
		TEntity entity,
		CancellationToken token = default);

	/// <summary>
	/// Deletes the specified collection of entities from the underlying data store.
	/// </summary>
	/// <remarks>
	/// This method marks the provided entities as deleted in the database context, so that
	/// changes to the entities will be persisted to the database during the next save operation.
	/// </remarks>
	/// <param name="entities">The collection of entities to delete.</param>
	/// <param name="token">The cancellation token to cancel the request.</param>
	/// <returns>The <see cref="Task"/> representing the asynchronous operation.</returns>
	Task DeleteAsync(
		IEnumerable<TEntity> entities,
		CancellationToken token = default);

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
	Task<int> DeleteAsync(
		Expression<Func<TEntity, bool>>? expression,
		CancellationToken token = default);

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
	int CountByCondition(
		Expression<Func<TEntity, bool>> expression,
		Func<IQueryable<TEntity>, IQueryable<TEntity>>? queryFilter = null,
		bool ignoreQueryFilters = false);

	/// <summary>
	/// Counts the total number of entities in the data source.
	/// </summary>
	/// <param name="ignoreQueryFilters">
	/// A value indicating whether to ignore any query filters applied to the entity type.
	/// <see langword="true"/> to ignore query filters; otherwise, <see langword="false"/>.
	/// </param>
	/// <param name="token">The cancellation token to cancel the request.</param>
	/// <returns>The total number of entities in the data source.</returns>
	Task<int> CountAllAsync(
		bool ignoreQueryFilters = false,
		CancellationToken token = default);

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
	Task<int> CountByConditionAsync(
		Expression<Func<TEntity, bool>> expression,
		Func<IQueryable<TEntity>, IQueryable<TEntity>>? queryFilter = null,
		bool ignoreQueryFilters = false,
		CancellationToken token = default);

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
	/// An <see cref="IReadOnlyList{T}"/> containing all entities of type <typeparamref name="TEntity"/>
	/// that match the query criteria.
	/// </returns>
	IReadOnlyList<TEntity> GetAll(
		bool ignoreQueryFilters = false,
		bool trackChanges = false);

	/// <summary>
	/// Retrieves all entities of type <typeparamref name="TEntity"/> from the data source and projects them
	/// into a different form using the specified <paramref name="selector"/>. The optional <paramref name="fieldSelector"/>
	/// is used to specify which fields to include in the projection, and the <paramref name="ignoreQueryFilters"/> parameter
	/// allows bypassing global query filters when retrieving entities.
	/// </summary>
	/// <typeparam name="TResult">
	/// The type to which the entities should be projected. This can be a DTO, an anonymous type, or any other type that can
	/// be constructed from the properties of <typeparamref name="TEntity"/>.
	/// </typeparam>
	/// <param name="selector">The expression used to project the entities into the desired form.</param>
	/// <param name="fieldSelector">The optional expression used to specify which fields to include in the projection.</param>
	/// <param name="ignoreQueryFilters">
	/// A value indicating whether to ignore any query filters applied to the entity type when retrieving entities for projection.
	/// </param>
	/// <returns>
	/// A collection of <typeparamref name="TResult"/> containing the projected entities based on the specified selector and field selector.
	/// </returns>
	IReadOnlyList<TResult> GetAll<TResult>(
		Expression<Func<TEntity, TResult>> selector,
		Expression<Func<TResult, TResult>>? fieldSelector = null,
		bool ignoreQueryFilters = false);

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
	/// An <see cref="IReadOnlyList{T}"/> containing all entities of type <typeparamref name="TEntity"/>
	/// that match the query criteria.
	/// </returns>
	Task<IReadOnlyList<TEntity>> GetAllAsync(
		bool ignoreQueryFilters = false,
		bool trackChanges = false,
		CancellationToken token = default);

	/// <summary>
	/// Retrieves all entities of type <typeparamref name="TEntity"/> from the data source and projects them
	/// into a different form using the specified <paramref name="selector"/>. The optional <paramref name="fieldSelector"/>
	/// is used to specify which fields to include in the projection, and the <paramref name="ignoreQueryFilters"/> parameter
	/// allows bypassing global query filters when retrieving entities.
	/// </summary>
	/// <typeparam name="TResult">
	/// The type to which the entities should be projected. This can be a DTO, an anonymous type, or any other type that can
	/// be constructed from the properties of <typeparamref name="TEntity"/>.
	/// </typeparam>
	/// <param name="selector">The expression used to project the entities into the desired form.</param>
	/// <param name="fieldSelector">The optional expression used to specify which fields to include in the projection.</param>
	/// <param name="ignoreQueryFilters">
	/// A value indicating whether to ignore any query filters applied to the entity type when retrieving entities for projection.
	/// </param>
	/// <param name="token">The cancellation token to cancel the request.</param>
	/// <returns>
	/// A collection of <typeparamref name="TResult"/> containing the projected entities based on the specified selector and field selector.
	/// </returns>
	Task<IReadOnlyList<TResult>> GetAllAsync<TResult>(
		Expression<Func<TEntity, TResult>> selector,
		Expression<Func<TResult, TResult>>? fieldSelector = null,
		bool ignoreQueryFilters = false,
		CancellationToken token = default);

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
		params string[] includeProperties);

	/// <summary>
	/// Retrieves a single result of type <typeparamref name="TResult"/> from entities that satisfy the
	/// specified condition.
	/// </summary>
	/// <remarks>
	/// If multiple entities match the specified condition, only the first result is returned. This method
	/// can be used to efficiently retrieve a single value or projection from a filtered set of entities.
	/// The behavior of global query filters can be controlled using the <paramref name="ignoreQueryFilters"/>
	/// parameter.
	/// </remarks>
	/// <typeparam name="TResult">The type of the result to project from the entity.</typeparam>
	/// <param name="expression">An expression that defines the condition to filter entities of type <typeparamref name="TEntity"/>.</param>
	/// <param name="selector">An expression that specifies how to project the filtered entity to a result of type <typeparamref name="TResult"/>.</param>
	/// <param name="fieldSelector">An optional expression to further select or transform fields from the projected result.</param>
	/// <param name="queryFilter">An optional function to apply additional query operations, such as sorting or including related entities, to the
	/// filtered set.</param>
	/// <param name="ignoreQueryFilters">true to ignore any global query filters applied to the entity type; otherwise, false.</param>
	/// <returns>
	/// The projected result of type <typeparamref name="TResult"/> if an entity matching the condition is found; otherwise, null.
	/// </returns>
	TResult? GetByCondition<TResult>(
		Expression<Func<TEntity, bool>> expression,
		Expression<Func<TEntity, TResult>> selector,
		Expression<Func<TResult, TResult>>? fieldSelector = null,
		Func<IQueryable<TEntity>, IQueryable<TEntity>>? queryFilter = null,
		bool ignoreQueryFilters = false);

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
		params string[] includeProperties);

	/// <summary>
	/// Asynchronously retrieves a single result projected from entities that satisfy the specified condition.
	/// </summary>
	/// <remarks>
	/// If multiple entities match the specified condition, only the first matching result is returned.
	/// This method is typically used to retrieve a single value or object based on a filter and projection.
	/// </remarks>
	/// <typeparam name="TResult">The type of the result to project and return.</typeparam>
	/// <param name="expression">An expression that defines the condition to filter entities of type TEntity.</param>
	/// <param name="selector">An expression that specifies how to project the filtered entity to the result type TResult.</param>
	/// <param name="fieldSelector">
	/// An optional expression to further select or shape the projected result. If null, the entire projected result is returned.
	/// </param>
	/// <param name="queryFilter">
	/// An optional function to apply additional query transformations, such as sorting or including related data, before executing the query.
	/// </param>
	/// <param name="ignoreQueryFilters">true to ignore any global query filters applied to the entity type; otherwise, false.</param>
	/// <param name="token">A CancellationToken that can be used to cancel the asynchronous operation.</param>
	/// <returns>
	/// A task that represents the asynchronous operation. The task result contains the projected result if a matching entity is found;
	/// otherwise, null.
	/// </returns>
	Task<TResult?> GetByConditionAsync<TResult>(
		Expression<Func<TEntity, bool>> expression,
		Expression<Func<TEntity, TResult>> selector,
		Expression<Func<TResult, TResult>>? fieldSelector = null,
		Func<IQueryable<TEntity>, IQueryable<TEntity>>? queryFilter = null,
		bool ignoreQueryFilters = false,
		CancellationToken token = default);

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
	IReadOnlyList<TEntity> GetManyByCondition(
		Expression<Func<TEntity, bool>> expression,
		Func<IQueryable<TEntity>, IQueryable<TEntity>>? queryFilter = null,
		bool ignoreQueryFilters = false,
		Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
		int? skip = null,
		int? take = null,
		bool trackChanges = false,
		params string[] includeProperties);

	/// <summary>
	/// Retrieves a collection of entities that satisfy the specified condition, projects them to the specified result
	/// type, and applies optional filtering, ordering, and pagination.
	/// </summary>
	/// <remarks>
	/// Use this method to retrieve and project multiple entities based on complex query scenarios, including custom
	/// filtering, ordering, and pagination. This method is typically used in repository patterns to abstract data access logic.
	/// </remarks>
	/// <typeparam name="TResult">The type of the result returned by the selector expression.</typeparam>
	/// <param name="expression">An expression that defines the condition entities must satisfy to be included in the result.</param>
	/// <param name="selector">An expression that specifies how to project each matching entity to the result type.</param>
	/// <param name="fieldSelector">An optional expression that selects specific fields from the projected result.</param>
	/// <param name="queryFilter">An optional function to apply additional filtering or transformation to the query before execution.</param>
	/// <param name="ignoreQueryFilters">true to ignore any global query filters applied to the entity type; otherwise, false.</param>
	/// <param name="orderBy">An optional function to specify the ordering of the results.</param>
	/// <param name="skip">The number of results to skip before returning results. If null, no results are skipped.</param>
	/// <param name="take">The maximum number of results to return. If null, all matching results are returned.</param>
	/// <returns>
	/// An enumerable collection of projected results that match the specified condition and query options.
	/// The collection may be empty if no entities satisfy the condition.
	/// </returns>
	IReadOnlyList<TResult> GetManyByCondition<TResult>(
		Expression<Func<TEntity, bool>> expression,
		Expression<Func<TEntity, TResult>> selector,
		Expression<Func<TResult, TResult>>? fieldSelector = null,
		Func<IQueryable<TEntity>, IQueryable<TEntity>>? queryFilter = null,
		bool ignoreQueryFilters = false,
		Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
		int? skip = null,
		int? take = null);

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
	Task<IReadOnlyList<TEntity>> GetManyByConditionAsync(
		Expression<Func<TEntity, bool>> expression,
		Func<IQueryable<TEntity>, IQueryable<TEntity>>? queryFilter = null,
		bool ignoreQueryFilters = false,
		Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
		int? skip = null,
		int? take = null,
		bool trackChanges = false,
		CancellationToken token = default,
		params string[] includeProperties);

	/// <summary>
	/// Asynchronously retrieves a collection of entities that satisfy the specified condition, projects them to the
	/// specified result type, and applies optional filtering, ordering, and pagination.
	/// </summary>
	/// <remarks>
	/// This method supports advanced querying scenarios, including projection, filtering, ordering, and pagination.
	/// Use the optional parameters to customize the query as needed. When trackChanges is set to true, the returned
	/// entities are tracked by the underlying context, which may impact performance and memory usage.
	/// </remarks>
	/// <typeparam name="TResult">The type to which the entities are projected in the result set.</typeparam>
	/// <param name="expression">An expression that defines the condition used to filter entities. Only entities matching this condition are
	/// included in the result.</param>
	/// <param name="selector">An expression that specifies how to project each entity into the result type.</param>
	/// <param name="fieldSelector">An optional expression that selects specific fields from the projected result.</param>
	/// <param name="queryFilter">An optional function that applies additional filtering or transformation to the query before execution.</param>
	/// <param name="ignoreQueryFilters">true to ignore any global query filters applied to the entity type; otherwise, false.</param>
	/// <param name="orderBy">An optional function that defines the ordering of the result set. If null, the default ordering is used.</param>
	/// <param name="skip">The number of results to skip before returning results. If null, no results are skipped.</param>
	/// <param name="take">The maximum number of results to return. If null, all matching results are returned.</param>
	/// <param name="token">A cancellation token that can be used to cancel the asynchronous operation.</param>
	/// <returns>
	/// A task that represents the asynchronous operation. The task result contains an enumerable collection of
	/// projected results that match the specified condition. The collection is empty if no entities match.
	/// </returns>
	Task<IReadOnlyList<TResult>> GetManyByConditionAsync<TResult>(
		Expression<Func<TEntity, bool>> expression,
		Expression<Func<TEntity, TResult>> selector,
		Expression<Func<TResult, TResult>>? fieldSelector = null,
		Func<IQueryable<TEntity>, IQueryable<TEntity>>? queryFilter = null,
		bool ignoreQueryFilters = false,
		Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
		int? skip = null,
		int? take = null,
		CancellationToken token = default);

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
#if NET8_0
		Expression<Func<SetPropertyCalls<TEntity>, SetPropertyCalls<TEntity>>> setPropertyCalls);
#else
		Action<UpdateSettersBuilder<TEntity>> setPropertyCalls);
#endif

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
	Task UpdateAsync(
		TEntity entity,
		CancellationToken token = default);

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
	Task UpdateAsync(
		IEnumerable<TEntity> entities,
		CancellationToken token = default);

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
#if NET8_0
		Expression<Func<SetPropertyCalls<TEntity>, SetPropertyCalls<TEntity>>> setPropertyCalls,
#else
		Action<UpdateSettersBuilder<TEntity>> setPropertyCalls,
#endif
		CancellationToken token = default);
}
