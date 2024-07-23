using BB84.EntityFrameworkCore.Models.Abstractions;
using BB84.EntityFrameworkCore.Repositories.Abstractions;

using Microsoft.EntityFrameworkCore;

namespace BB84.EntityFrameworkCore.Repositories;

/// <summary>
/// The identity repository class.
/// </summary>
/// <inheritdoc cref="IIdentityRepository{TEntity, TKey}"/>
public abstract class IdentityRepository<TEntity, TKey>(IDbContext dbContext) : GenericRepository<TEntity>(dbContext), IIdentityRepository<TEntity, TKey>
	where TEntity : class, IIdentityModel<TKey>
	where TKey : IEquatable<TKey>
{
	/// <inheritdoc/>
	public TEntity? GetById(TKey id, bool ignoreQueryFilters = false, bool trackChanges = false)
	{
		IQueryable<TEntity> query = PrepareQuery(
			expression: x => x.Id.Equals(id),
			ignoreQueryFilters: ignoreQueryFilters,
			trackChanges: trackChanges
			);

		return query.SingleOrDefault();
	}

	/// <inheritdoc/>
	public async Task<TEntity?> GetByIdAsync(TKey id, bool ignoreQueryFilters = false, bool trackChanges = false, CancellationToken cancellationToken = default)
	{
		IQueryable<TEntity> query = PrepareQuery(
			expression: x => x.Id.Equals(id),
			ignoreQueryFilters: ignoreQueryFilters,
			trackChanges: trackChanges
			);

		return await query.SingleOrDefaultAsync(cancellationToken);
	}

	/// <inheritdoc/>
	public IEnumerable<TEntity> GetByIds(IEnumerable<TKey> ids, bool ignoreQueryFilters = false, bool trackChanges = false)
	{
		IQueryable<TEntity> query = PrepareQuery(
			expression: x => ids.Contains(x.Id),
			ignoreQueryFilters: ignoreQueryFilters,
			trackChanges: trackChanges
			);

		return [.. query];
	}

	/// <inheritdoc/>
	public async Task<IEnumerable<TEntity>> GetByIdsAsync(IEnumerable<TKey> ids, bool ignoreQueryFilters = false, bool trackChanges = false, CancellationToken cancellationToken = default)
	{
		IQueryable<TEntity> query = PrepareQuery(
			expression: x => ids.Contains(x.Id),
			ignoreQueryFilters: ignoreQueryFilters,
			trackChanges: trackChanges
			);

		return await query.ToListAsync(cancellationToken);
	}
}

/// <inheritdoc/>
public abstract class IdentityRepository<TEntity>(DbContext dbContext) : IdentityRepository<TEntity, Guid>(dbContext),
	IIdentityRepository<TEntity> where TEntity : class, IIdentityModel
{ }
