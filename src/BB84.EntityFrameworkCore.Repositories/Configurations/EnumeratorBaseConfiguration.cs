using System.Diagnostics.CodeAnalysis;

using BB84.EntityFrameworkCore.Models.Abstractions;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BB84.EntityFrameworkCore.Repositories.Configurations;

/// <summary>
/// The enumerator base configuration class.
/// </summary>
/// <inheritdoc cref="IEntityTypeConfiguration{TEntity}"/>
[SuppressMessage("Style", "IDE0058", Justification = "Not relevant here, entity type configuration.")]
public abstract class EnumeratorBaseConfiguration<TEntity> : IEntityTypeConfiguration<TEntity> where TEntity : class, IEnumeratorModel
{
	/// <inheritdoc/>
	public virtual void Configure(EntityTypeBuilder<TEntity> builder)
	{
		builder.HasKey(x => x.Id)
			.IsClustered();

		builder.Property(e => e.Id)
			.IsRequired()
			.HasColumnOrder(1);

		builder.Property(e => e.Timestamp)
			.IsRowVersion()
			.HasColumnOrder(2);

		builder.Property(e => e.Name)
			.HasMaxLength(50)
			.IsRequired()
			.HasColumnOrder(3);

		builder.Property(e => e.Description)
			.HasMaxLength(250)
			.IsRequired(false)
			.HasColumnOrder(4);

		builder.Property(e => e.IsDeleted)
			.HasDefaultValue(false)
			.HasColumnOrder(5);

		builder.HasIndex(e => e.Name)
			.IsClustered()
			.IsUnique();
	}
}
