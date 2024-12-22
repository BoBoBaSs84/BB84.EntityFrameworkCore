using System.Linq.Expressions;

using BB84.EntityFrameworkCore.Models.Abstractions;
using BB84.EntityFrameworkCore.Repositories.Abstractions;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace BB84.EntityFrameworkCore.Repositories;

/// <summary>
/// The repository base implementation for identity based entities.
/// </summary>
/// <inheritdoc cref="IIdentityRepository{TEntity, TKey}"/>
public abstract class IdentityRepository<TEntity, TKey>(IDbContext dbContext) : GenericRepository<TEntity>(dbContext), IIdentityRepository<TEntity, TKey>
	where TEntity : class, IIdentityModel<TKey> where TKey : IEquatable<TKey>
{
	/// <inheritdoc/>
	public int Delete(TKey id)
		=> Delete(x => x.Id.Equals(id));

	/// <inheritdoc/>
	public int Delete(IEnumerable<TKey> ids)
		=> Delete(x => ids.Contains(x.Id));

	/// <inheritdoc/>
	public async Task<int> DeleteAsync(TKey id, CancellationToken token = default)
		=> await DeleteAsync(x => x.Id.Equals(id), token).ConfigureAwait(false);

	/// <inheritdoc/>
	public async Task<int> DeleteAsync(IEnumerable<TKey> ids, CancellationToken token = default)
		=> await DeleteAsync(x => ids.Contains(x.Id), token).ConfigureAwait(false);

	/// <inheritdoc/>
	public TEntity? GetById(TKey id, bool ignoreQueryFilters = false, bool trackChanges = false, params string[] includeProperties)
	{
		IQueryable<TEntity> query = PrepareQuery(
			expression: x => x.Id.Equals(id),
			ignoreQueryFilters: ignoreQueryFilters,
			trackChanges: trackChanges,
			includeProperties: includeProperties
			);

		return query.SingleOrDefault();
	}

	/// <inheritdoc/>
	public async Task<TEntity?> GetByIdAsync(TKey id, bool ignoreQueryFilters = false, bool trackChanges = false, CancellationToken token = default, params string[] includeProperties)
	{
		IQueryable<TEntity> query = PrepareQuery(
			expression: x => x.Id.Equals(id),
			ignoreQueryFilters: ignoreQueryFilters,
			trackChanges: trackChanges,
			includeProperties: includeProperties
			);

		return await query.SingleOrDefaultAsync(token)
			.ConfigureAwait(false);
	}

	/// <inheritdoc/>
	public IEnumerable<TEntity> GetByIds(IEnumerable<TKey> ids, bool ignoreQueryFilters = false, bool trackChanges = false, params string[] includeProperties)
	{
		IQueryable<TEntity> query = PrepareQuery(
			expression: x => ids.Contains(x.Id),
			ignoreQueryFilters: ignoreQueryFilters,
			trackChanges: trackChanges,
			includeProperties: includeProperties
			);

		return [.. query];
	}

	/// <inheritdoc/>
	public async Task<IEnumerable<TEntity>> GetByIdsAsync(IEnumerable<TKey> ids, bool ignoreQueryFilters = false, bool trackChanges = false, CancellationToken token = default, params string[] includeProperties)
	{
		IQueryable<TEntity> query = PrepareQuery(
			expression: x => ids.Contains(x.Id),
			ignoreQueryFilters: ignoreQueryFilters,
			trackChanges: trackChanges,
			includeProperties: includeProperties
			);

		return await query.ToListAsync(token)
			.ConfigureAwait(false);
	}

	/// <inheritdoc/>
	public int Update(TKey id, Expression<Func<SetPropertyCalls<TEntity>, SetPropertyCalls<TEntity>>> setPropertyCalls)
		=> Update(x => x.Id.Equals(id), setPropertyCalls);

	/// <inheritdoc/>
	public int Update(IEnumerable<TKey> ids, Expression<Func<SetPropertyCalls<TEntity>, SetPropertyCalls<TEntity>>> setPropertyCalls)
		=> Update(x => ids.Equals(x.Id), setPropertyCalls);

	/// <inheritdoc/>
	public async Task<int> UpdateAsync(TKey id, Expression<Func<SetPropertyCalls<TEntity>, SetPropertyCalls<TEntity>>> setPropertyCalls, CancellationToken token = default)
		=> await UpdateAsync(x => x.Id.Equals(id), setPropertyCalls, token).ConfigureAwait(false);

	/// <inheritdoc/>
	public async Task<int> UpdateAsync(IEnumerable<TKey> ids, Expression<Func<SetPropertyCalls<TEntity>, SetPropertyCalls<TEntity>>> setPropertyCalls, CancellationToken token = default)
		=> await UpdateAsync(x => ids.Contains(x.Id), setPropertyCalls, token).ConfigureAwait(false);
}

/// <inheritdoc cref="IdentityRepository{TEntity, TKey}"/>
/// <inheritdoc cref="IIdentityRepository{TEntity}"/>
public abstract class IdentityRepository<TEntity>(IDbContext dbContext) : IdentityRepository<TEntity, Guid>(dbContext), IIdentityRepository<TEntity>
	where TEntity : class, IIdentityModel
{ }
