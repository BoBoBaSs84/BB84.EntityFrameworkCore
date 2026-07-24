// Copyright: 2024 Robert Peter Meyer
// License: MIT
//
// This source code is licensed under the MIT license found in the
// LICENSE file in the root directory of this source tree.
using System.Linq.Expressions;

using BB84.EntityFrameworkCore.Entities.Abstractions;
using BB84.EntityFrameworkCore.Repositories.Abstractions;

using Microsoft.EntityFrameworkCore.Query;

namespace BB84.EntityFrameworkCore.Repositories;

/// <summary>
/// Provides an abstract base class for managing identity-based entities in a data store.
/// </summary>
/// <remarks>
/// This interface extends <see cref="GenericRepository{TEntity}"/> and provides additional
/// methods for CRUD operations specifically tailored to entities with identity-based primary
/// keys.
/// </remarks>
/// <typeparam name="TEntity">The type of the entity managed by the repository.</typeparam>
/// <typeparam name="TKey">The type of the unique identifier for the entity.</typeparam>
/// <param name="dbContext">The database context to work with.</param>
public abstract class IdentityRepository<TEntity, TKey>(IDbContext dbContext) : GenericRepository<TEntity>(dbContext), IIdentityRepository<TEntity, TKey>
	where TEntity : class, IIdentityEntity<TKey>
	where TKey : IEquatable<TKey>
{
	/// <inheritdoc/>
	public int Delete(TKey id)
		=> Delete(ById(id));

	/// <inheritdoc/>
	public int Delete(IEnumerable<TKey> ids)
		=> Delete(ByIds(ids));

	/// <inheritdoc/>
	public async Task<int> DeleteAsync(TKey id, CancellationToken token = default)
		=> await DeleteAsync(ById(id), token).ConfigureAwait(false);

	/// <inheritdoc/>
	public async Task<int> DeleteAsync(IEnumerable<TKey> ids, CancellationToken token = default)
		=> await DeleteAsync(ByIds(ids), token).ConfigureAwait(false);

	/// <inheritdoc/>
	public TEntity? GetById(TKey id, bool ignoreQueryFilters = false, bool trackChanges = false, params string[] includeProperties)
		=> QuerySingle(expression: ById(id), ignoreQueryFilters: ignoreQueryFilters, trackChanges: trackChanges, includeProperties: includeProperties);

	/// <inheritdoc/>
	public TResult? GetById<TResult>(TKey id, Expression<Func<TEntity, TResult>> selector, Expression<Func<TResult, TResult>>? fieldSelector = null, bool ignoreQueryFilters = false)
		=> QuerySingle(selector: selector, fieldSelector: fieldSelector, expression: ById(id), ignoreQueryFilters: ignoreQueryFilters);

	/// <inheritdoc/>
	public async Task<TEntity?> GetByIdAsync(TKey id, bool ignoreQueryFilters = false, bool trackChanges = false, CancellationToken token = default, params string[] includeProperties)
		=> await QuerySingleAsync(expression: ById(id), ignoreQueryFilters: ignoreQueryFilters, trackChanges: trackChanges, token: token, includeProperties: includeProperties).ConfigureAwait(false);

	/// <inheritdoc/>
	public async Task<TResult?> GetByIdAsync<TResult>(TKey id, Expression<Func<TEntity, TResult>> selector, Expression<Func<TResult, TResult>>? fieldSelector = null, bool ignoreQueryFilters = false, CancellationToken token = default)
		=> await QuerySingleAsync(selector: selector, fieldSelector: fieldSelector, expression: ById(id), ignoreQueryFilters: ignoreQueryFilters, token: token).ConfigureAwait(false);

	/// <inheritdoc/>
	public IReadOnlyList<TEntity> GetByIds(IEnumerable<TKey> ids, bool ignoreQueryFilters = false, bool trackChanges = false, params string[] includeProperties)
		=> QueryMany(expression: ByIds(ids), ignoreQueryFilters: ignoreQueryFilters, trackChanges: trackChanges, includeProperties: includeProperties);

	/// <inheritdoc/>
	public IReadOnlyList<TResult> GetByIds<TResult>(IEnumerable<TKey> ids, Expression<Func<TEntity, TResult>> selector, Expression<Func<TResult, TResult>>? fieldSelector = null, bool ignoreQueryFilters = false)
		=> QueryMany(selector: selector, fieldSelector: fieldSelector, expression: ByIds(ids), ignoreQueryFilters: ignoreQueryFilters);

	/// <inheritdoc/>
	public async Task<IReadOnlyList<TEntity>> GetByIdsAsync(IEnumerable<TKey> ids, bool ignoreQueryFilters = false, bool trackChanges = false, CancellationToken token = default, params string[] includeProperties)
		=> await QueryManyAsync(expression: ByIds(ids), ignoreQueryFilters: ignoreQueryFilters, trackChanges: trackChanges, token: token, includeProperties: includeProperties).ConfigureAwait(false);

	/// <inheritdoc/>
	public async Task<IReadOnlyList<TResult>> GetByIdsAsync<TResult>(IEnumerable<TKey> ids, Expression<Func<TEntity, TResult>> selector, Expression<Func<TResult, TResult>>? fieldSelector = null, bool ignoreQueryFilters = false, CancellationToken token = default)
		=> await QueryManyAsync(selector: selector, fieldSelector: fieldSelector, expression: ByIds(ids), ignoreQueryFilters: ignoreQueryFilters, token: token).ConfigureAwait(false);

	/// <inheritdoc/>
	public int Update(
		TKey id,
#if NET8_0
		Expression<Func<SetPropertyCalls<TEntity>, SetPropertyCalls<TEntity>>> setPropertyCalls)
#else
		Action<UpdateSettersBuilder<TEntity>> setPropertyCalls)
