// Copyright: 2024 Robert Peter Meyer
// License: MIT
//
// This source code is licensed under the MIT license found in the
// LICENSE file in the root directory of this source tree.
using BB84.EntityFrameworkCore.Entities.Abstractions;
using BB84.EntityFrameworkCore.Repositories.Abstractions;

using Microsoft.EntityFrameworkCore;

namespace BB84.EntityFrameworkCore.Repositories;

/// <summary>
/// The repository base implementation for enumerator based entities.
/// </summary>
/// <inheritdoc cref="IEnumeratorRepository{TEntity, TKey}"/>
public abstract class EnumeratorRepository<TEntity, TKey>(IDbContext dbContext) : IdentityRepository<TEntity, TKey>(dbContext), IEnumeratorRepository<TEntity, TKey>
	where TEntity : class, IEnumeratorEntity<TKey>
	where TKey : IEquatable<TKey>
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

/// <inheritdoc cref="EnumeratorRepository{TEntity, TKey}"/>
/// <inheritdoc cref="IEnumeratorRepository{TEntity}"/>
public abstract class EnumeratorRepository<TEntity>(IDbContext dbContext) : EnumeratorRepository<TEntity, int>(dbContext), IEnumeratorRepository<TEntity>
	where TEntity : class, IEnumeratorEntity
{ }
