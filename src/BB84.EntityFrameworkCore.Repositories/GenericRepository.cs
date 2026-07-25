// Copyright: 2024 Robert Peter Meyer
// License: MIT
//
// This source code is licensed under the MIT license found in the
// LICENSE file in the root directory of this source tree.
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

using BB84.EntityFrameworkCore.Repositories.Abstractions;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace BB84.EntityFrameworkCore.Repositories;

/// <summary>
/// The generic repository implementation.
/// </summary>
/// <inheritdoc cref="IGenericRepository{TEntity}"/>
/// <param name="dbContext">The database context to work with.</param>
public abstract class GenericRepository<TEntity>(IDbContext dbContext) : IGenericRepository<TEntity>
	where TEntity : class
{
	/// <summary>
	/// The collection of all <typeparamref name="TEntity"/> within the database context.
	/// </summary>
	protected DbSet<TEntity> DbSet
		=> dbContext.Set<TEntity>();

	/// <inheritdoc/>
	public void Create(TEntity entity)
		=> DbSet.Add(entity);

	/// <inheritdoc/>
	public void Create(IEnumerable<TEntity> entities)
		=> DbSet.AddRange(entities);

	/// <inheritdoc/>
	public async Task CreateAsync(TEntity entity, CancellationToken token = default)
		=> await DbSet.AddAsync(entity, token).ConfigureAwait(false);

	/// <inheritdoc/>
	public async Task CreateAsync(IEnumerable<TEntity> entities, CancellationToken token = default)
		=> await DbSet.AddRangeAsync(entities, token).ConfigureAwait(false);

	/// <inheritdoc/>
	public int CountAll(bool ignoreQueryFilters = false)
		=> QueryCount(ignoreQueryFilters: ignoreQueryFilters);

	/// <inheritdoc/>
	public async Task<int> CountAllAsync(bool ignoreQueryFilters = false, CancellationToken token = default)
		=> await QueryCountAsync(ignoreQueryFilters: ignoreQueryFilters, token: token).ConfigureAwait(false);

	/// <inheritdoc/>
	public int CountByCondition(Func<IQueryable<TEntity>, IQueryable<TEntity>> queryFilter, bool ignoreQueryFilters = false)
		=> QueryCount(queryFilter: queryFilter, ignoreQueryFilters: ignoreQueryFilters);

	/// <inheritdoc/>
	public int CountByCondition(Expression<Func<TEntity, bool>> expression, Func<IQueryable<TEntity>, IQueryable<TEntity>>? queryFilter = null, bool ignoreQueryFilters = false)
		=> QueryCount(expression: expression, queryFilter: queryFilter, ignoreQueryFilters: ignoreQueryFilters);

	/// <inheritdoc/>
	public async Task<int> CountByConditionAsync(Func<IQueryable<TEntity>, IQueryable<TEntity>> queryFilter, bool ignoreQueryFilters = false, CancellationToken token = default)
		=> await QueryCountAsync(queryFilter: queryFilter, ignoreQueryFilters: ignoreQueryFilters, token: token).ConfigureAwait(false);

	/// <inheritdoc/>
	public async Task<int> CountByConditionAsync(Expression<Func<TEntity, bool>> expression, Func<IQueryable<TEntity>, IQueryable<TEntity>>? queryFilter = null, bool ignoreQueryFilters = false, CancellationToken token = default)
		=> await QueryCountAsync(expression: expression, queryFilter: queryFilter, ignoreQueryFilters: ignoreQueryFilters, token: token).ConfigureAwait(false);

	/// <inheritdoc/>
	public void Delete(TEntity entity)
		=> DbSet.Remove(entity);

	/// <inheritdoc/>
	public void Delete(IEnumerable<TEntity> entities)
		=> DbSet.RemoveRange(entities);

	/// <inheritdoc/>
	public int Delete(Expression<Func<TEntity, bool>> expression)
		=> PrepareQuery(expression).ExecuteDelete();

	/// <inheritdoc/>
	public Task DeleteAsync(TEntity entity, CancellationToken token = default)
	{
		if (token.IsCancellationRequested)
			return Task.FromCanceled(token);

		DbSet.Remove(entity);

		return Task.CompletedTask;
	}

	/// <inheritdoc/>
	public Task DeleteAsync(IEnumerable<TEntity> entities, CancellationToken token = default)
	{
		if (token.IsCancellationRequested)
			return Task.FromCanceled(token);

		DbSet.RemoveRange(entities);

		return Task.CompletedTask;
	}

	/// <inheritdoc/>
	public async Task<int> DeleteAsync(Expression<Func<TEntity, bool>> expression, CancellationToken token = default)
		=> await PrepareQuery(expression).ExecuteDeleteAsync(token).ConfigureAwait(false);

	/// <inheritdoc/>
	public IReadOnlyList<TEntity> GetAll(bool ignoreQueryFilters = false, bool trackChanges = false)
		=> QueryMany(ignoreQueryFilters: ignoreQueryFilters, trackChanges: trackChanges);

	/// <inheritdoc/>
	public IReadOnlyList<TResult> GetAll<TResult>(Expression<Func<TEntity, TResult>> selector, Expression<Func<TResult, TResult>>? fieldSelector = null, bool ignoreQueryFilters = false)
		=> QueryMany(selector: selector, fieldSelector: fieldSelector, ignoreQueryFilters: ignoreQueryFilters);

	/// <inheritdoc/>
	public async Task<IReadOnlyList<TEntity>> GetAllAsync(bool ignoreQueryFilters = false, bool trackChanges = false, CancellationToken token = default)
		=> await QueryManyAsync(ignoreQueryFilters: ignoreQueryFilters, trackChanges: trackChanges, token: token).ConfigureAwait(false);

	/// <inheritdoc/>
	public async Task<IReadOnlyList<TResult>> GetAllAsync<TResult>(Expression<Func<TEntity, TResult>> selector, Expression<Func<TResult, TResult>>? fieldSelector = null, bool ignoreQueryFilters = false, CancellationToken token = default)
		=> await QueryManyAsync(selector: selector, fieldSelector: fieldSelector, ignoreQueryFilters: ignoreQueryFilters, token: token).ConfigureAwait(false);

	/// <inheritdoc/>
	public TEntity? GetByCondition(Func<IQueryable<TEntity>, IQueryable<TEntity>> queryFilter, bool ignoreQueryFilters = false, bool trackChanges = false, params string[] includeProperties)
		=> QuerySingle(queryFilter: queryFilter, ignoreQueryFilters: ignoreQueryFilters, trackChanges: trackChanges, includeProperties: includeProperties);

	/// <inheritdoc/>
	public TEntity? GetByCondition(Expression<Func<TEntity, bool>> expression, Func<IQueryable<TEntity>, IQueryable<TEntity>>? queryFilter = null, bool ignoreQueryFilters = false, bool trackChanges = false, params string[] includeProperties)
		=> QuerySingle(expression: expression, queryFilter: queryFilter, ignoreQueryFilters: ignoreQueryFilters, trackChanges: trackChanges, includeProperties: includeProperties);

	/// <inheritdoc/>
	public TResult? GetByCondition<TResult>(Expression<Func<TEntity, bool>> expression, Expression<Func<TEntity, TResult>> selector, Expression<Func<TResult, TResult>>? fieldSelector = null, Func<IQueryable<TEntity>, IQueryable<TEntity>>? queryFilter = null, bool ignoreQueryFilters = false)
		=> QuerySingle(selector: selector, fieldSelector: fieldSelector, expression: expression, queryFilter: queryFilter, ignoreQueryFilters: ignoreQueryFilters);

	/// <inheritdoc/>
	public async Task<TEntity?> GetByConditionAsync(Func<IQueryable<TEntity>, IQueryable<TEntity>> queryFilter, bool ignoreQueryFilters = false, bool trackChanges = false, CancellationToken token = default, params string[] includeProperties)
		=> await QuerySingleAsync(queryFilter: queryFilter, ignoreQueryFilters: ignoreQueryFilters, trackChanges: trackChanges, token: token, includeProperties: includeProperties).ConfigureAwait(false);

	/// <inheritdoc/>
	public async Task<TEntity?> GetByConditionAsync(Expression<Func<TEntity, bool>> expression, Func<IQueryable<TEntity>, IQueryable<TEntity>>? queryFilter = null, bool ignoreQueryFilters = false, bool trackChanges = false, CancellationToken token = default, params string[] includeProperties)
		=> await QuerySingleAsync(expression: expression, queryFilter: queryFilter, ignoreQueryFilters: ignoreQueryFilters, trackChanges: trackChanges, token: token, includeProperties: includeProperties).ConfigureAwait(false);

	/// <inheritdoc/>
	public async Task<TResult?> GetByConditionAsync<TResult>(Expression<Func<TEntity, bool>> expression, Expression<Func<TEntity, TResult>> selector, Expression<Func<TResult, TResult>>? fieldSelector = null, Func<IQueryable<TEntity>, IQueryable<TEntity>>? queryFilter = null, bool ignoreQueryFilters = false, CancellationToken token = default)
		=> await QuerySingleAsync(selector: selector, fieldSelector: fieldSelector, expression: expression, queryFilter: queryFilter, ignoreQueryFilters: ignoreQueryFilters, token: token).ConfigureAwait(false);

	/// <inheritdoc/>
	public IReadOnlyList<TEntity> GetManyByCondition(Func<IQueryable<TEntity>, IQueryable<TEntity>> queryFilter, bool ignoreQueryFilters = false, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null, int? skip = null, int? take = null, bool trackChanges = false, params string[] includeProperties)
		=> QueryMany(queryFilter: queryFilter, ignoreQueryFilters: ignoreQueryFilters, orderBy: orderBy, skip: skip, take: take, trackChanges: trackChanges, includeProperties: includeProperties);

	/// <inheritdoc/>
	public IReadOnlyList<TEntity> GetManyByCondition(Expression<Func<TEntity, bool>> expression, Func<IQueryable<TEntity>, IQueryable<TEntity>>? queryFilter = null, bool ignoreQueryFilters = false, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null, int? skip = null, int? take = null, bool trackChanges = false, params string[] includeProperties)
		=> QueryMany(expression: expression, queryFilter: queryFilter, ignoreQueryFilters: ignoreQueryFilters, orderBy: orderBy, skip: skip, take: take, trackChanges: trackChanges, includeProperties: includeProperties);

	/// <inheritdoc/>
	public IReadOnlyList<TResult> GetManyByCondition<TResult>(Expression<Func<TEntity, bool>> expression, Expression<Func<TEntity, TResult>> selector, Expression<Func<TResult, TResult>>? fieldSelector = null, Func<IQueryable<TEntity>, IQueryable<TEntity>>? queryFilter = null, bool ignoreQueryFilters = false, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null, int? skip = null, int? take = null)
		=> QueryMany(selector: selector, fieldSelector: fieldSelector, expression: expression, queryFilter: queryFilter, ignoreQueryFilters: ignoreQueryFilters, orderBy: orderBy, skip: skip, take: take);

	/// <inheritdoc/>
	public async Task<IReadOnlyList<TEntity>> GetManyByConditionAsync(Func<IQueryable<TEntity>, IQueryable<TEntity>> queryFilter, bool ignoreQueryFilters = false, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null, int? skip = null, int? take = null, bool trackChanges = false, CancellationToken token = default, params string[] includeProperties)
		=> await QueryManyAsync(queryFilter: queryFilter, ignoreQueryFilters: ignoreQueryFilters, orderBy: orderBy, skip: skip, take: take, trackChanges: trackChanges, token: token, includeProperties: includeProperties).ConfigureAwait(false);

	/// <inheritdoc/>
	public async Task<IReadOnlyList<TEntity>> GetManyByConditionAsync(Expression<Func<TEntity, bool>> expression, Func<IQueryable<TEntity>, IQueryable<TEntity>>? queryFilter = null, bool ignoreQueryFilters = false, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null, int? skip = null, int? take = null, bool trackChanges = false, CancellationToken token = default, params string[] includeProperties)
		=> await QueryManyAsync(expression: expression, queryFilter: queryFilter, ignoreQueryFilters: ignoreQueryFilters, orderBy: orderBy, skip: skip, take: take, trackChanges: trackChanges, token: token, includeProperties: includeProperties).ConfigureAwait(false);

	/// <inheritdoc/>
	public async Task<IReadOnlyList<TResult>> GetManyByConditionAsync<TResult>(Expression<Func<TEntity, bool>> expression, Expression<Func<TEntity, TResult>> selector, Expression<Func<TResult, TResult>>? fieldSelector = null, Func<IQueryable<TEntity>, IQueryable<TEntity>>? queryFilter = null, bool ignoreQueryFilters = false, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null, int? skip = null, int? take = null, CancellationToken token = default)
		=> await QueryManyAsync(selector: selector, fieldSelector: fieldSelector, expression: expression, queryFilter: queryFilter, ignoreQueryFilters: ignoreQueryFilters, orderBy: orderBy, skip: skip, take: take, token: token).ConfigureAwait(false);

	/// <inheritdoc/>
	public IAsyncEnumerable<TEntity> StreamAll(bool ignoreQueryFilters = false, bool trackChanges = false, CancellationToken token = default)
		=> QueryManyStream(ignoreQueryFilters: ignoreQueryFilters, trackChanges: trackChanges, token: token);

	/// <inheritdoc/>
	public IAsyncEnumerable<TResult> StreamAll<TResult>(Expression<Func<TEntity, TResult>> selector, Expression<Func<TResult, TResult>>? fieldSelector = null, bool ignoreQueryFilters = false, CancellationToken token = default)
		=> QueryManyStream(selector: selector, fieldSelector: fieldSelector, ignoreQueryFilters: ignoreQueryFilters, token: token);

	/// <inheritdoc/>
	public IAsyncEnumerable<TEntity> StreamByCondition(Func<IQueryable<TEntity>, IQueryable<TEntity>> queryFilter, bool ignoreQueryFilters = false, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null, int? skip = null, int? take = null, bool trackChanges = false, CancellationToken token = default, params string[] includeProperties)
		=> QueryManyStream(queryFilter: queryFilter, ignoreQueryFilters: ignoreQueryFilters, orderBy: orderBy, skip: skip, take: take, trackChanges: trackChanges, token: token, includeProperties: includeProperties);

	/// <inheritdoc/>
	public IAsyncEnumerable<TEntity> StreamByCondition(Expression<Func<TEntity, bool>> expression, Func<IQueryable<TEntity>, IQueryable<TEntity>>? queryFilter = null, bool ignoreQueryFilters = false, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null, int? skip = null, int? take = null, bool trackChanges = false, CancellationToken token = default, params string[] includeProperties)
		=> QueryManyStream(expression: expression, queryFilter: queryFilter, ignoreQueryFilters: ignoreQueryFilters, orderBy: orderBy, skip: skip, take: take, trackChanges: trackChanges, token: token, includeProperties: includeProperties);

	/// <inheritdoc/>
	public IAsyncEnumerable<TResult> StreamByCondition<TResult>(Expression<Func<TEntity, bool>> expression, Expression<Func<TEntity, TResult>> selector, Expression<Func<TResult, TResult>>? fieldSelector = null, Func<IQueryable<TEntity>, IQueryable<TEntity>>? queryFilter = null, bool ignoreQueryFilters = false, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null, int? skip = null, int? take = null, CancellationToken token = default)
		=> QueryManyStream(selector: selector, fieldSelector: fieldSelector, expression: expression, queryFilter: queryFilter, ignoreQueryFilters: ignoreQueryFilters, orderBy: orderBy, skip: skip, take: take, token: token);

	/// <inheritdoc/>
	public void Update(TEntity entity)
		=> DbSet.Update(entity);

	/// <inheritdoc/>
	public void Update(IEnumerable<TEntity> entities)
		=> DbSet.UpdateRange(entities);

	/// <inheritdoc/>
	public int Update(
		Expression<Func<TEntity, bool>> expression,
#if NET8_0
		Expression<Func<SetPropertyCalls<TEntity>, SetPropertyCalls<TEntity>>> setPropertyCalls)
#else
		Action<UpdateSettersBuilder<TEntity>> setPropertyCalls)
