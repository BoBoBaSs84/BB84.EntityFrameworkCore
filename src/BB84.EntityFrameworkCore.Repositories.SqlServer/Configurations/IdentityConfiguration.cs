﻿using System.Diagnostics.CodeAnalysis;

using BB84.EntityFrameworkCore.Models.Abstractions;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BB84.EntityFrameworkCore.Repositories.SqlServer.Configurations;

/// <summary>
/// The entity configuration for identity based entities.
/// </summary>
/// <inheritdoc cref="IEntityTypeConfiguration{TEntity}"/>
[SuppressMessage("Style", "IDE0058", Justification = "Not relevant here, entity type configuration.")]
public abstract class IdentityConfiguration<TEntity, TKey> : IEntityTypeConfiguration<TEntity>
	where TEntity : class, IIdentityModel<TKey>
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
			.IsConcurrencyToken()
			.HasColumnOrder(2)
			.ValueGeneratedOnAddOrUpdate();
	}
}

/// <inheritdoc cref="IdentityConfiguration{TEntity, TKey}"/>
/// <remarks>
/// The identity column is of type <see cref="Guid"/>.
/// </remarks>
[SuppressMessage("Style", "IDE0058", Justification = "Not relevant here, entity type configuration.")]
public abstract class IdentityConfiguration<TEntity> : IdentityConfiguration<TEntity, Guid>, IEntityTypeConfiguration<TEntity>
	where TEntity : class, IIdentityModel
{
	/// <inheritdoc/>
	public override void Configure(EntityTypeBuilder<TEntity> builder)
	{
		base.Configure(builder);

		builder.Property(p => p.Id)
			.HasDefaultValueSql("NEWID()");
	}
}
