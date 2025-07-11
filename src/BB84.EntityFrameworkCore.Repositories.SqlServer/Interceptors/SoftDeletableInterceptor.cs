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
/// Intercepts save operations in a <see cref="DbContext"/> to enforce soft delete behavior for
/// entities implementing the <see cref="ISoftDeletable"/> interface.
/// </summary>
/// <remarks>
/// This interceptor modifies the behavior of entities marked for deletion by setting their
/// <see cref="ISoftDeletable.IsDeleted"/> property to <see langword="true"/> and changing their
/// state to <see cref="EntityState.Modified"/>. This ensures that soft-deleted entities are not
/// physically removed from the database.
/// </remarks>
/// <inheritdoc cref="SaveChangesInterceptor"/>
public sealed class SoftDeletableInterceptor : SaveChangesInterceptor
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
	/// Intercepts and processes entities implementing <see cref="ISoftDeletable"/> in the
	/// specified <see cref="DbContext"/>.
	/// </summary>
	/// <param name="dbContext">
	/// The <see cref="DbContext"/> instance whose tracked entities are to be intercepted.
	/// </param>
	private static void InterceptEntities(DbContext? dbContext)
	{
		if (dbContext is not null)
		{
			IEnumerable<EntityEntry<ISoftDeletable>> entityEntries = dbContext.ChangeTracker.Entries<ISoftDeletable>();

			foreach (EntityEntry<ISoftDeletable> entityEntry in entityEntries)
			{
				switch (entityEntry.State)
				{
					case EntityState.Deleted:
						entityEntry.Entity.IsDeleted = true;
						entityEntry.State = EntityState.Modified;
						break;
					case EntityState.Detached:
					case EntityState.Unchanged:
					case EntityState.Modified:
					case EntityState.Added:
					default:
						break;
				}
			}
		}
	}
}
