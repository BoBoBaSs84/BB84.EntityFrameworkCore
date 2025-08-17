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
/// <see cref="IAuditedCompositeEntity{TCreator, TEdited}"/> interface.
/// </summary>
/// <remarks>
/// This abstract class provides a reusable configuration for audited composite entities,
/// ensuring that the <c>Timestamp</c>, <c>Creator</c>, and <c>Editor</c> properties are consistently
/// configured across all derived entity types. The <c>Timestamp</c> property is marked as a concurrency
/// token and is automatically updated on add or update operations. The <c>Creator</c> property is required,
/// while the <c>Editor</c> property is optional.
/// </remarks>
/// <typeparam name="TEntity">The type of the entity being configured.</typeparam>
/// <typeparam name="TCreator">The type representing the creator of the entity.</typeparam>
/// <typeparam name="TEdited">The type representing the editor of the entity.</typeparam>
/// <inheritdoc cref="IEntityTypeConfiguration{TEntity}"/>
[SuppressMessage("Style", "IDE0058", Justification = "Not relevant here, entity type configuration.")]
public abstract class AuditedCompositeConfiguration<TEntity, TCreator, TEdited> : IEntityTypeConfiguration<TEntity>
	where TEntity : class, IAuditedCompositeEntity<TCreator, TEdited>
	where TCreator : notnull
{
	/// <inheritdoc/>
	public virtual void Configure(EntityTypeBuilder<TEntity> builder)
	{
		builder.Property(e => e.Timestamp)
			.HasColumnOrder(3)
			.IsConcurrencyToken()
			.ValueGeneratedOnAddOrUpdate();

		builder.Property(e => e.CreatedBy)
			.HasColumnOrder(4)
			.IsRequired();

		builder.Property(e => e.EditedBy)
			.HasColumnOrder(5)
			.IsRequired(false);
	}
}

/// <inheritdoc cref="AuditedCompositeConfiguration{TEntity, TCreator, TEdited}"/>
[SuppressMessage("Style", "IDE0058", Justification = "Not relevant here, entity type configuration.")]
public abstract class AuditedCompositeConfiguration<TEntity> : AuditedCompositeConfiguration<TEntity, string, string?>, IEntityTypeConfiguration<TEntity>
	where TEntity : class, IAuditedCompositeEntity
{
	/// <inheritdoc/>
	public override void Configure(EntityTypeBuilder<TEntity> builder)
	{
		base.Configure(builder);

		builder.Property(e => e.CreatedBy)
			.IsSysNameColumn();

		builder.Property(e => e.EditedBy)
			.IsSysNameColumn();
	}
}
