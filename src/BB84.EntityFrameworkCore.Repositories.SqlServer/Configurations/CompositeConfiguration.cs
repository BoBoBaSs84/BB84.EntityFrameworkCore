// Copyright: 2024 Robert Peter Meyer
// License: MIT
//
// This source code is licensed under the MIT license found in the
// LICENSE file in the root directory of this source tree.
using System.Diagnostics.CodeAnalysis;

using BB84.EntityFrameworkCore.Entities.Abstractions;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BB84.EntityFrameworkCore.Repositories.SqlServer.Configurations;

/// <summary>
/// Represents an abstract base class for configuring entity types that implement the
/// <see cref="ICompositeEntity"/> interface.
/// </summary>
/// <remarks>
/// This class is intended to be used as a base for defining entity type configurations in
/// Entity Framework Core. It provides a default implementation for configuring common properties,
/// such as the <c>Timestamp</c> property.
/// </remarks>
/// <typeparam name="TEntity">The type of the entity being configured.</typeparam>
[SuppressMessage("Style", "IDE0058", Justification = "Not relevant here, entity type configuration.")]
public abstract class CompositeConfiguration<TEntity> : IEntityTypeConfiguration<TEntity>
	where TEntity : class, ICompositeEntity
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
