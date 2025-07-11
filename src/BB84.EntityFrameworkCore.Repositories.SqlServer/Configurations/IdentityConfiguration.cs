// Copyright: 2024 Robert Peter Meyer
// License: MIT
//
// This source code is licensed under the MIT license found in the
// LICENSE file in the root directory of this source tree.
using System.Diagnostics.CodeAnalysis;

using BB84.EntityFrameworkCore.Entities.Abstractions;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BB84.EntityFrameworkCore.Repositories.SqlServer.Configurations;

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
/// <inheritdoc cref="IEntityTypeConfiguration{TEntity}"/>
[SuppressMessage("Style", "IDE0058", Justification = "Not relevant here, entity type configuration.")]
public abstract class IdentityConfiguration<TEntity, TKey> : IEntityTypeConfiguration<TEntity>
	where TEntity : class, IIdentityEntity<TKey>
	where TKey : IEquatable<TKey>
{
	/// <inheritdoc/>
	public virtual void Configure(EntityTypeBuilder<TEntity> builder)
	{
		builder.HasKey(e => e.Id)
			.IsClustered(false);

		builder.Property(e => e.Id)
			.HasColumnOrder(1)
			.ValueGeneratedOnAdd();

		builder.Property(e => e.Timestamp)
			.IsConcurrencyToken()
			.HasColumnOrder(2)
			.ValueGeneratedOnAddOrUpdate();
	}
}

/// <inheritdoc cref="IdentityConfiguration{TEntity, TKey}"/>
[SuppressMessage("Style", "IDE0058", Justification = "Not relevant here, entity type configuration.")]
public abstract class IdentityConfiguration<TEntity> : IdentityConfiguration<TEntity, Guid>, IEntityTypeConfiguration<TEntity>
	where TEntity : class, IIdentityEntity
{
	/// <inheritdoc/>
	public override void Configure(EntityTypeBuilder<TEntity> builder)
	{
		base.Configure(builder);

		builder.Property(p => p.Id)
			.HasDefaultValueSql("NEWID()");
	}
}
