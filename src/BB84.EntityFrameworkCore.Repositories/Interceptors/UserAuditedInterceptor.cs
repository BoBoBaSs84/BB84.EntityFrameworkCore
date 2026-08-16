// Copyright: 2024 Robert Peter Meyer
// License: MIT
//
// This source code is licensed under the MIT license found in the
// LICENSE file in the root directory of this source tree.
using BB84.EntityFrameworkCore.Entities.Abstractions.Components;
using BB84.EntityFrameworkCore.Repositories.Abstractions;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace BB84.EntityFrameworkCore.Repositories.Interceptors;

/// <summary>
/// A save changes interceptor that automatically fills the audit columns for entities
/// implementing the <see cref="IUserAudited{TCreator, TEditor}"/> interface.
/// </summary>
/// <remarks>
/// <para>
/// This interceptor sets the creator when an entity is added and the last editor when an
/// entity is modified, using the identity returned by the supplied provider.
/// </para>
/// <para>
/// The provider is asked once per save operation, so every entity taking part in one save
/// records the same identity.
/// </para>
/// </remarks>
/// <typeparam name="TUser">The type representing the user.</typeparam>
/// <param name="currentUserProvider">The provider supplying the current user.</param>
/// <inheritdoc cref="SaveChangesInterceptor"/>
public class UserAuditedInterceptor<TUser>(ICurrentUserProvider<TUser> currentUserProvider) : SaveChangesInterceptor
	where TUser : notnull
{
	private readonly ICurrentUserProvider<TUser> _currentUserProvider = currentUserProvider;

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
	/// Intercepts and processes entities implementing <see cref="IUserAudited{TCreator, TEditor}"/>
	/// in the specified <see cref="DbContext"/>.
	/// </summary>
	/// <param name="dbContext">
	/// The <see cref="DbContext"/> instance whose tracked entities are to be intercepted.
	/// </param>
	private void InterceptEntities(DbContext? dbContext)
	{
		if (dbContext is not null)
		{
			IEnumerable<EntityEntry<IUserAudited<TUser, TUser?>>> entityEntries = dbContext.ChangeTracker.Entries<IUserAudited<TUser, TUser?>>();
			TUser currentUser = _currentUserProvider.GetCurrentUser();

			foreach (EntityEntry<IUserAudited<TUser, TUser?>> entityEntry in entityEntries)
			{
				switch (entityEntry.State)
				{
					case EntityState.Added:
						entityEntry.Entity.CreatedBy = currentUser;
						continue;
					case EntityState.Modified:
						entityEntry.Entity.EditedBy = currentUser;
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

/// <inheritdoc cref="UserAuditedInterceptor{TUser}"/>
/// <remarks>
/// The convenience alias for the default <see cref="string"/> based audit columns.
/// </remarks>
/// <param name="currentUserProvider">The provider supplying the current user.</param>
public sealed class UserAuditedInterceptor(ICurrentUserProvider currentUserProvider) : UserAuditedInterceptor<string>(currentUserProvider)
{ }
