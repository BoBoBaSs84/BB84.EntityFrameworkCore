﻿using System.Diagnostics.CodeAnalysis;

using BB84.EntityFrameworkCore.Models.Abstractions;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BB84.EntityFrameworkCore.Repository.Configurations;

/// <summary>
/// The composite base configuration class.
/// </summary>
/// <inheritdoc cref="IEntityTypeConfiguration{TEntity}"/>
/// <inheritdoc cref="ICompositeModel{TCreated, TModified}"/>
[SuppressMessage("Style", "IDE0058", Justification = "Not relevant here, entity type configuration.")]
public abstract class CompositeBaseConfiguration<TEntity, TCreated, TModified> : IEntityTypeConfiguration<TEntity>
	where TEntity : class, ICompositeModel<TCreated, TModified>
{
	/// <inheritdoc/>
	public virtual void Configure(EntityTypeBuilder<TEntity> builder)
	{
		builder.Property(e => e.Timestamp)
			.IsRowVersion()
			.HasColumnOrder(1);

		builder.Property(e => e.CreatedBy)
			.IsRequired()
			.HasColumnOrder(2);

		builder.Property(e => e.ModifiedBy)
			.IsRequired(false)
			.HasColumnOrder(3);
	}
}

/// <inheritdoc/>
public abstract class CompositeBaseConfiguration<TEntity> : CompositeBaseConfiguration<TEntity, string, string?>,
	IEntityTypeConfiguration<TEntity> where TEntity : class, ICompositeModel
{ }