using System.Diagnostics.CodeAnalysis;

using BB84.EntityFrameworkCore.Entities.Abstractions;
using BB84.EntityFrameworkCore.Repositories.SqlServer.Extensions;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BB84.EntityFrameworkCore.Repositories.SqlServer.Configurations;

/// <summary>
/// The entity configuration for audited composite based entities.
/// </summary>
/// <inheritdoc cref="IEntityTypeConfiguration{TEntity}"/>
[SuppressMessage("Style", "IDE0058", Justification = "Not relevant here, entity type configuration.")]
public abstract class AuditedCompositeConfiguration<TEntity, TCreator, TEdited> : IEntityTypeConfiguration<TEntity>
	where TEntity : class, IAuditedCompositeEntity<TCreator, TEdited>
{
	/// <inheritdoc/>
	public virtual void Configure(EntityTypeBuilder<TEntity> builder)
	{
		builder.Property(e => e.Timestamp)
			.HasColumnOrder(3)
			.IsConcurrencyToken()
			.ValueGeneratedOnAddOrUpdate();

		builder.Property(e => e.Creator)
			.HasColumnOrder(4)
			.IsRequired();

		builder.Property(e => e.Editor)
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

		builder.Property(e => e.Creator)
			.IsSysNameColumn();

		builder.Property(e => e.Editor)
			.IsSysNameColumn();
	}
}
