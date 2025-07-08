using BB84.EntityFrameworkCore.Entities.Abstractions;
using BB84.EntityFrameworkCore.Repositories.Abstractions;

using Microsoft.EntityFrameworkCore;

namespace BB84.EntityFrameworkCore.Repositories.SqlServer.Repositories;

/// <summary>
/// Represents a repository for managing hierarchical entities of type <typeparamref name="TEntity"/>.
/// </summary>
/// <inheritdoc cref="IdentityRepository{TEntity, TKey}"/>
public abstract class HierarchyRepository<TEntity>(IDbContext dbContext) : GenericRepository<TEntity>(dbContext), IHierarchyRepository<TEntity>
		where TEntity : class, IHierarchyEntity
{
	public IEnumerable<TEntity?> GetAncestor(int level, bool ignoreQueryFilters = false, bool trackChanges = false, params string[] includeProperties)
	{
		IQueryable<TEntity> query = PrepareQuery(
			expression: x => x.Id.GetLevel() == level,
			ignoreQueryFilters: ignoreQueryFilters,
			trackChanges: trackChanges,
			includeProperties: includeProperties
		);

		return [.. query];
	}

	public async Task<IEnumerable<TEntity?>> GetAncestorAsync(int level, bool ignoreQueryFilters = false, bool trackChanges = false, CancellationToken token = default, params string[] includeProperties)
	{
		IQueryable<TEntity> query = PrepareQuery(
			expression: x => x.Id.GetLevel() == level,
			ignoreQueryFilters: ignoreQueryFilters,
			trackChanges: trackChanges,
			includeProperties: includeProperties
		);

		return await query.ToListAsync(token)
			.ConfigureAwait(false);
	}

	public TEntity? GetById(HierarchyId id, bool ignoreQueryFilters = false, bool trackChanges = false, params string[] includeProperties)
	{
		IQueryable<TEntity> query = PrepareQuery(
			expression: x => x.Id == id,
			ignoreQueryFilters: ignoreQueryFilters,
			trackChanges: trackChanges,
			includeProperties: includeProperties
		);

		return query.SingleOrDefault();
	}

	public async Task<TEntity?> GetByIdAsync(HierarchyId id, bool ignoreQueryFilters = false, bool trackChanges = false, CancellationToken token = default, params string[] includeProperties)
	{
		IQueryable<TEntity> query = PrepareQuery(
			expression: x => x.Id == id,
			ignoreQueryFilters: ignoreQueryFilters,
			trackChanges: trackChanges,
			includeProperties: includeProperties
		);

		return await query.SingleOrDefaultAsync(token)
			.ConfigureAwait(false);
	}

	public IEnumerable<TEntity> GetByIds(IEnumerable<HierarchyId> ids, bool ignoreQueryFilters = false, bool trackChanges = false, params string[] includeProperties)
	{
		var query = PrepareQuery(
			expression: x => ids.Contains(x.Id),
			ignoreQueryFilters: ignoreQueryFilters,
			trackChanges: trackChanges,
			includeProperties: includeProperties
		);

		return [.. query];
	}

	public async Task<IEnumerable<TEntity>> GetByIdsAsync(IEnumerable<HierarchyId> ids, bool ignoreQueryFilters = false, bool trackChanges = false, CancellationToken token = default, params string[] includeProperties)
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

	public IEnumerable<TEntity> GetDescendants(HierarchyId? childId1, HierarchyId? childId2, bool ignoreQueryFilters = false, bool trackChanges = false, params string[] includeProperties)
	{
		IQueryable<TEntity> query = PrepareQuery(
			expression: x => x.Id == x.Id.GetDescendant(childId1, childId2),
			ignoreQueryFilters: ignoreQueryFilters,
			trackChanges: trackChanges,
			includeProperties: includeProperties
		);

		return [.. query];
	}

	public async Task<IEnumerable<TEntity>> GetDescendantsAsync(HierarchyId? childId1, HierarchyId? childId2, bool ignoreQueryFilters = false, bool trackChanges = false, CancellationToken token = default, params string[] includeProperties)
	{
		IQueryable<TEntity> query = PrepareQuery(
			expression: x => x.Id == x.Id.GetDescendant(childId1, childId2),
			ignoreQueryFilters: ignoreQueryFilters,
			trackChanges: trackChanges,
			includeProperties: includeProperties
		);

		return await query.ToListAsync(token)
			.ConfigureAwait(false);
	}
}
