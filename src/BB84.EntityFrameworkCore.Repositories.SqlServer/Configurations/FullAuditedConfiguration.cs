using System.Diagnostics.CodeAnalysis;

using BB84.EntityFrameworkCore.Entities.Abstractions;
using BB84.EntityFrameworkCore.Repositories.SqlServer.Extensions;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BB84.EntityFrameworkCore.Repositories.SqlServer.Configurations;

/// <summary>
/// The entity configuration for full audited based entities.
/// </summary>
/// <inheritdoc cref="IEntityTypeConfiguration{TEntity}"/>
[SuppressMessage("Style", "IDE0058", Justification = "Not relevant here, entity type configuration.")]
public abstract class FullAuditedConfiguration<TEntity, TKey, TCreator, TEdited> : IEntityTypeConfiguration<TEntity>
	where TEntity : class, IFullAuditedEntity<TKey, TCreator, TEdited>
	where TKey : IEquatable<TKey>
{
	/// <inheritdoc/>
	public virtual void Configure(EntityTypeBuilder<TEntity> builder)
	{
		builder.HasKey(k => k.Id)
			.IsClustered(false);

		builder.Property(p => p.Id)
			.HasColumnOrder(1)
			.ValueGeneratedOnAdd();

		builder.Property(p => p.Timestamp)
			.HasColumnOrder(2)
			.IsConcurrencyToken()
			.ValueGeneratedOnAddOrUpdate();

		builder.Property(p => p.Creator)
			.HasColumnOrder(3)
			.IsRequired();

		builder.Property(p => p.Created)
			.HasColumnOrder(4)
			.IsRequired();

		builder.Property(p => p.Editor)
			.HasColumnOrder(5)
			.IsRequired(false);

		builder.Property(p => p.Edited)
			.HasColumnOrder(6)
			.IsRequired(false);
	}
}

/// <inheritdoc cref="FullAuditedConfiguration{TEntity, TKey, TCreator, TEdited}"/>
[SuppressMessage("Style", "IDE0058", Justification = "Not relevant here, entity type configuration.")]
public abstract class FullAuditeConfiguration<TEntity, TKey> : FullAuditedConfiguration<TEntity, TKey, string, string?>, IEntityTypeConfiguration<TEntity>
	where TEntity : class, IFullAuditedEntity<TKey>
	where TKey : IEquatable<TKey>
{
	/// <inheritdoc/>
	public override void Configure(EntityTypeBuilder<TEntity> builder)
	{
		base.Configure(builder);

		builder.Property(p => p.Creator)
			.IsSysNameColumn();

		builder.Property(p => p.Editor)
			.IsSysNameColumn();
	}
}

/// <inheritdoc cref="FullAuditedConfiguration{TEntity, TKey, TCreator, TEdited}"/>
[SuppressMessage("Style", "IDE0058", Justification = "Not relevant here, entity type configuration.")]
public abstract class FullAuditeConfiguration<TEntity, TCreator, TEdited> : FullAuditedConfiguration<TEntity, Guid, TCreator, TEdited>, IEntityTypeConfiguration<TEntity>
	where TEntity : class, IFullAuditedEntity<TCreator, TEdited>
{
	/// <inheritdoc/>
	public override void Configure(EntityTypeBuilder<TEntity> builder)
	{
		base.Configure(builder);

		builder.Property(p => p.Id)
			.HasDefaultValueSql("NEWID()");
	}
}

/// <inheritdoc cref="FullAuditedConfiguration{TEntity, TKey, TCreator, TEdited}"/>
[SuppressMessage("Style", "IDE0058", Justification = "Not relevant here, entity type configuration.")]
public abstract class FullAuditeConfiguration<TEntity> : FullAuditedConfiguration<TEntity, Guid, string, string?>, IEntityTypeConfiguration<TEntity>
	where TEntity : class, IFullAuditedEntity
{
	/// <inheritdoc/>
	public override void Configure(EntityTypeBuilder<TEntity> builder)
	{
		base.Configure(builder);

		builder.Property(p => p.Id)
			.HasDefaultValueSql("NEWID()");

		builder.Property(p => p.Creator)
			.IsSysNameColumn();

		builder.Property(p => p.Editor)
			.IsSysNameColumn();
	}
}
