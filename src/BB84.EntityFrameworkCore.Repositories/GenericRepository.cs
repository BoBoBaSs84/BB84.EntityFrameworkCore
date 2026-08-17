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
	private readonly DbSet<TEntity> _dbSet = dbContext.Set<TEntity>();

	/// <inheritdoc/>
	public void Create(TEntity entity)
		=> _dbSet.Add(entity);

	/// <inheritdoc/>
	public void Create(IEnumerable<TEntity> entities)
		=> _dbSet.AddRange(entities);

	/// <inheritdoc/>
	public async Task CreateAsync(TEntity entity, CancellationToken cancellationToken = default)
		=> await _dbSet.AddAsync(entity, cancellationToken).ConfigureAwait(false);

	/// <inheritdoc/>
	public async Task CreateAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
		=> await _dbSet.AddRangeAsync(entities, cancellationToken).ConfigureAwait(false);

	/// <inheritdoc/>
	public void Delete(TEntity entity)
		=> _dbSet.Remove(entity);

	/// <inheritdoc/>
	public void Delete(IEnumerable<TEntity> entities)
		=> _dbSet.RemoveRange(entities);

	/// <inheritdoc/>
	public int ExecuteDelete(Expression<Func<TEntity, bool>> expression)
		=> _dbSet.Where(expression).ExecuteDelete();

	/// <inheritdoc/>
	public Task DeleteAsync(TEntity entity, CancellationToken cancellationToken = default)
	{
		if (cancellationToken.IsCancellationRequested)
			return Task.FromCanceled(cancellationToken);

		_ = _dbSet.Remove(entity);

		return Task.CompletedTask;
	}

	/// <inheritdoc/>
	public Task DeleteAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
	{
		if (cancellationToken.IsCancellationRequested)
			return Task.FromCanceled(cancellationToken);

		_dbSet.RemoveRange(entities);

		return Task.CompletedTask;
	}

	/// <inheritdoc/>
	public async Task<int> ExecuteDeleteAsync(Expression<Func<TEntity, bool>> expression, CancellationToken cancellationToken = default)
		=> await _dbSet.Where(expression).ExecuteDeleteAsync(cancellationToken).ConfigureAwait(false);

	/// <inheritdoc/>
	public int Count(Query<TEntity>? query = null)
		=> PrepareQuery(query, forCount: true).Count();

	/// <inheritdoc/>
	public async Task<int> CountAsync(Query<TEntity>? query = null, CancellationToken cancellationToken = default)
		=> await PrepareQuery(query, forCount: true).CountAsync(cancellationToken).ConfigureAwait(false);

	/// <inheritdoc/>
	public TEntity? GetSingle(Query<TEntity>? query = null)
		=> PrepareQuery(query).SingleOrDefault();

	/// <inheritdoc/>
	public TResult? GetSingle<TResult>(Expression<Func<TEntity, TResult>> selector, Query<TEntity>? query = null)
		=> ApplyProjection(PrepareQuery(query), selector).SingleOrDefault();

	/// <inheritdoc/>
	public async Task<TEntity?> GetSingleAsync(Query<TEntity>? query = null, CancellationToken cancellationToken = default)
		=> await PrepareQuery(query).SingleOrDefaultAsync(cancellationToken).ConfigureAwait(false);

	/// <inheritdoc/>
	public async Task<TResult?> GetSingleAsync<TResult>(Expression<Func<TEntity, TResult>> selector, Query<TEntity>? query = null, CancellationToken cancellationToken = default)
		=> await ApplyProjection(PrepareQuery(query), selector).SingleOrDefaultAsync(cancellationToken).ConfigureAwait(false);

	/// <inheritdoc/>
	public IReadOnlyList<TEntity> GetList(Query<TEntity>? query = null)
		=> [.. PrepareQuery(query)];

	/// <inheritdoc/>
	public IReadOnlyList<TResult> GetList<TResult>(Expression<Func<TEntity, TResult>> selector, Query<TEntity>? query = null)
		=> [.. ApplyProjection(PrepareQuery(query), selector)];

	/// <inheritdoc/>
	public async Task<IReadOnlyList<TEntity>> GetListAsync(Query<TEntity>? query = null, CancellationToken cancellationToken = default)
		=> await PrepareQuery(query).ToListAsync(cancellationToken).ConfigureAwait(false);

	/// <inheritdoc/>
	public async Task<IReadOnlyList<TResult>> GetListAsync<TResult>(Expression<Func<TEntity, TResult>> selector, Query<TEntity>? query = null, CancellationToken cancellationToken = default)
		=> await ApplyProjection(PrepareQuery(query), selector).ToListAsync(cancellationToken).ConfigureAwait(false);

	/// <inheritdoc/>
	public async IAsyncEnumerable<TEntity> Stream(Query<TEntity>? query = null, [EnumeratorCancellation] CancellationToken cancellationToken = default)
	{
		await foreach (TEntity entity in PrepareQuery(query).AsAsyncEnumerable().WithCancellation(cancellationToken).ConfigureAwait(false))
			yield return entity;
	}

	/// <inheritdoc/>
	public async IAsyncEnumerable<TResult> Stream<TResult>(Expression<Func<TEntity, TResult>> selector, Query<TEntity>? query = null, [EnumeratorCancellation] CancellationToken cancellationToken = default)
	{
		await foreach (TResult result in ApplyProjection(PrepareQuery(query), selector).AsAsyncEnumerable().WithCancellation(cancellationToken).ConfigureAwait(false))
			yield return result;
	}

	/// <inheritdoc/>
	public void Update(TEntity entity)
		=> _dbSet.Update(entity);

	/// <inheritdoc/>
	public void Update(IEnumerable<TEntity> entities)
		=> _dbSet.UpdateRange(entities);

	/// <inheritdoc/>
	public int ExecuteUpdate(
		Expression<Func<TEntity, bool>> expression,
		Action<UpdateSettersBuilder<TEntity>> setPropertyCalls)
		=> _dbSet.Where(expression).ExecuteUpdate(setPropertyCalls);

	/// <inheritdoc/>
	public Task UpdateAsync(TEntity entity, CancellationToken cancellationToken = default)
	{
		if (cancellationToken.IsCancellationRequested)
			return Task.FromCanceled(cancellationToken);

		_ = _dbSet.Update(entity);

		return Task.CompletedTask;
	}

	/// <inheritdoc/>
	public Task UpdateAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
	{
		if (cancellationToken.IsCancellationRequested)
			return Task.FromCanceled(cancellationToken);

		_dbSet.UpdateRange(entities);

		return Task.CompletedTask;
	}

	/// <inheritdoc/>
	public async Task<int> ExecuteUpdateAsync(
		Expression<Func<TEntity, bool>> expression,
		Action<UpdateSettersBuilder<TEntity>> setPropertyCalls,
		CancellationToken cancellationToken = default)
		=> await _dbSet.Where(expression).ExecuteUpdateAsync(setPropertyCalls, cancellationToken).ConfigureAwait(false);

	/// <summary>
	/// Composes the <see cref="IQueryable{TEntity}"/> described by the specified
	/// <paramref name="query"/>.
	/// </summary>
	/// <remarks>
	/// The single place where a <see cref="Query{TEntity}"/> is turned into a query. Use it
	/// when building a repository method the interface does not cover.
	/// </remarks>
	/// <param name="query">The query to apply. Applies nothing when omitted.</param>
	/// <param name="forCount">
	/// Whether the query is being composed for a count, in which case the options that cannot
	/// change how many rows match are skipped.
	/// </param>
	/// <returns>The composed query.</returns>
	protected IQueryable<TEntity> PrepareQuery(Query<TEntity>? query, bool forCount = false)
	{
		IQueryable<TEntity> queryable = query?.TrackChanges == true && !forCount
			? _dbSet
			: _dbSet.AsNoTracking();

		if (query is null)
			return queryable;

		if (query.Where is not null)
			queryable = queryable.Where(query.Where);

		if (query.QueryFilter is not null)
			queryable = query.QueryFilter(queryable);

		if (query.IgnoreQueryFilters)
			queryable = queryable.IgnoreQueryFilters();

		// Ordering, paging and includes cannot change how many rows match, and an ORDER BY
		// without a paging clause is invalid inside a count on some providers.
		if (forCount)
			return queryable;

		if (query.Include is not null)
			queryable = query.Include.Aggregate(queryable, (current, include) => current.Include(include));

		if (query.OrderBy is not null)
			queryable = query.OrderBy(queryable);

		if (query.Skip.HasValue)
			queryable = queryable.Skip(query.Skip.Value);

		if (query.Take.HasValue)
			queryable = queryable.Take(query.Take.Value);

		return queryable;
	}

	/// <summary>
	/// Projects the elements of the source query into <typeparamref name="TResult"/>.
	/// </summary>
	/// <typeparam name="TResult">The type to project into.</typeparam>
	/// <param name="query">The query to project.</param>
	/// <param name="selector">The projection to apply.</param>
	/// <returns>The projected query.</returns>
	protected static IQueryable<TResult> ApplyProjection<TResult>(
		IQueryable<TEntity> query,
		Expression<Func<TEntity, TResult>> selector)
		=> query.Select(selector);

	/// <summary>
	/// Returns the specified <paramref name="query"/> with an additional condition applied.
	/// </summary>
	/// <remarks>
	/// The condition is appended rather than replacing anything, so a caller supplied
	/// <see cref="Query{TEntity}.Where"/> keeps applying alongside it. Used by the derived
	/// repositories to add their key or name condition.
	/// </remarks>
	/// <param name="query">The query to extend.</param>
	/// <param name="condition">The condition to add.</param>
	/// <returns>The extended query.</returns>
	protected static Query<TEntity> WithCondition(Query<TEntity>? query, Expression<Func<TEntity, bool>> condition)
	{
		Func<IQueryable<TEntity>, IQueryable<TEntity>>? queryFilter = query?.QueryFilter;

		return (query ?? new Query<TEntity>()) with
		{
			QueryFilter = queryFilter is null
				? source => source.Where(condition)
				: source => queryFilter(source).Where(condition)
		};
	}
}
