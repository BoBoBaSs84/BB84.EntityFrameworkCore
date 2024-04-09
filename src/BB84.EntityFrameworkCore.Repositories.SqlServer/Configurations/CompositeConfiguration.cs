using System.Diagnostics.CodeAnalysis;

using BB84.EntityFrameworkCore.Models.Abstractions;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BB84.EntityFrameworkCore.Repositories.SqlServer.Configurations;

/// <summary>
/// The composite base configuration class.
/// </summary>
/// <inheritdoc cref="IEntityTypeConfiguration{TEntity}"/>
/// <inheritdoc cref="ICompositeModel"/>
[SuppressMessage("Style", "IDE0058", Justification = "Not relevant here, entity type configuration.")]
public abstract class CompositeConfiguration<TEntity> : IEntityTypeConfiguration<TEntity>
	where TEntity : class, ICompositeModel
{
	/// <inheritdoc/>
	public virtual void Configure(EntityTypeBuilder<TEntity> builder)
	{
		builder.Property(e => e.Timestamp)
			.HasColumnOrder(3)
			.IsConcurrencyToken()
			.ValueGeneratedOnAddOrUpdate();
	}
}
