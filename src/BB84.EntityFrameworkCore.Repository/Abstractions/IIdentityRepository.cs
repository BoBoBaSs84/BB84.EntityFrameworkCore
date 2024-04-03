using BB84.EntityFrameworkCore.Models.Abstractions;

namespace BB84.EntityFrameworkCore.Repository.Abstractions;

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
	/// <param name="cancellationToken">The cancellation token.</param>
	/// <returns>A collection of <typeparamref name="TEntity"/>.</returns>
	Task<IEnumerable<TEntity>> GetByIdsAsync(
		IEnumerable<TKey> ids,
		bool ignoreQueryFilters = false,
		bool trackChanges = false,
		CancellationToken cancellationToken = default
		);
}

/// <inheritdoc/>
public interface IIdentityRepository<TEntity> : IIdentityRepository<TEntity, Guid>
	where TEntity : class, IIdentityModel
{ }