#endif
		=> PrepareQuery(expression).ExecuteUpdate(setPropertyCalls);

	/// <inheritdoc/>
	public Task UpdateAsync(TEntity entity, CancellationToken token = default)
	{
		if (token.IsCancellationRequested)
			return Task.FromCanceled(token);

		DbSet.Update(entity);

		return Task.CompletedTask;
	}

	/// <inheritdoc/>
	public Task UpdateAsync(IEnumerable<TEntity> entities, CancellationToken token = default)
	{
		if (token.IsCancellationRequested)
			return Task.FromCanceled(token);

		DbSet.UpdateRange(entities);

		return Task.CompletedTask;
	}

	/// <inheritdoc/>
	public async Task<int> UpdateAsync(
		Expression<Func<TEntity, bool>> expression,
#if NET8_0
		Expression<Func<SetPropertyCalls<TEntity>, SetPropertyCalls<TEntity>>> setPropertyCalls,
#else
		Action<UpdateSettersBuilder<TEntity>> setPropertyCalls,
#endif
		CancellationToken token = default)
		=> await PrepareQuery(expression).ExecuteUpdateAsync(setPropertyCalls, token).ConfigureAwait(false);

	/// <summary>
	/// Counts the <typeparamref name="TEntity"/> instances that match the provided criteria.
	/// </summary>
	/// <param name="expression">The condition to fulfill to be counted.</param>
	/// <param name="queryFilter">The function used to filter the entities.</param>
	/// <param name="ignoreQueryFilters">Should model-level entity query filters be applied?</param>
	/// <returns>The total number of matching <typeparamref name="TEntity"/> instances.</returns>
	protected int QueryCount(
		Expression<Func<TEntity, bool>>? expression = null,
		Func<IQueryable<TEntity>, IQueryable<TEntity>>? queryFilter = null,
		bool ignoreQueryFilters = false)
	{
		IQueryable<TEntity> query = PrepareQuery(
			expression: expression,
			queryFilter: queryFilter,
			ignoreQueryFilters: ignoreQueryFilters
			);

		return query.Count();
	}

	/// <inheritdoc cref="QueryCount"/>
	/// <param name="token">The cancellation token to cancel the request.</param>
	protected async Task<int> QueryCountAsync(
		Expression<Func<TEntity, bool>>? expression = null,
		Func<IQueryable<TEntity>, IQueryable<TEntity>>? queryFilter = null,
		bool ignoreQueryFilters = false,
		CancellationToken token = default)
	{
		IQueryable<TEntity> query = PrepareQuery(
			expression: expression,
			queryFilter: queryFilter,
			ignoreQueryFilters: ignoreQueryFilters
			);

		return await query.CountAsync(token)
			.ConfigureAwait(false);
	}

	/// <summary>
	/// Returns the single <typeparamref name="TEntity"/> that matches the provided criteria.
	/// </summary>
	/// <param name="expression">The condition to fulfill to be returned.</param>
	/// <param name="queryFilter">The function used to filter the entities.</param>
	/// <param name="ignoreQueryFilters">Should model-level entity query filters be applied?</param>
	/// <param name="trackChanges">Should the fetched entity be tracked?</param>
	/// <param name="includeProperties">Any other navigation properties to include when returning the entity.</param>
	/// <returns>The found <typeparamref name="TEntity"/> or <see langword="null"/>.</returns>
	protected TEntity? QuerySingle(
		Expression<Func<TEntity, bool>>? expression = null,
		Func<IQueryable<TEntity>, IQueryable<TEntity>>? queryFilter = null,
		bool ignoreQueryFilters = false,
		bool trackChanges = false,
		params string[] includeProperties)
	{
		IQueryable<TEntity> query = PrepareQuery(
			expression: expression,
			queryFilter: queryFilter,
			ignoreQueryFilters: ignoreQueryFilters,
			trackChanges: trackChanges,
			includeProperties: includeProperties
			);

		return query.SingleOrDefault();
	}

	/// <inheritdoc cref="QuerySingle(Expression{Func{TEntity, bool}}, Func{IQueryable{TEntity}, IQueryable{TEntity}}, bool, bool, string[])"/>
	/// <param name="token">The cancellation token to cancel the request.</param>
	protected async Task<TEntity?> QuerySingleAsync(
		Expression<Func<TEntity, bool>>? expression = null,
		Func<IQueryable<TEntity>, IQueryable<TEntity>>? queryFilter = null,
		bool ignoreQueryFilters = false,
		bool trackChanges = false,
		CancellationToken token = default,
		params string[] includeProperties)
	{
		IQueryable<TEntity> query = PrepareQuery(
			expression: expression,
			queryFilter: queryFilter,
			ignoreQueryFilters: ignoreQueryFilters,
			trackChanges: trackChanges,
			includeProperties: includeProperties
			);

		return await query.SingleOrDefaultAsync(token)
			.ConfigureAwait(false);
	}

	/// <summary>
	/// Returns the single projection of type <typeparamref name="TResult"/> that matches the provided criteria.
	/// </summary>
	/// <typeparam name="TResult">The type of the result elements after projection.</typeparam>
	/// <param name="selector">The expression that defines the projection from the entity to the result type.</param>
	/// <param name="fieldSelector">The optional expression that further projects the result type.</param>
	/// <param name="expression">The condition to fulfill to be returned.</param>
	/// <param name="queryFilter">The function used to filter the entities.</param>
	/// <param name="ignoreQueryFilters">Should model-level entity query filters be applied?</param>
	/// <returns>The projected <typeparamref name="TResult"/> or <see langword="null"/>.</returns>
	protected TResult? QuerySingle<TResult>(
		Expression<Func<TEntity, TResult>> selector,
		Expression<Func<TResult, TResult>>? fieldSelector = null,
		Expression<Func<TEntity, bool>>? expression = null,
		Func<IQueryable<TEntity>, IQueryable<TEntity>>? queryFilter = null,
		bool ignoreQueryFilters = false)
	{
		IQueryable<TEntity> query = PrepareQuery(
			expression: expression,
			queryFilter: queryFilter,
			ignoreQueryFilters: ignoreQueryFilters
			);

		return ApplyProjection(query, selector, fieldSelector)
			.SingleOrDefault();
	}

	/// <inheritdoc cref="QuerySingle{TResult}(Expression{Func{TEntity, TResult}}, Expression{Func{TResult, TResult}}, Expression{Func{TEntity, bool}}, Func{IQueryable{TEntity}, IQueryable{TEntity}}, bool)"/>
	/// <param name="token">The cancellation token to cancel the request.</param>
	protected async Task<TResult?> QuerySingleAsync<TResult>(
		Expression<Func<TEntity, TResult>> selector,
		Expression<Func<TResult, TResult>>? fieldSelector = null,
		Expression<Func<TEntity, bool>>? expression = null,
		Func<IQueryable<TEntity>, IQueryable<TEntity>>? queryFilter = null,
		bool ignoreQueryFilters = false,
		CancellationToken token = default)
	{
		IQueryable<TEntity> query = PrepareQuery(
			expression: expression,
			queryFilter: queryFilter,
			ignoreQueryFilters: ignoreQueryFilters
			);

		return await ApplyProjection(query, selector, fieldSelector)
			.SingleOrDefaultAsync(token)
			.ConfigureAwait(false);
	}

	/// <summary>
	/// Returns the collection of <typeparamref name="TEntity"/> that matches the provided criteria.
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
	protected IReadOnlyList<TEntity> QueryMany(
		Expression<Func<TEntity, bool>>? expression = null,
		Func<IQueryable<TEntity>, IQueryable<TEntity>>? queryFilter = null,
		bool ignoreQueryFilters = false,
		Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
		int? skip = null,
		int? take = null,
		bool trackChanges = false,
		params string[] includeProperties)
	{
		IQueryable<TEntity> query = PrepareQuery(
			expression: expression,
			queryFilter: queryFilter,
			ignoreQueryFilters: ignoreQueryFilters,
			orderBy: orderBy,
			skip: skip,
			take: take,
			trackChanges: trackChanges,
			includeProperties: includeProperties
			);

		return [.. query];
	}

	/// <inheritdoc cref="QueryMany(Expression{Func{TEntity, bool}}, Func{IQueryable{TEntity}, IQueryable{TEntity}}, bool, Func{IQueryable{TEntity}, IOrderedQueryable{TEntity}}, int?, int?, bool, string[])"/>
	/// <param name="token">The cancellation token to cancel the request.</param>
	protected async Task<IReadOnlyList<TEntity>> QueryManyAsync(
		Expression<Func<TEntity, bool>>? expression = null,
		Func<IQueryable<TEntity>, IQueryable<TEntity>>? queryFilter = null,
		bool ignoreQueryFilters = false,
		Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
		int? skip = null,
		int? take = null,
		bool trackChanges = false,
		CancellationToken token = default,
		params string[] includeProperties)
	{
		IQueryable<TEntity> query = PrepareQuery(
			expression: expression,
			queryFilter: queryFilter,
			ignoreQueryFilters: ignoreQueryFilters,
			orderBy: orderBy,
			skip: skip,
			take: take,
			trackChanges: trackChanges,
			includeProperties: includeProperties
			);

		return await query.ToListAsync(token)
			.ConfigureAwait(false);
	}

	/// <summary>
	/// Returns the collection of <typeparamref name="TResult"/> projections that match the provided criteria.
	/// </summary>
	/// <typeparam name="TResult">The type of the result elements after projection.</typeparam>
	/// <param name="selector">The expression that defines the projection from the entity to the result type.</param>
	/// <param name="fieldSelector">The optional expression that further projects the result type.</param>
	/// <param name="expression">The condition to fulfill to be returned.</param>
	/// <param name="queryFilter">The function used to filter the entities.</param>
	/// <param name="ignoreQueryFilters">Should model-level entity query filters be applied?</param>
	/// <param name="orderBy">The function used to order the entities.</param>
	/// <param name="skip">The number of records to skip.</param>
	/// <param name="take">The number of records to limit the results to.</param>
	/// <returns>A collection of <typeparamref name="TResult"/>.</returns>
	protected IReadOnlyList<TResult> QueryMany<TResult>(
		Expression<Func<TEntity, TResult>> selector,
		Expression<Func<TResult, TResult>>? fieldSelector = null,
		Expression<Func<TEntity, bool>>? expression = null,
		Func<IQueryable<TEntity>, IQueryable<TEntity>>? queryFilter = null,
		bool ignoreQueryFilters = false,
		Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
		int? skip = null,
		int? take = null)
	{
		IQueryable<TEntity> query = PrepareQuery(
			expression: expression,
			queryFilter: queryFilter,
			ignoreQueryFilters: ignoreQueryFilters,
			orderBy: orderBy,
			skip: skip,
			take: take
			);

		return [.. ApplyProjection(query, selector, fieldSelector)];
	}

	/// <inheritdoc cref="QueryMany{TResult}(Expression{Func{TEntity, TResult}}, Expression{Func{TResult, TResult}}, Expression{Func{TEntity, bool}}, Func{IQueryable{TEntity}, IQueryable{TEntity}}, bool, Func{IQueryable{TEntity}, IOrderedQueryable{TEntity}}, int?, int?)"/>
	/// <param name="token">The cancellation token to cancel the request.</param>
	protected async Task<IReadOnlyList<TResult>> QueryManyAsync<TResult>(
		Expression<Func<TEntity, TResult>> selector,
		Expression<Func<TResult, TResult>>? fieldSelector = null,
		Expression<Func<TEntity, bool>>? expression = null,
		Func<IQueryable<TEntity>, IQueryable<TEntity>>? queryFilter = null,
		bool ignoreQueryFilters = false,
		Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
		int? skip = null,
		int? take = null,
		CancellationToken token = default)
	{
		IQueryable<TEntity> query = PrepareQuery(
			expression: expression,
			queryFilter: queryFilter,
			ignoreQueryFilters: ignoreQueryFilters,
			orderBy: orderBy,
			skip: skip,
			take: take
			);

		return await ApplyProjection(query, selector, fieldSelector)
			.ToListAsync(token)
			.ConfigureAwait(false);
	}

	/// <summary>
	/// Streams the collection of <typeparamref name="TEntity"/> that matches the provided criteria.
	/// </summary>
	/// <remarks>
	/// The result set is not buffered, the entities are yielded as they are read from the database.
	/// The returned sequence is lazy and must be enumerated within the lifetime of the underlying
	/// database context.
	/// </remarks>
	/// <param name="expression">The condition to fulfill to be returned.</param>
	/// <param name="queryFilter">The function used to filter the entities.</param>
	/// <param name="ignoreQueryFilters">Should model-level entity query filters be applied?</param>
	/// <param name="orderBy">The function used to order the entities.</param>
	/// <param name="skip">The number of records to skip.</param>
	/// <param name="take">The number of records to limit the results to.</param>
	/// <param name="trackChanges">Should the fetched entities be tracked?</param>
	/// <param name="token">The cancellation token to cancel the request.</param>
	/// <param name="includeProperties">Any other navigation properties to include when returning the collection.</param>
	/// <returns>An asynchronous sequence of <typeparamref name="TEntity"/>.</returns>
	protected async IAsyncEnumerable<TEntity> QueryManyStream(
		Expression<Func<TEntity, bool>>? expression = null,
		Func<IQueryable<TEntity>, IQueryable<TEntity>>? queryFilter = null,
		bool ignoreQueryFilters = false,
		Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
		int? skip = null,
		int? take = null,
		bool trackChanges = false,
		[EnumeratorCancellation] CancellationToken token = default,
		params string[] includeProperties)
	{
		IQueryable<TEntity> query = PrepareQuery(
			expression: expression,
			queryFilter: queryFilter,
			ignoreQueryFilters: ignoreQueryFilters,
			orderBy: orderBy,
			skip: skip,
			take: take,
			trackChanges: trackChanges,
			includeProperties: includeProperties
			);

		await foreach (TEntity entity in query.AsAsyncEnumerable().WithCancellation(token).ConfigureAwait(false))
			yield return entity;
	}

	/// <summary>
	/// Streams the collection of <typeparamref name="TResult"/> projections that match the provided criteria.
	/// </summary>
	/// <remarks>
	/// The result set is not buffered, the projections are yielded as they are read from the database.
	/// The returned sequence is lazy and must be enumerated within the lifetime of the underlying
	/// database context.
	/// </remarks>
	/// <typeparam name="TResult">The type of the result elements after projection.</typeparam>
	/// <param name="selector">The expression that defines the projection from the entity to the result type.</param>
	/// <param name="fieldSelector">The optional expression that further projects the result type.</param>
	/// <param name="expression">The condition to fulfill to be returned.</param>
	/// <param name="queryFilter">The function used to filter the entities.</param>
	/// <param name="ignoreQueryFilters">Should model-level entity query filters be applied?</param>
	/// <param name="orderBy">The function used to order the entities.</param>
	/// <param name="skip">The number of records to skip.</param>
	/// <param name="take">The number of records to limit the results to.</param>
	/// <param name="token">The cancellation token to cancel the request.</param>
	/// <returns>An asynchronous sequence of <typeparamref name="TResult"/>.</returns>
	protected async IAsyncEnumerable<TResult> QueryManyStream<TResult>(
		Expression<Func<TEntity, TResult>> selector,
		Expression<Func<TResult, TResult>>? fieldSelector = null,
		Expression<Func<TEntity, bool>>? expression = null,
		Func<IQueryable<TEntity>, IQueryable<TEntity>>? queryFilter = null,
		bool ignoreQueryFilters = false,
		Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
		int? skip = null,
		int? take = null,
		[EnumeratorCancellation] CancellationToken token = default)
	{
		IQueryable<TEntity> query = PrepareQuery(
			expression: expression,
			queryFilter: queryFilter,
			ignoreQueryFilters: ignoreQueryFilters,
			orderBy: orderBy,
			skip: skip,
			take: take
			);

		await foreach (TResult result in ApplyProjection(query, selector, fieldSelector).AsAsyncEnumerable().WithCancellation(token).ConfigureAwait(false))
			yield return result;
	}

	/// <summary>
	/// Prepares the <see cref="IQueryable"/> of type <typeparamref name="TEntity"/> before it gets executed.
	/// </summary>
	/// <param name="expression">The condition to fulfill to be returned.</param>
	/// <param name="queryFilter">The function used to filter the entities.</param>
	/// <param name="ignoreQueryFilters">Should model-level entity query filters be applied?</param>
	/// <param name="orderBy">The function used to order the entities.</param>
	/// <param name="take">The number of records to limit the results to.</param>
	/// <param name="skip">The number of records to skip.</param>
	/// <param name="trackChanges">Should the fetched entities be tracked?</param>
	/// <param name="includeProperties">Any other navigation properties to include when returning the collection.</param>
	/// <returns>A <see cref="IQueryable"/> of type <typeparamref name="TEntity"/>.</returns>
	protected IQueryable<TEntity> PrepareQuery(Expression<Func<TEntity, bool>>? expression = null, Func<IQueryable<TEntity>, IQueryable<TEntity>>? queryFilter = null, bool ignoreQueryFilters = false, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null, int? skip = null, int? take = null, bool trackChanges = false, params string[] includeProperties)
	{
		IQueryable<TEntity> query = !trackChanges ? DbSet.AsNoTracking() : DbSet;

		if (expression is not null)
			query = query.Where(expression);

		if (queryFilter is not null)
			query = queryFilter(query);

		if (ignoreQueryFilters)
			query = query.IgnoreQueryFilters();

		if (includeProperties.Length > 0)
			query = includeProperties.Aggregate(query, (theQuery, theInclude) => theQuery.Include(theInclude));

		if (orderBy is not null)
			query = orderBy(query);

		if (skip.HasValue)
			query = query.Skip(skip.Value);

		if (take.HasValue)
			query = query.Take(take.Value);

		return query;
	}

	/// <summary>
	/// Projects the elements of the source query to a new form using the specified selector, and optionally
	/// applies a secondary projection to the result.
	/// </summary>
	/// <remarks>
	/// Use this method to perform flexible projections on a query, allowing for both an initial and an optional
	/// secondary transformation. This can be useful when you need to shape the result set dynamically based on
	/// additional criteria.
	/// </remarks>
	/// <typeparam name="TResult">The type of the result elements after projection.</typeparam>
	/// <param name="query">The source query containing the elements to project.</param>
	/// <param name="selector">
	/// An expression that defines the initial projection from the source entity to the result type.
	/// </param>
	/// <param name="fieldSelector">
	/// An optional expression that further projects the result type. If provided, it is applied to each element
	/// after the initial projection.
	/// </param>
	/// <returns>
	/// An IQueryable containing the projected elements, optionally further transformed by the secondary selector.
	/// </returns>
	protected static IQueryable<TResult> ApplyProjection<TResult>(
		IQueryable<TEntity> query,
		Expression<Func<TEntity, TResult>> selector,
		Expression<Func<TResult, TResult>>? fieldSelector)
	{
		IQueryable<TResult> projected = query.Select(selector);

		if (fieldSelector is not null)
			projected = projected.Select(fieldSelector);

		return projected;
	}
}
