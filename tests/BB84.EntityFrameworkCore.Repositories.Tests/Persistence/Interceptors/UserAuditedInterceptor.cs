using BB84.EntityFrameworkCore.Entities.Abstractions.Components;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace BB84.EntityFrameworkCore.RepositoriesTests.Persistence.Interceptors;

public sealed class UserAuditedInterceptor : SaveChangesInterceptor
{
	public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
	{
		InterceptEntities(eventData.Context);
		return base.SavingChanges(eventData, result);
	}

	public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
	{
		InterceptEntities(eventData.Context);
		return base.SavingChangesAsync(eventData, result, cancellationToken);
	}

	private static void InterceptEntities(DbContext? dbContext)
	{
		if (dbContext is not null)
		{
			IEnumerable<EntityEntry<IUserAudited>> entries = dbContext.ChangeTracker.Entries<IUserAudited>();
			string userName = $"{Environment.MachineName}\\{Environment.UserName}";

			foreach (EntityEntry<IUserAudited> entry in entries)
			{
				switch (entry.State)
				{
					case EntityState.Added:
						entry.Entity.Creator = userName;
						continue;
					case EntityState.Modified:
						entry.Entity.Editor = userName;
						continue;
				}
			}
		}
	}
}
