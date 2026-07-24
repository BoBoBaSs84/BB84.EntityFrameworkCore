// Copyright: 2024 Robert Peter Meyer
// License: MIT
//
// This source code is licensed under the MIT license found in the
// LICENSE file in the root directory of this source tree.
using System.Linq.Expressions;

using BB84.EntityFrameworkCore.Entities.Abstractions;
using BB84.EntityFrameworkCore.Repositories.Abstractions;

namespace BB84.EntityFrameworkCore.Repositories;

/// <summary>
/// Provides an abstract base class for managing enumeration-based entities in a data store.
/// </summary>
/// <remarks>
/// This interface extends <see cref="IdentityRepository{TEntity, TKey}"/> and provides
/// additional methods for CRUD operations specifically tailored to entities with
/// identity-based primary keys.
/// </remarks>
/// <typeparam name="TEntity">The type of the entity managed by the repository.</typeparam>
/// <typeparam name="TKey">The type of the unique identifier for the entity.</typeparam>
/// <param name="dbContext">The database context to work with.</param>
public abstract class EnumeratorRepository<TEntity, TKey>(IDbContext dbContext) : IdentityRepository<TEntity, TKey>(dbContext), IEnumeratorRepository<TEntity, TKey>
	where TEntity : class, IEnumeratorEntity<TKey>
	where TKey : IEquatable<TKey>
{
	/// <inheritdoc/>
	public TEntity? GetByName(string name, bool ignoreQueryFilters = false, bool trackChanges = false)
		=> QuerySingle(expression: ByName(name), ignoreQueryFilters: ignoreQueryFilters, trackChanges: trackChanges);

	/// <inheritdoc/>
	public async Task<TEntity?> GetByNameAsync(string name, bool ignoreQueryFilters = false, bool trackChanges = false, CancellationToken cancellationToken = default)
		=> await QuerySingleAsync(expression: ByName(name), ignoreQueryFilters: ignoreQueryFilters, trackChanges: trackChanges, token: cancellationToken).ConfigureAwait(false);

	/// <inheritdoc/>
	public IReadOnlyList<TEntity> GetByNames(IEnumerable<string> names, bool ignoreQueryFilters = false, bool trackChanges = false)
		=> QueryMany(expression: ByNames(names), ignoreQueryFilters: ignoreQueryFilters, trackChanges: trackChanges);

	/// <inheritdoc/>
	public async Task<IReadOnlyList<TEntity>> GetByNamesAsync(IEnumerable<string> names, bool ignoreQueryFilters = false, bool trackChanges = false, CancellationToken cancellationToken = default)
		=> await QueryManyAsync(expression: ByNames(names), ignoreQueryFilters: ignoreQueryFilters, trackChanges: trackChanges, token: cancellationToken).ConfigureAwait(false);

	/// <summary>
	/// Returns the condition that matches the <typeparamref name="TEntity"/> with the provided <paramref name="name"/>.
	/// </summary>
	/// <param name="name">The name of the <typeparamref name="TEntity"/>.</param>
	/// <returns>The condition to fulfill to be selected.</returns>
	protected static Expression<Func<TEntity, bool>> ByName(string name)
		=> x => x.Name == name;

	/// <summary>
	/// Returns the condition that matches the <typeparamref name="TEntity"/> instances with the provided <paramref name="names"/>.
	/// </summary>
	/// <param name="names">The names of the <typeparamref name="TEntity"/>.</param>
	/// <returns>The condition to fulfill to be selected.</returns>
	protected static Expression<Func<TEntity, bool>> ByNames(IEnumerable<string> names)
		=> x => names.Contains(x.Name);
}

/// <inheritdoc cref="EnumeratorRepository{TEntity, TKey}"/>
public abstract class EnumeratorRepository<TEntity>(IDbContext dbContext) : EnumeratorRepository<TEntity, int>(dbContext), IEnumeratorRepository<TEntity>
	where TEntity : class, IEnumeratorEntity
{ }
