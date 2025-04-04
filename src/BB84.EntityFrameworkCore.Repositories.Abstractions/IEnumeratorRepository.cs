// Copyright: 2024 Robert Peter Meyer
// License: MIT
//
// This source code is licensed under the MIT license found in the
// LICENSE file in the root directory of this source tree.
using BB84.EntityFrameworkCore.Entities.Abstractions;

namespace BB84.EntityFrameworkCore.Repositories.Abstractions;

/// <summary>
/// The repository interface for enumerator based entities.
/// </summary>
/// <inheritdoc cref="IIdentityRepository{TEntity, TKey}"/>
public interface IEnumeratorRepository<TEntity, TKey> : IIdentityRepository<TEntity, TKey>
	where TEntity : class, IEnumeratorEntity<TKey>
	where TKey : IEquatable<TKey>
{
	/// <summary>
	/// Returns an <typeparamref name="TEntity"/> by its name or <see langword="null"/>.
	/// </summary>
	/// <param name="name">The name of the <typeparamref name="TEntity"/>.</param>
	/// <param name="ignoreQueryFilters">Should model-level entity query filters be applied?</param>
	/// <param name="trackChanges">Should the fetched entity be tracked?</param>
	/// <returns>The <typeparamref name="TEntity"/> or <see langword="null"/>.</returns>
	TEntity? GetByName(
		string name,
		bool ignoreQueryFilters = false,
		bool trackChanges = false
		);

	/// <summary>
	/// Returns an <typeparamref name="TEntity"/> by its name or <see langword="null"/>.
	/// </summary>
	/// <param name="name">The name of the <typeparamref name="TEntity"/>.</param>
	/// <param name="ignoreQueryFilters">Should model-level entity query filters be applied?</param>
	/// <param name="trackChanges">Should the fetched entity be tracked?</param>
	/// <param name="cancellationToken">The cancellation token to cancel the request.</param>
	/// <returns>The <typeparamref name="TEntity"/> or <see langword="null"/>.</returns>
	Task<TEntity?> GetByNameAsync(
		string name,
		bool ignoreQueryFilters = false,
		bool trackChanges = false,
		CancellationToken cancellationToken = default
		);

	/// <summary>
	/// Returns a collection of <typeparamref name="TEntity"/> by their names or an empty result.
	/// </summary>
	/// <param name="names">The names of the <typeparamref name="TEntity"/>.</param>
	/// <param name="ignoreQueryFilters">Should model-level entity query filters be applied?</param>
	/// <param name="trackChanges">Should the fetched entities be tracked?</param>
	/// <returns>A collection of <typeparamref name="TEntity"/>.</returns>
	IEnumerable<TEntity> GetByNames(
		IEnumerable<string> names,
		bool ignoreQueryFilters = false,
		bool trackChanges = false
		);

	/// <summary>
	/// Returns a collection of <typeparamref name="TEntity"/> by their names or an empty result.
	/// </summary>
	/// <param name="names">The names of the <typeparamref name="TEntity"/>.</param>
	/// <param name="ignoreQueryFilters">Should model-level entity query filters be applied?</param>
	/// <param name="trackChanges">Should the fetched entities be tracked?</param>
	/// <param name="cancellationToken">The cancellation token.</param>
	/// <returns>A collection of <typeparamref name="TEntity"/>.</returns>
	Task<IEnumerable<TEntity>> GetByNamesAsync(
		IEnumerable<string> names,
		bool ignoreQueryFilters = false,
		bool trackChanges = false,
		CancellationToken cancellationToken = default
		);
}

/// <inheritdoc cref="IEnumeratorRepository{TEntity, TKey}"/>
/// <remarks>
/// The identity column is of type <see cref="int"/>.
/// </remarks>
public interface IEnumeratorRepository<TEntity> : IEnumeratorRepository<TEntity, int>
	where TEntity : class, IEnumeratorEntity
{ }
