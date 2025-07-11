﻿// Copyright: 2024 Robert Peter Meyer
// License: MIT
//
// This source code is licensed under the MIT license found in the
// LICENSE file in the root directory of this source tree.
using System.Linq.Expressions;

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
	{
		IQueryable<TEntity> query = PrepareQuery(
			ignoreQueryFilters: ignoreQueryFilters
			);

		return query.Count();
	}

	/// <inheritdoc/>
	public int Count(Expression<Func<TEntity, bool>>? expression = null, Func<IQueryable<TEntity>, IQueryable<TEntity>>? queryFilter = null, bool ignoreQueryFilters = false)
	{
		IQueryable<TEntity> query = PrepareQuery(
			expression: expression,
			queryFilter: queryFilter,
			ignoreQueryFilters: ignoreQueryFilters
			);

		return query.Count();
	}

	/// <inheritdoc/>
	public async Task<int> CountAllAsync(bool ignoreQueryFilters = false, CancellationToken token = default)
	{
		IQueryable<TEntity> query = PrepareQuery(
			ignoreQueryFilters: ignoreQueryFilters
			);

		return await query.CountAsync(token)
			.ConfigureAwait(false);
	}

	/// <inheritdoc/>
	public async Task<int> CountAsync(Expression<Func<TEntity, bool>>? expression = null, Func<IQueryable<TEntity>, IQueryable<TEntity>>? queryFilter = null, bool ignoreQueryFilters = false, CancellationToken token = default)
	{
		IQueryable<TEntity> query = PrepareQuery(
			expression: expression,
			queryFilter: queryFilter,
			ignoreQueryFilters: ignoreQueryFilters
			);

		return await query.CountAsync(token)
			.ConfigureAwait(false);
	}

	/// <inheritdoc/>
	public void Delete(TEntity entity)
		=> DbSet.Remove(entity);

	/// <inheritdoc/>
	public void Delete(IEnumerable<TEntity> entities)
		=> DbSet.RemoveRange(entities);

	/// <inheritdoc/>
	public int Delete(Expression<Func<TEntity, bool>>? expression)
		=> PrepareQuery(expression).ExecuteDelete();

	/// <inheritdoc/>
	public async Task DeleteAsync(TEntity entity, CancellationToken token = default)
		=> await Task.Run(() => DbSet.Remove(entity), token).ConfigureAwait(false);

	/// <inheritdoc/>
	public async Task DeleteAsync(IEnumerable<TEntity> entities, CancellationToken token = default)
		=> await Task.Run(() => DbSet.RemoveRange(entities), token).ConfigureAwait(false);

	/// <inheritdoc/>
	public async Task<int> DeleteAsync(Expression<Func<TEntity, bool>>? expression, CancellationToken token = default)
		=> await PrepareQuery(expression).ExecuteDeleteAsync(token).ConfigureAwait(false);

	/// <inheritdoc/>
	public IEnumerable<TEntity> GetAll(bool ignoreQueryFilters = false, bool trackChanges = false)
	{
		IQueryable<TEntity> query = PrepareQuery(
			ignoreQueryFilters: ignoreQueryFilters,
			trackChanges: trackChanges
			);

		return [.. query];
	}

	/// <inheritdoc/>
	public async Task<IEnumerable<TEntity>> GetAllAsync(bool ignoreQueryFilters = false, bool trackChanges = false, CancellationToken token = default)
	{
		IQueryable<TEntity> query = PrepareQuery(
			ignoreQueryFilters: ignoreQueryFilters,
			trackChanges: trackChanges
			);

		return await query.ToListAsync(token)
			.ConfigureAwait(false);
	}

	/// <inheritdoc/>
	public IEnumerable<TEntity> GetManyByCondition(Expression<Func<TEntity, bool>>? expression = null, Func<IQueryable<TEntity>, IQueryable<TEntity>>? queryFilter = null, bool ignoreQueryFilters = false, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null, int? skip = null, int? take = null, bool trackChanges = false, params string[] includeProperties)
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

	/// <inheritdoc/>
	public async Task<IEnumerable<TEntity>> GetManyByConditionAsync(Expression<Func<TEntity, bool>>? expression = null, Func<IQueryable<TEntity>, IQueryable<TEntity>>? queryFilter = null, bool ignoreQueryFilters = false, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null, int? skip = null, int? take = null, bool trackChanges = false, CancellationToken token = default, params string[] includeProperties)
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

	/// <inheritdoc/>
	public TEntity? GetByCondition(Expression<Func<TEntity, bool>> expression, Func<IQueryable<TEntity>, IQueryable<TEntity>>? queryFilter = null, bool ignoreQueryFilters = false, bool trackChanges = false, params string[] includeProperties)
	{
		IQueryable<TEntity> query = PrepareQuery(
			expression: expression,
			queryFilter: queryFilter,
			ignoreQueryFilters: ignoreQueryFilters,
			trackChanges: trackChanges,
			includeProperties: includeProperties
			);

		return query.FirstOrDefault();
	}

	/// <inheritdoc/>
	public async Task<TEntity?> GetByConditionAsync(Expression<Func<TEntity, bool>> expression, Func<IQueryable<TEntity>, IQueryable<TEntity>>? queryFilter = null, bool ignoreQueryFilters = false, bool trackChanges = false, CancellationToken token = default, params string[] includeProperties)
	{
		IQueryable<TEntity> query = PrepareQuery(
			expression: expression,
			queryFilter: queryFilter,
			ignoreQueryFilters: ignoreQueryFilters,
			trackChanges: trackChanges,
			includeProperties: includeProperties
			);

		return await query.FirstOrDefaultAsync(token)
			.ConfigureAwait(false);
	}

	/// <inheritdoc/>
	public void Update(TEntity entity)
		=> DbSet.Update(entity);

	/// <inheritdoc/>
	public void Update(IEnumerable<TEntity> entities)
		=> DbSet.UpdateRange(entities);

	/// <inheritdoc/>
	public int Update(Expression<Func<TEntity, bool>> expression, Expression<Func<SetPropertyCalls<TEntity>, SetPropertyCalls<TEntity>>> setPropertyCalls)
		=> PrepareQuery(expression).ExecuteUpdate(setPropertyCalls);

	/// <inheritdoc/>
	public async Task UpdateAsync(TEntity entity, CancellationToken token = default)
		=> await Task.Run(() => DbSet.Update(entity), token).ConfigureAwait(false);

	/// <inheritdoc/>
	public async Task UpdateAsync(IEnumerable<TEntity> entities, CancellationToken token = default)
		=> await Task.Run(() => DbSet.UpdateRange(entities), token).ConfigureAwait(false);

	/// <inheritdoc/>
	public async Task<int> UpdateAsync(Expression<Func<TEntity, bool>> expression, Expression<Func<SetPropertyCalls<TEntity>, SetPropertyCalls<TEntity>>> setPropertyCalls, CancellationToken token = default)
		=> await PrepareQuery(expression).ExecuteUpdateAsync(setPropertyCalls, token).ConfigureAwait(false);

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
}
