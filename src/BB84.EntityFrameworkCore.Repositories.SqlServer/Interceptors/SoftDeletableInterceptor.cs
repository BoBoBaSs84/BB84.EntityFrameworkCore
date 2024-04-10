using BB84.EntityFrameworkCore.Models.Abstractions.Components;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace BB84.EntityFrameworkCore.Repositories.SqlServer.Interceptors;

/// <summary>
/// The soft deletable interceptor class.
/// </summary>
/// <inheritdoc/>
public sealed class SoftDeletableInterceptor : SaveChangesInterceptor
{
	/// <inheritdoc/>
	public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
	{
		UpdateEntities(eventData.Context);
		return base.SavingChanges(eventData, result);
	}

	/// <inheritdoc/>
	public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
	{
		UpdateEntities(eventData.Context);
		return base.SavingChangesAsync(eventData, result, cancellationToken);
	}

	private static void UpdateEntities(DbContext? context)
	{
		if (context is null)
			return;

		IEnumerable<EntityEntry<ISoftDeletable>> entites = context.ChangeTracker.Entries<ISoftDeletable>();

		foreach (EntityEntry<ISoftDeletable> entry in entites)
		{
			if (entry.State is EntityState.Deleted)
			{
				entry.Entity.IsDeleted = true;
				entry.State = EntityState.Modified;
			}
		}
	}
}
