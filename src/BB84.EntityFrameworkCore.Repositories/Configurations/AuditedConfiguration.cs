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
/// Represents an abstract base class for configuring the entities of type
/// <see cref="IAuditedEntity{TKey, TCreator, TEditor}"/> for identity-related and time audited
/// entities in the Entity Framework Core model.
/// </summary>
/// <remarks>
/// This class provides a default configuration for audited entities, including primary key setup,
/// concurrency token configuration, and required properties for creator and editor fields.
/// Derived classes can override the <see cref="Configure"/> method to customize the configuration.
/// </remarks>
/// <typeparam name="TEntity">The type of the entity being configured.</typeparam>
/// <typeparam name="TKey">The type of the primary key for the entity.</typeparam>
/// <typeparam name="TCreator">The type representing the creator of the entity.</typeparam>
/// <typeparam name="TEditor">The type representing the editor of the entity.</typeparam>
public abstract class AuditedConfiguration<TEntity, TKey, TCreator, TEditor> : IEntityTypeConfiguration<TEntity>
	where TEntity : class, IAuditedEntity<TKey, TCreator, TEditor>
	where TKey : IEquatable<TKey>
	where TCreator : notnull
{
	/// <inheritdoc/>
	public virtual void Configure(EntityTypeBuilder<TEntity> builder)
	{
		EntityTypeBuilderDefaults.ApplyIdentityKey<TEntity, TKey>(builder);
		EntityTypeBuilderDefaults.ApplyConcurrencyToken(builder, columnOrder: 2);
		EntityTypeBuilderDefaults.ApplyUserAuditColumns<TEntity, TCreator, TEditor>(builder, createdByOrder: 3, editedByOrder: 4);
	}
}

/// <inheritdoc cref="AuditedConfiguration{TEntity, TKey, TCreator, TEditor}"/>
/// <remarks>
/// <typeparamref name="TKey"/> is supplied; <c>TCreator</c> defaults to <see cref="string"/>
/// and <c>TEditor</c> to <see cref="string"/>. For a custom creator or editor type use
/// <see cref="AuditedConfiguration{TEntity, TKey, TCreator, TEditor}"/> and name all four.
/// </remarks>
public abstract class AuditedConfiguration<TEntity, TKey> : AuditedConfiguration<TEntity, TKey, string, string?>, IEntityTypeConfiguration<TEntity>
	where TEntity : class, IAuditedEntity<TKey>
	where TKey : IEquatable<TKey>
{ }

/// <inheritdoc cref="AuditedConfiguration{TEntity, TKey, TCreator, TEditor}"/>
/// <remarks>
/// Only <typeparamref name="TEntity"/> is supplied; <c>TKey</c> defaults to <see cref="Guid"/>,
/// <c>TCreator</c> to <see cref="string"/> and <c>TEditor</c> to <see cref="string"/>.
/// </remarks>
public abstract class AuditedConfiguration<TEntity> : AuditedConfiguration<TEntity, Guid, string, string?>,
	IEntityTypeConfiguration<TEntity> where TEntity : class, IAuditedEntity
{ }
