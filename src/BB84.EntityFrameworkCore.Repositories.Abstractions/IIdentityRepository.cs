using System.Linq.Expressions;

using BB84.EntityFrameworkCore.Models.Abstractions;

using Microsoft.EntityFrameworkCore.Query;

namespace BB84.EntityFrameworkCore.Repositories.Abstractions;

/// <summary>
/// The identity repository interface.
/// </summary>
/// <remarks>
/// <typeparamref name="TEntity"/> must implement the <see cref="IIdentityModel{TKey}"/> interface.
/// </remarks>
/// <inheritdoc cref="IGenericRepository{TEntity}"/>
/// <inheritdoc cref="IIdentityModel{TKey}"/>
public interface IIdentityRepository<TEntity, TKey> : IGenericRepository<TEntity> where TEntity : class, IIdentityModel<TKey> where TKey : IEquatable<TKey>
{
	/// <summary>
	/// Deletes the database row for the <typeparamref name="TEntity"/> instance which matches
	/// the <paramref name="id"/> from the database.
	/// </summary>
	/// <remarks>
	/// This operation executes immediately against the database, rather than being deferred
	/// until save changes is called. It also does not interact with the EF change tracker in
	/// any way: entity instances which happen to be tracked when this operation is invoked
	/// aren't taken into account, and aren't updated to reflect the changes.
	/// </remarks>	
	/// <param name="id">The primary key of the <typeparamref name="TEntity"/>.</param>
	/// <returns>The total number of rows deleted in the database.</returns>
	int Delete(TKey id);

	/// <summary>
	/// Deletes all database rows for the <typeparamref name="TEntity"/> instances which matches
	/// the <paramref name="ids"/> from the database.
	/// </summary>
	/// <remarks>
	/// This operation executes immediately against the database, rather than being deferred
	/// until save changes is called. It also does not interact with the EF change tracker in
	/// any way: entity instances which happen to be tracked when this operation is invoked
	/// aren't taken into account, and aren't updated to reflect the changes.
	/// </remarks>	
	/// <param name="ids">The primary keys of the <typeparamref name="TEntity"/>.</param>
	/// <returns>The total number of rows deleted in the database.</returns>
	int Delete(IEnumerable<TKey> ids);

	/// <summary>
	/// Deletes the database row for the <typeparamref name="TEntity"/> instance which matches
	/// the <paramref name="id"/> from the database.
	/// </summary>
	/// <remarks>
	/// This operation executes immediately against the database, rather than being deferred
	/// until save changes is called. It also does not interact with the EF change tracker in
	/// any way: entity instances which happen to be tracked when this operation is invoked
	/// aren't taken into account, and aren't updated to reflect the changes.
	/// </remarks>
	/// <param name="id">The primary key of the <typeparamref name="TEntity"/>.</param>
	/// <param name="token">The cancellation token to cancel the request.</param>
	/// <returns>The total number of rows deleted in the database.</returns>
	Task<int> DeleteAsync(TKey id, CancellationToken token = default);

	/// <summary>
	/// Deletes all database rows for the <typeparamref name="TEntity"/> instances which matches
	/// the <paramref name="ids"/> from the database.
	/// </summary>
	/// <remarks>
	/// This operation executes immediately against the database, rather than being deferred
	/// until save changes is called. It also does not interact with the EF change tracker in
	/// any way: entity instances which happen to be tracked when this operation is invoked
	/// aren't taken into account, and aren't updated to reflect the changes.
	/// </remarks>	
	/// <param name="ids">The primary keys of the <typeparamref name="TEntity"/>.</param>
	/// <param name="token">The cancellation token to cancel the request.</param>
	/// <returns>The total number of rows deleted in the database.</returns>
	Task<int> DeleteAsync(IEnumerable<TKey> ids, CancellationToken token = default);

	/// <summary>
	/// Returns an <typeparamref name="TEntity"/> by its primary key.
	/// </summary>
	/// <param name="id">The primary key of the <typeparamref name="TEntity"/>.</param>
	/// <param name="ignoreQueryFilters">Should model-level entity query filters be applied?</param>
	/// <param name="trackChanges">Should the fetched entity be tracked?</param>
	/// <param name="includeProperties">Any other navigation properties to include when returning the collection.</param>
	/// <returns>An <typeparamref name="TEntity"/> or <see langword="null"/>.</returns>
	TEntity? GetById(
		TKey id,
		bool ignoreQueryFilters = false,
		bool trackChanges = false,
		params string[] includeProperties
		);

