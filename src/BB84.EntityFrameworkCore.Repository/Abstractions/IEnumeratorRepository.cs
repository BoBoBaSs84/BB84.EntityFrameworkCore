using BB84.EntityFrameworkCore.Models.Abstractions;

namespace BB84.EntityFrameworkCore.Repository.Abstractions;

/// <summary>
/// The enumerator repository interface.
/// </summary>
/// <inheritdoc cref="IIdentityRepository{TEntity}"/>
/// <remarks>
/// <typeparamref name="TEntity"/> must implement the <see cref="IEnumeratorModel"/> interface.
/// </remarks>
public interface IEnumeratorRepository<TEntity> : IIdentityRepository<TEntity, int> where TEntity : class, IEnumeratorModel
{
  /// <summary>
  /// Returns an <typeparamref name="TEntity"/> by its name.
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
  /// Returns an <typeparamref name="TEntity"/> by its name.
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
  /// Returns a collection of <typeparamref name="TEntity"/> by their names.
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
  /// Returns a collection of <typeparamref name="TEntity"/> by their names.
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
