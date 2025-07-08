using BB84.EntityFrameworkCore.Entities.Abstractions;

using Microsoft.EntityFrameworkCore;

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace BB84.EntityFrameworkCore.Repositories.Abstractions;
#pragma warning restore IDE0130 // Namespace does not match folder structure

/// <summary>
/// Represents a repository contract for managing hierarchical entities.
/// </summary>
/// <typeparam name="TEntity">
/// The type of the entity that implements <see cref="IHierarchyEntity"/>.
/// </typeparam>
public interface IHierarchyRepository<TEntity> : IGenericRepository<TEntity>
		where TEntity : class, IHierarchyEntity
{
	IEnumerable<TEntity?> GetAncestor(
		int level,
		bool ignoreQueryFilters = false,
		bool trackChanges = false,
		params string[] includeProperties
		);

	Task<IEnumerable<TEntity?>> GetAncestorAsync(
		int level,
		bool ignoreQueryFilters = false,
		bool trackChanges = false,
		CancellationToken token = default,
		params string[] includeProperties
		);

	TEntity? GetById(
		HierarchyId id,
		bool ignoreQueryFilters = false,
		bool trackChanges = false,
		params string[] includeProperties
		);

	Task<TEntity?> GetByIdAsync(
		HierarchyId id,
		bool ignoreQueryFilters = false,
		bool trackChanges = false,
		CancellationToken token = default,
		params string[] includeProperties
		);

	IEnumerable<TEntity> GetByIds(
		IEnumerable<HierarchyId> ids,
		bool ignoreQueryFilters = false,
		bool trackChanges = false,
		params string[] includeProperties
		);

	Task<IEnumerable<TEntity>> GetByIdsAsync(
		IEnumerable<HierarchyId> ids,
		bool ignoreQueryFilters = false,
		bool trackChanges = false,
		CancellationToken token = default,
		params string[] includeProperties
		);

	IEnumerable<TEntity> GetDescendants(
		HierarchyId? childId1,
		HierarchyId? childId2,
		bool ignoreQueryFilters = false,
		bool trackChanges = false,
		params string[] includeProperties
		);

	Task<IEnumerable<TEntity>> GetDescendantsAsync(
		HierarchyId? childId1,
		HierarchyId? childId2,
		bool ignoreQueryFilters = false,
		bool trackChanges = false,
		CancellationToken token = default,
		params string[] includeProperties
		);
}
