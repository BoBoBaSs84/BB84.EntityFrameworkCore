using System.Linq.Expressions;

using BB84.EntityFrameworkCore.Models.Abstractions;

using Microsoft.EntityFrameworkCore.Query;

namespace BB84.EntityFrameworkCore.Repositories.Abstractions;

/// <summary>
/// The identity repository interface.
/// </summary>
/// <inheritdoc cref="IGenericRepository{TEntity}"/>
/// <inheritdoc cref="IIdentityModel{TKey}"/>
/// <remarks>
/// <typeparamref name="TEntity"/> must implement the <see cref="IIdentityModel"/> interface.
/// </remarks>
public interface IIdentityRepository<TEntity, TKey> : IGenericRepository<TEntity>
	where TEntity : class, IIdentityModel<TKey>
	where TKey : IEquatable<TKey>
{
	/// <summary>
	/// Deletes a database rows for the entity instances which matches the <paramref name="id"/>
	/// from the database.
	/// </summary>
	/// <param name="id">The primary key of the <typeparamref name="TEntity"/>.</param>
	/// <returns>The total number of rows deleted in the database.</returns>
	int DeleteById(TKey id);

	/// <summary>
	/// Deletes all database rows for the entity instances which matches the <paramref name="ids"/>
	/// from the database.
	/// </summary>
	/// <param name="ids">The primary keys of the <typeparamref name="TEntity"/>.</param>
	/// <returns>The total number of rows deleted in the database.</returns>
	int DeleteByIds(IEnumerable<TKey> ids);

	/// <summary>
	/// Deletes a database rows for the entity instances which matches the <paramref name="id"/>
	/// from the database.
	/// </summary>
	/// <param name="id">The primary key of the <typeparamref name="TEntity"/>.</param>
	/// <param name="cancellationToken">The cancellation token to cancel the request.</param>
	/// <returns>The total number of rows deleted in the database.</returns>
	Task<int> DeleteByIdAsync(TKey id, CancellationToken cancellationToken = default);

	/// <summary>
	/// Deletes all database rows for the entity instances which matches the <paramref name="ids"/>
	/// from the database.
	/// </summary>
	/// <param name="ids">The primary keys of the <typeparamref name="TEntity"/>.</param>
	/// <param name="cancellationToken">The cancellation token to cancel the request.</param>
	/// <returns>The total number of rows deleted in the database.</returns>
	Task<int> DeleteByIdsAsync(IEnumerable<TKey> ids, CancellationToken cancellationToken = default);

	/// <summary>
	/// Returns an <typeparamref name="TEntity"/> by its primary key.
	/// </summary>
	/// <param name="id">The primary key of the <typeparamref name="TEntity"/>.</param>
	/// <param name="ignoreQueryFilters">Should model-level entity query filters be applied?</param>
	/// <param name="trackChanges">Should the fetched entity be tracked?</param>
	/// <returns>An <typeparamref name="TEntity"/> or <see langword="null"/>.</returns>
	TEntity? GetById(
		TKey id,
		bool ignoreQueryFilters = false,
		bool trackChanges = false
		);

	/// <summary>
	/// Returns an <typeparamref name="TEntity"/> by its primary key.
	/// </summary>
	/// <param name="id">The primary key of the <typeparamref name="TEntity"/>.</param>
	/// <param name="ignoreQueryFilters">Should model-level entity query filters be applied?</param>
	/// <param name="trackChanges">Should the fetched entity be tracked?</param>
	/// <param name="cancellationToken">The cancellation token to cancel the request.</param>
	/// <returns>The <typeparamref name="TEntity"/> or <see langword="null"/>.</returns>
	Task<TEntity?> GetByIdAsync(
		TKey id,
		bool ignoreQueryFilters = false,
		bool trackChanges = false,
		CancellationToken cancellationToken = default
		);

	/// <summary>
	/// Returns a collection of <typeparamref name="TEntity"/> by their primary keys.
	/// </summary>
	/// <param name="ids">The primary keys of the <typeparamref name="TEntity"/>.</param>
	/// <param name="ignoreQueryFilters">Should model-level entity query filters be applied?</param>
	/// <param name="trackChanges">Should the fetched entities be tracked?</param>
	/// <returns>A collection of <typeparamref name="TEntity"/>.</returns>
	IEnumerable<TEntity> GetByIds(
		IEnumerable<TKey> ids,
		bool ignoreQueryFilters = false,
		bool trackChanges = false
		);

	/// <summary>
	/// Returns a collection of <typeparamref name="TEntity"/> by their primary keys.
	/// </summary>
	/// <param name="ids">The primary key of the <typeparamref name="TEntity"/>.</param>
	/// <param name="ignoreQueryFilters">Should model-level entity query filters be applied?</param>
	/// <param name="trackChanges">Should the fetched entities be tracked?</param>
	/// <param name="cancellationToken">The cancellation token to cancel the request.</param>
	/// <returns>A collection of <typeparamref name="TEntity"/>.</returns>
	Task<IEnumerable<TEntity>> GetByIdsAsync(
		IEnumerable<TKey> ids,
		bool ignoreQueryFilters = false,
		bool trackChanges = false,
		CancellationToken cancellationToken = default
		);

	/// <summary>
	/// Updates the database row for the <typeparamref name="TEntity"/> instance which match the <paramref name="id"/>
	/// from the database.
	/// </summary>
	/// <param name="id">The primary key of the <typeparamref name="TEntity"/>.</param>
	/// <param name="setPropertyCalls">A collection of set property statements specifying properties to update.</param>
	/// <returns>The total number of rows updated in the database.</returns>
	int Update(
		TKey id,
		Expression<Func<SetPropertyCalls<TEntity>, SetPropertyCalls<TEntity>>> setPropertyCalls
		);

	/// <summary>
	/// Updates all database rows for the <typeparamref name="TEntity"/> instances which match the <paramref name="ids"/>
	/// from the database.
	/// </summary>
	/// <param name="ids">The primary key of the <typeparamref name="TEntity"/>.</param>
	/// <param name="setPropertyCalls">A collection of set property statements specifying properties to update.</param>
	/// <returns>The total number of rows updated in the database.</returns>
	int Update(
		IEnumerable<TKey> ids,
		Expression<Func<SetPropertyCalls<TEntity>, SetPropertyCalls<TEntity>>> setPropertyCalls
		);

	/// <summary>
	/// Updates the database row for the <typeparamref name="TEntity"/> instance which match the <paramref name="id"/>.
	/// </summary>
	/// <param name="id">The primary key of the <typeparamref name="TEntity"/>.</param>
	/// <param name="setPropertyCalls">A collection of set property statements specifying properties to update.</param>
	/// <param name="cancellationToken">The cancellation token to cancel the request.</param>
	/// <returns>The total number of rows updated in the database.</returns>
	Task<int> UpdateAsync(
		TKey id,
		Expression<Func<SetPropertyCalls<TEntity>, SetPropertyCalls<TEntity>>> setPropertyCalls,
		CancellationToken cancellationToken = default
		);

	/// <summary>
	/// Updates all database rows for the <typeparamref name="TEntity"/> instances which match the <paramref name="ids"/>
	/// from the database.
	/// </summary>
	/// <param name="ids">The primary key of the <typeparamref name="TEntity"/>.</param>
	/// <param name="setPropertyCalls">A collection of set property statements specifying properties to update.</param>
	/// <param name="cancellationToken">The cancellation token to cancel the request.</param>
	/// <returns>The total number of rows updated in the database.</returns>
	Task<int> UpdateAsync(
		IEnumerable<TKey> ids,
		Expression<Func<SetPropertyCalls<TEntity>, SetPropertyCalls<TEntity>>> setPropertyCalls,
		CancellationToken cancellationToken = default
		);
}

/// <inheritdoc/>
public interface IIdentityRepository<TEntity> : IIdentityRepository<TEntity, Guid>
	where TEntity : class, IIdentityModel
{ }
