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
/// Represents an abstract base class for configuring the entities of type
/// <see cref="IAuditedEntity{TKey, TCreator, TEdited}"/> for dentity-related and time audited
/// entities in the Entity Framework Core model.
/// </summary>
/// <remarks>
/// This class provides a default configuration for audited entities, including primary key setup,
/// concurrency token configuration, and required properties for creator and editor fields.
/// Derived classes can override the <see cref="Configure"/> method to customize the configuration.
/// </remarks>
/// <typeparam name="TEntity">The type of the entity being configured.</typeparam>
/// <typeparam name="TKey">The type of the primary key for the entity.</typeparam>
/// <typeparam name="TCreator">The type representing the creator of the entity.</typeparam>
/// <typeparam name="TEdited">The type representing the editor of the entity.</typeparam>
/// <inheritdoc cref="IEntityTypeConfiguration{TEntity}"/>
[SuppressMessage("Style", "IDE0058", Justification = "Not relevant here, entity type configuration.")]
public abstract class AuditedConfiguration<TEntity, TKey, TCreator, TEdited> : IEntityTypeConfiguration<TEntity>
	where TEntity : class, IAuditedEntity<TKey, TCreator, TEdited>
	where TKey : IEquatable<TKey>
	where TCreator : notnull
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
			.HasColumnOrder(2)
			.IsConcurrencyToken()
			.ValueGeneratedOnAddOrUpdate();

		builder.Property(e => e.CreatedBy)
			.HasColumnOrder(3)
			.IsRequired();

		builder.Property(e => e.EditedBy)
			.HasColumnOrder(4)
			.IsRequired(false);
	}
}

/// <inheritdoc cref="AuditedConfiguration{TEntity, TKey, TCreator, TEdited}"/>
[SuppressMessage("Style", "IDE0058", Justification = "Not relevant here, entity type configuration.")]
public abstract class AuditedConfiguration<TEntity, TKey> : AuditedConfiguration<TEntity, TKey, string, string?>, IEntityTypeConfiguration<TEntity>
	where TEntity : class, IAuditedEntity<TKey>
	where TKey : IEquatable<TKey>
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

/// <inheritdoc cref="AuditedConfiguration{TEntity, TKey, TCreator, TEdited}"/>
[SuppressMessage("Style", "IDE0058", Justification = "Not relevant here, entity type configuration.")]
public abstract class AuditedConfiguration<TEntity, TCreator, TEdited> : AuditedConfiguration<TEntity, Guid, TCreator, TEdited>, IEntityTypeConfiguration<TEntity>
	where TEntity : class, IAuditedEntity<TCreator, TEdited>
	where TCreator : notnull
{
	/// <inheritdoc/>
	public override void Configure(EntityTypeBuilder<TEntity> builder)
	{
		base.Configure(builder);

		builder.Property(p => p.Id)
			.HasDefaultValueSql("NEWID()");
	}
}

/// <inheritdoc cref="AuditedConfiguration{TEntity, TKey, TCreator, TEdited}"/>
[SuppressMessage("Style", "IDE0058", Justification = "Not relevant here, entity type configuration.")]
public abstract class AuditedConfiguration<TEntity> : AuditedConfiguration<TEntity, Guid, string, string?>,
	IEntityTypeConfiguration<TEntity> where TEntity : class, IAuditedEntity
{
	/// <inheritdoc/>
	public override void Configure(EntityTypeBuilder<TEntity> builder)
	{
		base.Configure(builder);

		builder.Property(p => p.Id)
			.HasDefaultValueSql("NEWID()");

		builder.Property(e => e.CreatedBy)
			.IsSysNameColumn();

		builder.Property(e => e.EditedBy)
			.IsSysNameColumn();
	}
}
