// Copyright: 2024 Robert Peter Meyer
// License: MIT
//
// This source code is licensed under the MIT license found in the
// LICENSE file in the root directory of this source tree.
using System.Linq.Expressions;

namespace BB84.EntityFrameworkCore.Repositories.Abstractions;

/// <summary>
/// Describes how a read operation composes its query: what to filter by, what to include,
/// how to order, how much to skip and take, and how to treat tracking and query filters.
/// </summary>
/// <remarks>
/// <para>
/// Every option is <b>optional</b>. An instance with nothing set means "everything, untracked,
/// with the global query filters applied", so passing no query at all reads the whole set.
/// </para>
/// <para>
/// The options are applied in a fixed order: <see cref="Where"/>, <see cref="QueryFilter"/>,
/// <see cref="IgnoreQueryFilters"/>, <see cref="Include"/>, <see cref="OrderBy"/>,
/// <see cref="Skip"/>, <see cref="Take"/>. Ordering therefore happens before paging, which is
/// the only combination of the two that is meaningful.
/// </para>
/// <example>
/// <code>
/// IReadOnlyList&lt;PersonEntity&gt; people = await repository.GetListAsync(
///     new()
///     {
///         Where = x =&gt; x.IsActive,
///         OrderBy = q =&gt; q.OrderBy(x =&gt; x.LastName),
///         Include = [x =&gt; x.Jobs],
///         Take = 50,
///     },
///     cancellationToken);
/// </code>
/// </example>
/// </remarks>
/// <typeparam name="TEntity">The type of the entity being queried.</typeparam>
public sealed record Query<TEntity>
	where TEntity : class
{
	/// <summary>
	/// The condition an entity has to fulfill to be selected.
	/// </summary>
	public Expression<Func<TEntity, bool>>? Where { get; init; }

	/// <summary>
	/// An additional composition step applied to the query.
	/// </summary>
	/// <remarks>
	/// The escape hatch for anything the other options cannot express, such as
	/// <c>ThenInclude</c>, grouping or a join. Applied after <see cref="Where"/>.
	/// </remarks>
	public Func<IQueryable<TEntity>, IQueryable<TEntity>>? QueryFilter { get; init; }

	/// <summary>
	/// The related data to load along with the entity.
	/// </summary>
	/// <remarks>
	/// Each expression names a navigation property, for example <c>x =&gt; x.Jobs</c>. Use
	/// <see cref="QueryFilter"/> instead when a nested <c>ThenInclude</c> is needed.
	/// </remarks>
	public IReadOnlyList<Expression<Func<TEntity, object?>>>? Include { get; init; }

	/// <summary>
	/// The ordering applied to the query.
	/// </summary>
	public Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? OrderBy { get; init; }

	/// <summary>
	/// The number of entities to bypass.
	/// </summary>
	public int? Skip { get; init; }

	/// <summary>
	/// The number of entities to return.
	/// </summary>
	public int? Take { get; init; }

	/// <summary>
	/// Whether the returned entities are tracked by the change tracker.
	/// </summary>
	/// <remarks>
	/// Defaults to <see langword="false"/>. Leave it off for reads that are not going to be
	/// written back, and especially for streaming reads, where tracking makes the change
	/// tracker grow with every entity yielded.
	/// </remarks>
	public bool TrackChanges { get; init; }

	/// <summary>
	/// Whether the global query filters are ignored.
	/// </summary>
	/// <remarks>
	/// Defaults to <see langword="false"/>. Set it to <see langword="true"/> to read rows that
	/// a global filter would otherwise hide, such as soft deleted ones.
	/// </remarks>
	public bool IgnoreQueryFilters { get; init; }
}