#endif
		=> Update(ById(id), setPropertyCalls);

	/// <inheritdoc/>
	public int Update(
		IEnumerable<TKey> ids,
#if NET8_0
		Expression<Func<SetPropertyCalls<TEntity>, SetPropertyCalls<TEntity>>> setPropertyCalls)
#else
		Action<UpdateSettersBuilder<TEntity>> setPropertyCalls)
#endif
		=> Update(ByIds(ids), setPropertyCalls);

	/// <inheritdoc/>
	public async Task<int> UpdateAsync(
		TKey id,
#if NET8_0
		Expression<Func<SetPropertyCalls<TEntity>, SetPropertyCalls<TEntity>>> setPropertyCalls,
#else
		Action<UpdateSettersBuilder<TEntity>> setPropertyCalls,
#endif
		CancellationToken token = default)
		=> await UpdateAsync(ById(id), setPropertyCalls, token).ConfigureAwait(false);

	/// <inheritdoc/>
	public async Task<int> UpdateAsync(
		IEnumerable<TKey> ids,
#if NET8_0
		Expression<Func<SetPropertyCalls<TEntity>, SetPropertyCalls<TEntity>>> setPropertyCalls,
#else
		Action<UpdateSettersBuilder<TEntity>> setPropertyCalls,
#endif
		CancellationToken token = default)
		=> await UpdateAsync(ByIds(ids), setPropertyCalls, token).ConfigureAwait(false);

	/// <summary>
	/// Returns the condition that matches the <typeparamref name="TEntity"/> with the provided <paramref name="id"/>.
	/// </summary>
	/// <param name="id">The primary key of the <typeparamref name="TEntity"/>.</param>
	/// <returns>The condition to fulfill to be selected.</returns>
	protected static Expression<Func<TEntity, bool>> ById(TKey id)
		=> x => x.Id.Equals(id);

	/// <summary>
	/// Returns the condition that matches the <typeparamref name="TEntity"/> instances with the provided <paramref name="ids"/>.
	/// </summary>
	/// <param name="ids">The primary keys of the <typeparamref name="TEntity"/>.</param>
	/// <returns>The condition to fulfill to be selected.</returns>
	protected static Expression<Func<TEntity, bool>> ByIds(IEnumerable<TKey> ids)
		=> x => ids.Contains(x.Id);
}

/// <inheritdoc cref="IdentityRepository{TEntity, TKey}"/>
public abstract class IdentityRepository<TEntity>(IDbContext dbContext) : IdentityRepository<TEntity, Guid>(dbContext), IIdentityRepository<TEntity>
	where TEntity : class, IIdentityEntity
{ }
