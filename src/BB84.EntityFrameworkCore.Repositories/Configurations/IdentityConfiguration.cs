// Copyright: 2024 Robert Peter Meyer
// License: MIT
//
// This source code is licensed under the MIT license found in the
// LICENSE file in the root directory of this source tree.
using BB84.EntityFrameworkCore.Entities.Abstractions;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BB84.EntityFrameworkCore.Repositories.Configurations;

/// <summary>
/// Represents an abstract base class for configuring entity types that implement the
/// <see cref="IIdentityEntity{Tkey}"/> interface.
/// </summary>
/// <remarks>
/// This class defines a default configuration for identity-related entities, including the primary key and
/// concurrency token. Derived classes can override the <see cref="Configure"/> method to provide additional
/// or customized configuration.
/// </remarks>
/// <typeparam name="TEntity">The type of the entity being configured.</typeparam>
/// <typeparam name="TKey">The type of the primary key for the entity.</typeparam>
public abstract class IdentityConfiguration<TEntity, TKey> : IEntityTypeConfiguration<TEntity>
	where TEntity : class, IIdentityEntity<TKey>
	where TKey : IEquatable<TKey>
{
	/// <inheritdoc/>
	public virtual void Configure(EntityTypeBuilder<TEntity> builder)
	{
		EntityTypeBuilderDefaults.ApplyIdentityKey<TEntity, TKey>(builder);
		EntityTypeBuilderDefaults.ApplyConcurrencyToken(builder, columnOrder: 2);
	}
}

/// <inheritdoc cref="IdentityConfiguration{TEntity, TKey}"/>
public abstract class IdentityConfiguration<TEntity> : IdentityConfiguration<TEntity, Guid>, IEntityTypeConfiguration<TEntity>
	where TEntity : class, IIdentityEntity
{ }
