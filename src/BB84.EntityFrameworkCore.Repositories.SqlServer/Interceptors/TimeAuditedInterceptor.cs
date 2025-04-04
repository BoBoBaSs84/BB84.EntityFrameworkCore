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
/// The save changes interceptor for time audited entities.
/// </summary>
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

	private static void InterceptEntities(DbContext? dbContext)
	{
		if (dbContext is not null)
		{
			IEnumerable<EntityEntry<ITimeAudited>> entries = dbContext.ChangeTracker.Entries<ITimeAudited>();

			foreach (EntityEntry<ITimeAudited> entry in entries)
			{
				switch (entry.State)
				{
					case EntityState.Added:
						entry.Entity.Created = DateTime.UtcNow;
						continue;
					case EntityState.Modified:
						entry.Entity.Edited = DateTime.UtcNow;
						continue;
				}
			}
		}
	}
}
