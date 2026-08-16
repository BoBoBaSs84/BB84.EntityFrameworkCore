// Copyright: 2024 Robert Peter Meyer
// License: MIT
//
// This source code is licensed under the MIT license found in the
// LICENSE file in the root directory of this source tree.
using System.Diagnostics.CodeAnalysis;

using BB84.EntityFrameworkCore.Entities.Abstractions;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BB84.EntityFrameworkCore.Repositories.Configurations;

/// <summary>
/// Represents an abstract base class for configuring entity types that implement the
/// <see cref="IEnumeratorEntity{Tkey}"/> interface.
/// </summary>
/// <remarks>
/// <para>
/// This class defines a standard configuration for entities, including primary key setup,
/// concurrency tokens, property constraints and indexing.
/// </para>
/// <para>
/// A global query filter excluding soft deleted rows is applied, so that entities marked as
/// deleted by the soft deletable interceptor are no longer returned by queries. Pass
/// <see langword="true"/> for the <c>ignoreQueryFilters</c> parameter of the repository read
/// methods to include them again.
/// </para>
/// <para>
/// Be aware that a <b>required</b> navigation pointing at a soft deletable entity type
/// interacts badly with this filter: filtering out the principal row also removes the
/// dependents that require it. Model such navigations as optional, or apply a matching filter
/// to both ends of the relationship.
/// </para>
/// </remarks>
/// <typeparam name="TEntity">The type of the entity being configured.</typeparam>
/// <typeparam name="TKey">The type of the key for the entity.</typeparam>
[SuppressMessage("Style", "IDE0058", Justification = "Not relevant here, entity type configuration.")]
public abstract class EnumeratorConfiguration<TEntity, TKey> : IEntityTypeConfiguration<TEntity>
	where TEntity : class, IEnumeratorEntity<TKey>
	where TKey : IEquatable<TKey>
{
	/// <inheritdoc/>
	public virtual void Configure(EntityTypeBuilder<TEntity> builder)
	{
		builder.HasKey(x => x.Id);

		builder.Property(e => e.Id)
			.HasColumnOrder(1)
			.IsRequired();

		EntityTypeBuilderDefaults.ApplyConcurrencyToken(builder, columnOrder: 2);

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

		builder.HasQueryFilter(e => !e.IsDeleted);

		builder.HasIndex(e => e.Name)
			.IsUnique();
	}
}

/// <inheritdoc cref="EnumeratorConfiguration{TEntity, TKey}"/>
/// <remarks>
/// The identity column is of type <see cref="int"/>.
/// </remarks>
public abstract class EnumeratorConfiguration<TEntity> : EnumeratorConfiguration<TEntity, int>
	where TEntity : class, IEnumeratorEntity
{ }
