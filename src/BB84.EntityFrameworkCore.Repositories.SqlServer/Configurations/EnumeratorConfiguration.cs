﻿using System.Diagnostics.CodeAnalysis;

using BB84.EntityFrameworkCore.Models.Abstractions;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BB84.EntityFrameworkCore.Repositories.SqlServer.Configurations;

/// <summary>
/// The entity configuration for enumerator based entities.
/// </summary>
/// <inheritdoc cref="IEntityTypeConfiguration{TEntity}"/>
[SuppressMessage("Style", "IDE0058", Justification = "Not relevant here, entity type configuration.")]
public abstract class EnumeratorConfiguration<TEntity, TKey> : IEntityTypeConfiguration<TEntity>
	where TEntity : class, IEnumeratorModel<TKey>
	where TKey : IEquatable<TKey>
{
	/// <inheritdoc/>
	public virtual void Configure(EntityTypeBuilder<TEntity> builder)
	{
		builder.HasKey(x => x.Id)
			.IsClustered(false);

		builder.Property(e => e.Id)
			.HasColumnOrder(1)
			.IsRequired();

		builder.Property(e => e.Timestamp)
			.HasColumnOrder(2)
			.IsConcurrencyToken()
			.ValueGeneratedOnAddOrUpdate();

		builder.Property(e => e.Name)
			.HasColumnOrder(3)
			.HasMaxLength(64)
			.IsRequired()
			.IsUnicode(false);

		builder.Property(e => e.Description)
			.HasColumnOrder(4)
			.HasMaxLength(256)
			.IsRequired(false)
			.IsUnicode();

		builder.Property(e => e.IsDeleted)
			.HasColumnOrder(5)
			.HasDefaultValue(false);

		builder.HasIndex(e => e.Name)
			.IsUnique();
	}
}

/// <inheritdoc cref="EnumeratorConfiguration{TEntity, TKey}"/>
/// <remarks>
/// The identity column is of type <see cref="int"/>.
/// </remarks>
[SuppressMessage("Style", "IDE0058", Justification = "Not relevant here, entity type configuration.")]
public abstract class EnumeratorConfiguration<TEntity> : EnumeratorConfiguration<TEntity, int>
	where TEntity : class, IEnumeratorModel
{
	/// <inheritdoc/>
	public override void Configure(EntityTypeBuilder<TEntity> builder)
	{
		base.Configure(builder);

		builder.HasKey(x => x.Id)
			.IsClustered();
	}
}
