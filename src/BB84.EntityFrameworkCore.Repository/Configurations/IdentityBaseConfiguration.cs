using System.Diagnostics.CodeAnalysis;

using BB84.EntityFrameworkCore.Models.Abstractions;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BB84.EntityFrameworkCore.Repository.Configurations;

/// <summary>
/// The identity base configuration class.
/// </summary>
/// <inheritdoc cref="IEntityTypeConfiguration{TEntity}"/>
/// <inheritdoc cref="IIdentityModel{TKey}"/>
[SuppressMessage("Style", "IDE0058", Justification = "Not relevant here, entity type configuration.")]
public abstract class IdentityBaseConfiguration<TEntity, TKey> : IEntityTypeConfiguration<TEntity>
	where TEntity : class, IIdentityModel<TKey>
	where TKey : IEquatable<TKey>
{
	/// <inheritdoc/>
	public virtual void Configure(EntityTypeBuilder<TEntity> builder)
	{
		builder.HasKey(e => e.Id)
			.IsClustered(false);

		builder.Property(e => e.Id)
			.HasDefaultValueSql("NEWID()")
			.ValueGeneratedOnAdd()
			.HasColumnOrder(1);

		builder.Property(e => e.Timestamp)
			.IsRowVersion()
			.HasColumnOrder(2);
	}
}

/// <inheritdoc/>
public abstract class IdentityBaseConfiguration<TEntity> : IdentityBaseConfiguration<TEntity, Guid>,
	IEntityTypeConfiguration<TEntity> where TEntity : class, IIdentityModel
{ }