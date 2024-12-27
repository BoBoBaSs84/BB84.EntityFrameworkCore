using System.Diagnostics.CodeAnalysis;

using BB84.EntityFrameworkCore.Entities.Abstractions;
using BB84.EntityFrameworkCore.Repositories.SqlServer.Extensions;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BB84.EntityFrameworkCore.Repositories.SqlServer.Configurations;

/// <summary>
/// The entity configuration for audited based entities.
/// </summary>
/// <inheritdoc cref="IEntityTypeConfiguration{TEntity}"/>
[SuppressMessage("Style", "IDE0058", Justification = "Not relevant here, entity type configuration.")]
public abstract class AuditedConfiguration<TEntity, TKey, TCreator, TEdited> : IEntityTypeConfiguration<TEntity>
	where TEntity : class, IAuditedEntity<TKey, TCreator, TEdited>
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
			.HasColumnOrder(2)
			.IsConcurrencyToken()
			.ValueGeneratedOnAddOrUpdate();

		builder.Property(e => e.Creator)
			.HasColumnOrder(3)
			.IsRequired();

		builder.Property(e => e.Editor)
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

		builder.Property(e => e.Creator)
			.IsSysNameColumn();

		builder.Property(e => e.Editor)
			.IsSysNameColumn();
	}
}

/// <inheritdoc cref="AuditedConfiguration{TEntity, TKey, TCreator, TEdited}"/>
[SuppressMessage("Style", "IDE0058", Justification = "Not relevant here, entity type configuration.")]
public abstract class AuditedConfiguration<TEntity, TCreator, TEdited> : AuditedConfiguration<TEntity, Guid, TCreator, TEdited>, IEntityTypeConfiguration<TEntity>
	where TEntity : class, IAuditedEntity<TCreator, TEdited>
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

		builder.Property(e => e.Creator)
			.IsSysNameColumn();

		builder.Property(e => e.Editor)
			.IsSysNameColumn();
	}
}