	/// <summary>
	/// Returns an <typeparamref name="TEntity"/> by its primary key.
	/// </summary>
	/// <param name="id">The primary key of the <typeparamref name="TEntity"/>.</param>
	/// <param name="ignoreQueryFilters">Should model-level entity query filters be applied?</param>
	/// <param name="trackChanges">Should the fetched entity be tracked?</param>
	/// <param name="token">The cancellation token to cancel the request.</param>
	/// <param name="includeProperties">Any other navigation properties to include when returning the collection.</param>
	/// <returns>The <typeparamref name="TEntity"/> or <see langword="null"/>.</returns>
	Task<TEntity?> GetByIdAsync(
		TKey id,
		bool ignoreQueryFilters = false,
		bool trackChanges = false,
		CancellationToken token = default,
		params string[] includeProperties
		);

	/// <summary>
	/// Returns a collection of <typeparamref name="TEntity"/> by their primary keys.
	/// </summary>
	/// <param name="ids">The primary keys of the <typeparamref name="TEntity"/>.</param>
	/// <param name="ignoreQueryFilters">Should model-level entity query filters be applied?</param>
	/// <param name="trackChanges">Should the fetched entities be tracked?</param>
	/// <param name="includeProperties">Any other navigation properties to include when returning the collection.</param>
	/// <returns>A collection of <typeparamref name="TEntity"/>.</returns>
	IEnumerable<TEntity> GetByIds(
		IEnumerable<TKey> ids,
		bool ignoreQueryFilters = false,
		bool trackChanges = false,
		params string[] includeProperties
		);

	/// <summary>
	/// Returns a collection of <typeparamref name="TEntity"/> by their primary keys.
	/// </summary>
	/// <param name="ids">The primary key of the <typeparamref name="TEntity"/>.</param>
	/// <param name="ignoreQueryFilters">Should model-level entity query filters be applied?</param>
	/// <param name="trackChanges">Should the fetched entities be tracked?</param>
	/// <param name="token">The cancellation token to cancel the request.</param>
	/// <param name="includeProperties">Any other navigation properties to include when returning the collection.</param>
	/// <returns>A collection of <typeparamref name="TEntity"/>.</returns>
	Task<IEnumerable<TEntity>> GetByIdsAsync(
		IEnumerable<TKey> ids,
		bool ignoreQueryFilters = false,
		bool trackChanges = false,
		CancellationToken token = default,
		params string[] includeProperties
		);

	/// <summary>
	/// Updates the database row for the <typeparamref name="TEntity"/> instance which matches
	/// the <paramref name="id"/> from the database.
	/// </summary>
	/// <param name="id">The primary key of the <typeparamref name="TEntity"/>.</param>
	/// <param name="setPropertyCalls">A collection of set property statements specifying properties to update.</param>
	/// <returns>The total number of rows updated in the database.</returns>
	int Update(
		TKey id,
		Expression<Func<SetPropertyCalls<TEntity>, SetPropertyCalls<TEntity>>> setPropertyCalls
		);

	/// <summary>
	/// Updates all database rows for the <typeparamref name="TEntity"/> instances which matches
	/// the <paramref name="ids"/> from the database.
	/// </summary>
	/// <param name="ids">The primary key of the <typeparamref name="TEntity"/>.</param>
	/// <param name="setPropertyCalls">A collection of set property statements specifying properties to update.</param>
	/// <returns>The total number of rows updated in the database.</returns>
	int Update(
		IEnumerable<TKey> ids,
		Expression<Func<SetPropertyCalls<TEntity>, SetPropertyCalls<TEntity>>> setPropertyCalls
		);

	/// <summary>
	/// Updates the database row for the <typeparamref name="TEntity"/> instance which matches
	/// the <paramref name="id"/> from the database.
	/// </summary>
	/// <param name="id">The primary key of the <typeparamref name="TEntity"/>.</param>
	/// <param name="setPropertyCalls">A collection of set property statements specifying properties to update.</param>
	/// <param name="token">The cancellation token to cancel the request.</param>
	/// <returns>The total number of rows updated in the database.</returns>
	Task<int> UpdateAsync(
		TKey id,
		Expression<Func<SetPropertyCalls<TEntity>, SetPropertyCalls<TEntity>>> setPropertyCalls,
		CancellationToken token = default
		);

	/// <summary>
	/// Updates all database rows for the <typeparamref name="TEntity"/> instances which matches
	/// the <paramref name="ids"/> from the database.
	/// </summary>
	/// <param name="ids">The primary key of the <typeparamref name="TEntity"/>.</param>
	/// <param name="setPropertyCalls">A collection of set property statements specifying properties to update.</param>
	/// <param name="token">The cancellation token to cancel the request.</param>
	/// <returns>The total number of rows updated in the database.</returns>
	Task<int> UpdateAsync(
		IEnumerable<TKey> ids,
		Expression<Func<SetPropertyCalls<TEntity>, SetPropertyCalls<TEntity>>> setPropertyCalls,
		CancellationToken token = default
		);
}

/// <inheritdoc/>
public interface IIdentityRepository<TEntity> : IIdentityRepository<TEntity, Guid> where TEntity : class, IIdentityModel
{ }
