using BB84.EntityFrameworkCore.Models.Abstractions;
using BB84.EntityFrameworkCore.Repositories.Abstractions;

using Microsoft.EntityFrameworkCore;

namespace BB84.EntityFrameworkCore.Repositories;

/// <summary>
/// The enumerator repository class.
/// </summary>
/// <inheritdoc cref="IEnumeratorRepository{TEntity}"/>
public abstract class EnumeratorRepository<TEntity>(DbContext dbContext) : IdentityRepository<TEntity, int>(dbContext),
	IEnumeratorRepository<TEntity> where TEntity : class, IEnumeratorModel
{
	/// <inheritdoc/>
	public TEntity? GetByName(string name, bool ignoreQueryFilters = false, bool trackChanges = false)
	{
		IQueryable<TEntity> query = PrepareQuery(
			expression: x => x.Name == name,
			ignoreQueryFilters: ignoreQueryFilters,
			trackChanges: trackChanges
			);

		return query.SingleOrDefault();
	}

	/// <inheritdoc/>
	public async Task<TEntity?> GetByNameAsync(string name, bool ignoreQueryFilters = false, bool trackChanges = false, CancellationToken cancellationToken = default)
	{
		IQueryable<TEntity> query = PrepareQuery(
			expression: x => x.Name == name,
			ignoreQueryFilters: ignoreQueryFilters,
			trackChanges: trackChanges
			);

		return await query.SingleOrDefaultAsync(cancellationToken);
	}

	/// <inheritdoc/>
	public IEnumerable<TEntity> GetByNames(IEnumerable<string> names, bool ignoreQueryFilters = false, bool trackChanges = false)
	{
		IQueryable<TEntity> query = PrepareQuery(
			expression: x => names.Contains(x.Name),
			ignoreQueryFilters: ignoreQueryFilters,
			trackChanges: trackChanges
			);

		return [.. query];
	}

	/// <inheritdoc/>
	public async Task<IEnumerable<TEntity>> GetByNamesAsync(IEnumerable<string> names, bool ignoreQueryFilters = false, bool trackChanges = false, CancellationToken cancellationToken = default)
	{
		IQueryable<TEntity> query = PrepareQuery(
			expression: x => names.Contains(x.Name),
			ignoreQueryFilters: ignoreQueryFilters,
			trackChanges: trackChanges
			);

		return await query.ToListAsync(cancellationToken);
	}
}
