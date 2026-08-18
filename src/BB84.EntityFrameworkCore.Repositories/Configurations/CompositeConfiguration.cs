// Copyright: 2024 Robert Peter Meyer
// License: MIT
//
// This source code is licensed under the MIT license found in the
// LICENSE file in the root directory of this source tree.
using BB84.EntityFrameworkCore.Entities.Abstractions;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BB84.EntityFrameworkCore.Repositories.Configurations;

/// <summary>
/// Represents an abstract base class for configuring entity types that implement the
/// <see cref="ICompositeEntity{TToken}"/> interface.
/// </summary>
/// <remarks>
/// This class is intended to be used as a base for defining entity type configurations in
/// Entity Framework Core. It provides a default implementation for configuring common properties,
/// such as the <c>Timestamp</c> property.
/// </remarks>
/// <typeparam name="TEntity">The type of the entity being configured.</typeparam>
/// <typeparam name="TToken">The type of the concurrency token.</typeparam>
public abstract class CompositeConfiguration<TEntity, TToken> : IEntityTypeConfiguration<TEntity>
	where TEntity : class, ICompositeEntity<TToken>
{
	/// <inheritdoc/>
	public virtual void Configure(EntityTypeBuilder<TEntity> builder)
		=> EntityTypeBuilderDefaults.ApplyConcurrencyToken<TEntity, TToken>(builder, columnOrder: 3);
}

/// <inheritdoc cref="CompositeConfiguration{TEntity, TToken}"/>
/// <remarks>
/// The token is a <see cref="byte"/> array. For a custom token type use
/// <see cref="CompositeConfiguration{TEntity, TToken}"/> and name both.
/// </remarks>
public abstract class CompositeConfiguration<TEntity> : CompositeConfiguration<TEntity, byte[]>, IEntityTypeConfiguration<TEntity>
	where TEntity : class, ICompositeEntity;
