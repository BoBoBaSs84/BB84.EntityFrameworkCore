// Copyright: 2024 Robert Peter Meyer
// License: MIT
//
// This source code is licensed under the MIT license found in the
// LICENSE file in the root directory of this source tree.
using System.Diagnostics.CodeAnalysis;

using BB84.EntityFrameworkCore.Entities.Abstractions;
using BB84.EntityFrameworkCore.Repositories.SqlServer.Extensions;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BB84.EntityFrameworkCore.Repositories.SqlServer.Configurations;

/// <summary>
/// Provides a base configuration for entities that implement the
/// <see cref="IFullAuditedEntity{TKey, TCreator, TEdited}"/> interface.
/// </summary>
/// <remarks>
/// This configuration defines common properties for audited entities, including primary key,
/// timestamps and audit fields such as creator and editor. It is intended to be used as a
/// base class for configuring entities that require full auditing support.
/// </remarks>
/// <typeparam name="TEntity">The type of the entity being configured.</typeparam>
/// <typeparam name="TKey">The type of the primary key for the entity.</typeparam>
/// <typeparam name="TCreator">The type representing the creator of the entity.</typeparam>
/// <typeparam name="TEdited">The type representing the editor of the entity.</typeparam>
[SuppressMessage("Style", "IDE0058", Justification = "Not relevant here, entity type configuration.")]
public abstract class FullAuditedConfiguration<TEntity, TKey, TCreator, TEdited> : IEntityTypeConfiguration<TEntity>
	where TEntity : class, IFullAuditedEntity<TKey, TCreator, TEdited>
	where TKey : IEquatable<TKey>
	where TCreator : notnull
{
	/// <inheritdoc/>
	public virtual void Configure(EntityTypeBuilder<TEntity> builder)
	{
		EntityTypeBuilderDefaults.ApplyIdentityKey<TEntity, TKey>(builder);
		EntityTypeBuilderDefaults.ApplyConcurrencyToken(builder, columnOrder: 2);
		EntityTypeBuilderDefaults.ApplyUserAuditColumns<TEntity, TCreator, TEdited>(builder, createdByOrder: 3, editedByOrder: 5);

		builder.Property(p => p.CreatedAt)
			.HasColumnOrder(4)
			.IsRequired();

		builder.Property(p => p.EditedAt)
			.HasColumnOrder(6)
			.IsRequired(false);
	}
}

/// <inheritdoc cref="FullAuditedConfiguration{TEntity, TKey, TCreator, TEdited}"/>
[SuppressMessage("Style", "IDE0058", Justification = "Not relevant here, entity type configuration.")]
public abstract class FullAuditedConfiguration<TEntity, TKey> : FullAuditedConfiguration<TEntity, TKey, string, string?>, IEntityTypeConfiguration<TEntity>
	where TEntity : class, IFullAuditedEntity<TKey>
	where TKey : IEquatable<TKey>
{
	/// <inheritdoc/>
	public override void Configure(EntityTypeBuilder<TEntity> builder)
	{
		base.Configure(builder);

		EntityTypeBuilderDefaults.ApplySysNameAuditColumns(builder);
	}
}

/// <inheritdoc cref="FullAuditedConfiguration{TEntity, TKey, TCreator, TEdited}"/>
[SuppressMessage("Style", "IDE0058", Justification = "Not relevant here, entity type configuration.")]
public abstract class FullAuditedConfiguration<TEntity, TCreator, TEdited> : FullAuditedConfiguration<TEntity, Guid, TCreator, TEdited>, IEntityTypeConfiguration<TEntity>
	where TEntity : class, IFullAuditedEntity<TCreator, TEdited>
	where TCreator : notnull
{
	/// <inheritdoc/>
	public override void Configure(EntityTypeBuilder<TEntity> builder)
	{
		base.Configure(builder);

		EntityTypeBuilderDefaults.ApplyGuidIdDefault(builder);
	}
}

/// <inheritdoc cref="FullAuditedConfiguration{TEntity, TKey, TCreator, TEdited}"/>
[SuppressMessage("Style", "IDE0058", Justification = "Not relevant here, entity type configuration.")]
public abstract class FullAuditedConfiguration<TEntity> : FullAuditedConfiguration<TEntity, Guid, string, string?>, IEntityTypeConfiguration<TEntity>
	where TEntity : class, IFullAuditedEntity
{
	/// <inheritdoc/>
	public override void Configure(EntityTypeBuilder<TEntity> builder)
	{
		base.Configure(builder);

		EntityTypeBuilderDefaults.ApplyGuidIdDefault(builder);
		EntityTypeBuilderDefaults.ApplySysNameAuditColumns(builder);
	}
}
