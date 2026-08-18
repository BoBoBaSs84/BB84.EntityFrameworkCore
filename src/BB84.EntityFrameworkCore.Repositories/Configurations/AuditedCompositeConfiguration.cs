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
/// Provides a base configuration for entities that implement the
/// <see cref="IAuditedCompositeEntity{TCreator, TEditor, TToken}"/> interface.
/// </summary>
/// <remarks>
/// This abstract class provides a reusable configuration for audited composite entities,
/// ensuring that the <c>Timestamp</c>, <c>Creator</c>, and <c>Editor</c> properties are consistently
/// configured across all derived entity types. The <c>Timestamp</c> property is marked as a concurrency
/// token and is automatically updated on add or update operations. The <c>Creator</c> property is required,
/// while the <c>Editor</c> property is optional.
/// </remarks>
/// <typeparam name="TEntity">The type of the entity being configured.</typeparam>
/// <typeparam name="TCreator">The type representing the creator of the entity.</typeparam>
/// <typeparam name="TEditor">The type representing the editor of the entity.</typeparam>
/// <typeparam name="TToken">The type of the concurrency token.</typeparam>
public abstract class AuditedCompositeConfiguration<TEntity, TCreator, TEditor, TToken> : IEntityTypeConfiguration<TEntity>
	where TEntity : class, IAuditedCompositeEntity<TCreator, TEditor, TToken>
	where TCreator : notnull
{
	/// <inheritdoc/>
	public virtual void Configure(EntityTypeBuilder<TEntity> builder)
	{
		EntityTypeBuilderDefaults.ApplyConcurrencyToken<TEntity, TToken>(builder, columnOrder: 3);
		EntityTypeBuilderDefaults.ApplyUserAuditColumns<TEntity, TCreator, TEditor>(builder, createdByOrder: 4, editedByOrder: 5);
	}
}

/// <inheritdoc cref="AuditedCompositeConfiguration{TEntity, TCreator, TEditor, TToken}"/>
/// <remarks>
/// The creator and editor types are supplied; <c>TToken</c> defaults to a <see cref="byte"/> array.
/// For a custom token type use
/// <see cref="AuditedCompositeConfiguration{TEntity, TCreator, TEditor, TToken}"/> and name all four.
/// </remarks>
public abstract class AuditedCompositeConfiguration<TEntity, TCreator, TEditor> : AuditedCompositeConfiguration<TEntity, TCreator, TEditor, byte[]>, IEntityTypeConfiguration<TEntity>
	where TEntity : class, IAuditedCompositeEntity<TCreator, TEditor>
	where TCreator : notnull;

/// <inheritdoc cref="AuditedCompositeConfiguration{TEntity, TCreator, TEditor, TToken}"/>
/// <remarks>
/// The creator and editor types default to <see cref="string"/> and the token to a
/// <see cref="byte"/> array.
/// </remarks>
public abstract class AuditedCompositeConfiguration<TEntity> : AuditedCompositeConfiguration<TEntity, string, string?>, IEntityTypeConfiguration<TEntity>
	where TEntity : class, IAuditedCompositeEntity;
