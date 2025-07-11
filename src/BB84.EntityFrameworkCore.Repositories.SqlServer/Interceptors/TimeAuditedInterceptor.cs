// Copyright: 2024 Robert Peter Meyer
// License: MIT
//
// This source code is licensed under the MIT license found in the
// LICENSE file in the root directory of this source tree.
using BB84.EntityFrameworkCore.Entities.Abstractions.Components;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace BB84.EntityFrameworkCore.Repositories.SqlServer.Interceptors;

/// <summary>
/// A save changes interceptor that automatically updates audit timestamps for entities
/// implementing the <see cref="ITimeAudited"/> interface.
/// </summary>
/// <remarks>
/// This interceptor updates the <see cref="ITimeAudited.Created"/> property to the current
/// UTC time when an entity is added and the <see cref="ITimeAudited.Edited"/> property to
/// the current UTC time when an entity is modified.
/// </remarks>
/// <inheritdoc cref="SaveChangesInterceptor"/>
public sealed class TimeAuditedInterceptor : SaveChangesInterceptor
{
	/// <inheritdoc/>
	public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
	{
		InterceptEntities(eventData.Context);
		return base.SavingChanges(eventData, result);
	}

	/// <inheritdoc/>
	public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
	{
		InterceptEntities(eventData.Context);
		return base.SavingChangesAsync(eventData, result, cancellationToken);
	}

	/// <summary>
	/// Intercepts and processes entities implementing <see cref="ITimeAudited"/> in the
	/// specified <see cref="DbContext"/>.
	/// </summary>
	/// <param name="dbContext">
	/// The <see cref="DbContext"/> instance whose tracked entities are to be intercepted.
	/// </param>
	private static void InterceptEntities(DbContext? dbContext)
	{
		if (dbContext is not null)
		{
			IEnumerable<EntityEntry<ITimeAudited>> entityEntries = dbContext.ChangeTracker.Entries<ITimeAudited>();

			foreach (EntityEntry<ITimeAudited> entityEntry in entityEntries)
			{
				switch (entityEntry.State)
				{
					case EntityState.Added:
						entityEntry.Entity.Created = DateTime.UtcNow;
						continue;
					case EntityState.Modified:
						entityEntry.Entity.Edited = DateTime.UtcNow;
						continue;
					case EntityState.Detached:
					case EntityState.Unchanged:
					case EntityState.Deleted:
					default:
						break;
				}
			}
		}
	}
}
