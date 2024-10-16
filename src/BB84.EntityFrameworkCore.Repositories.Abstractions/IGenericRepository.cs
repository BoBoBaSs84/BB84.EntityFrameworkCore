using System.Linq.Expressions;

using Microsoft.EntityFrameworkCore.Query;

namespace BB84.EntityFrameworkCore.Repositories.Abstractions;

/// <summary>
/// The generic repository interface.
/// </summary>
/// <typeparam name="TEntity">The entity to work with.</typeparam>
public interface IGenericRepository<TEntity> where TEntity : class
{
	/// <summary>
	/// Creates a new database row for the <typeparamref name="TEntity"/> instance.
	/// </summary>
	/// <param name="entity">The <typeparamref name="TEntity"/> to create.</param>
	void Create(TEntity entity);

	/// <summary>
	/// Creates multiple new database rows for the <typeparamref name="TEntity"/> instance.
	/// </summary>
	/// <param name="entities">The collection of <typeparamref name="TEntity"/> to create.</param>
	void Create(IEnumerable<TEntity> entities);

	/// <summary>
	/// Creates a new database row for the <typeparamref name="TEntity"/> instance.
	/// </summary>
	/// <param name="entity">The <typeparamref name="TEntity"/> to create.</param>
	/// <param name="token">The cancellation token to cancel the request.</param>
	/// <returns><see cref="Task"/></returns>
	Task CreateAsync(TEntity entity, CancellationToken token = default);

	/// <summary>
	/// Creates multiple new database rows for the <typeparamref name="TEntity"/> instance.
	/// </summary>
	/// <param name="entities">The collection of <typeparamref name="TEntity"/> to create.</param>
	/// <param name="token">The cancellation token to cancel the request.</param>
	/// <returns><see cref="Task"/></returns>
	Task CreateAsync(IEnumerable<TEntity> entities, CancellationToken token = default);

	/// <summary>
	/// Deletes an existing database row for the <typeparamref name="TEntity"/> instance.
	/// </summary>
	/// <param name="entity">The <typeparamref name="TEntity"/> to delete.</param>
	void Delete(TEntity entity);

	/// <summary>
	/// Deletes multiple existing database rows for the <typeparamref name="TEntity"/> instance.
	/// </summary>
	/// <param name="entities">The collection of <typeparamref name="TEntity"/> to delete.</param>
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
	/// Deletes an existing database row for the <typeparamref name="TEntity"/> instance.
	/// </summary>
	/// <param name="entity">The <typeparamref name="TEntity"/> to delete.</param>
	/// <param name="token">The cancellation token to cancel the request.</param>
	/// <returns><see cref="Task"/></returns>
	Task DeleteAsync(TEntity entity, CancellationToken token = default);

	/// <summary>
	/// Deletes multiple existing database rows for the <typeparamref name="TEntity"/> instance.
	/// </summary>
	/// <param name="entities">The collection of <typeparamref name="TEntity"/> to delete.</param>
	/// <param name="token">The cancellation token to cancel the request.</param>
	/// <returns><see cref="Task"/></returns>
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
	/// Returns the total number of <typeparamref name="TEntity"/>.
	/// </summary>
	/// <param name="ignoreQueryFilters">Should model-level entity query filters be applied?</param>
	/// <returns>The total number of <typeparamref name="TEntity"/>.</returns>
	int CountAll(bool ignoreQueryFilters = false);

	/// <summary>
	/// Returns the number of <typeparamref name="TEntity"/> based on the specified <paramref name="expression"/>.
	/// </summary>
	/// <param name="expression">The condition to fulfill to be counted.</param>
	/// <param name="queryFilter">The function used to filter the entities.</param>
	/// <param name="ignoreQueryFilters">Should model-level entity query filters be applied?</param>
	/// <returns>The number of <typeparamref name="TEntity"/>.</returns>
	int Count(
		Expression<Func<TEntity, bool>>? expression = null,
		Func<IQueryable<TEntity>, IQueryable<TEntity>>? queryFilter = null,
		bool ignoreQueryFilters = false
		);

	/// <summary>
	/// Returns the total number of <typeparamref name="TEntity"/>.
	/// </summary>
	/// <param name="ignoreQueryFilters">Should model-level entity query filters be applied?</param>
	/// <param name="token">The cancellation token to cancel the request.</param>
	/// <returns>The total number of <typeparamref name="TEntity"/>.</returns>
	Task<int> CountAllAsync(bool ignoreQueryFilters = false, CancellationToken token = default);

	/// <summary>
	/// Returns the number of <typeparamref name="TEntity"/> based on the specified <paramref name="expression"/>.
	/// </summary>
	/// <param name="expression">The condition to fulfill to be counted.</param>
	/// <param name="queryFilter">The function used to filter the entities.</param>
	/// <param name="ignoreQueryFilters">Should model-level entity query filters be applied?</param>
	/// <param name="token">The cancellation token to cancel the request.</param>
	/// <returns>The number of <typeparamref name="TEntity"/>.</returns>
	Task<int> CountAsync(
		Expression<Func<TEntity, bool>>? expression = null,
		Func<IQueryable<TEntity>, IQueryable<TEntity>>? queryFilter = null,
		bool ignoreQueryFilters = false,
		CancellationToken token = default
		);

	/// <summary>
	/// Returns a collection of all <typeparamref name="TEntity"/>.
	/// </summary>
	/// <param name="ignoreQueryFilters">Should model-level entity query filters be applied?</param>
	/// <param name="trackChanges">Should the fetched entities be tracked?</param>
	/// <returns>A colection of <typeparamref name="TEntity"/>.</returns>
	IEnumerable<TEntity> GetAll(bool ignoreQueryFilters = false, bool trackChanges = false);

	/// <summary>
	/// Returns a collection of all <typeparamref name="TEntity"/>.
	/// </summary>
	/// <param name="ignoreQueryFilters">Should model-level entity query filters be applied?</param>
	/// <param name="trackChanges">Should the fetched entities be tracked?</param>
	/// <param name="token">The cancellation token to cancel the request.</param>
	/// <returns>A colection of <typeparamref name="TEntity"/>.</returns>
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
	/// Updates an existing database row for the <typeparamref name="TEntity"/> instance.
	/// </summary>
	/// <param name="entity">The <typeparamref name="TEntity"/> to update.</param>
	void Update(TEntity entity);

	/// <summary>
	/// Updates multiple existing database rows for the <typeparamref name="TEntity"/> instance.
	/// </summary>
	/// <param name="entities">The collection of <typeparamref name="TEntity"/> to update.</param>
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
	/// Updates an existing database row for the <typeparamref name="TEntity"/> instance.
	/// </summary>
	/// <param name="entity">The <typeparamref name="TEntity"/> to update.</param>
	/// <param name="token">The cancellation token to cancel the request.</param>
	/// <returns><see cref="Task"/></returns>
	Task UpdateAsync(TEntity entity, CancellationToken token = default);

	/// <summary>
	/// Updates multiple existing database rows for the <typeparamref name="TEntity"/> instance.
	/// </summary>
	/// <param name="entities">The collection of <typeparamref name="TEntity"/> to update.</param>
	/// <param name="token">The cancellation token to cancel the request.</param>
	/// <returns><see cref="Task"/></returns>
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
