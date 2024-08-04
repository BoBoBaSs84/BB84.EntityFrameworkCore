using System.Diagnostics.CodeAnalysis;

using BB84.EntityFrameworkCore.Models.Abstractions;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BB84.EntityFrameworkCore.Repositories.SqlServer.Configurations;

/// <summary>
/// The audited composite configuration class.
/// </summary>
/// <inheritdoc cref="IEntityTypeConfiguration{TEntity}"/>
/// <inheritdoc cref="IAuditedCompositeModel{TCreated, TModified}"/>
[SuppressMessage("Style", "IDE0058", Justification = "Not relevant here, entity type configuration.")]
public abstract class AuditedCompositeConfiguration<TEntity, TCreated, TModified> : IEntityTypeConfiguration<TEntity>
	where TEntity : class, IAuditedCompositeModel<TCreated, TModified>
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

		builder.Property(e => e.ModifiedBy)
			.HasColumnOrder(5)
			.IsRequired(false);
	}
}

/// <inheritdoc/>
[SuppressMessage("Style", "IDE0058", Justification = "Not relevant here, entity type configuration.")]
public abstract class AuditedCompositeConfiguration<TEntity> : AuditedCompositeConfiguration<TEntity, string, string?>, IEntityTypeConfiguration<TEntity>
	where TEntity : class, IAuditedCompositeModel
{
	/// <inheritdoc/>
	public override void Configure(EntityTypeBuilder<TEntity> builder)
	{
		base.Configure(builder);

		builder.Property(e => e.CreatedBy)
			.HasColumnType("sysname");

		builder.Property(e => e.ModifiedBy)
			.HasColumnType("sysname");
	}
}
