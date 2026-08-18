// Copyright: 2024 Robert Peter Meyer
// License: MIT
//
// This source code is licensed under the MIT license found in the
// LICENSE file in the root directory of this source tree.
using System.Diagnostics.CodeAnalysis;

using BB84.EntityFrameworkCore.Entities.Abstractions.Components;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BB84.EntityFrameworkCore.Repositories.Configurations;

/// <summary>
/// Provides the shared property configuration used by the entity type configuration base classes.
/// </summary>
/// <remarks>
/// The configuration base classes form ladders of progressively narrower generic aliases. The rungs
/// share no common base that pins the creator and key types to their defaults, so the defaults that
/// depend on those types live here instead of being repeated on every rung.
/// </remarks>
[SuppressMessage("Style", "IDE0058", Justification = "Not relevant here, entity type configuration.")]
internal static class EntityTypeBuilderDefaults
{
	/// <summary>
	/// Configures the primary key and the identifier column of the entity.
	/// </summary>
	/// <typeparam name="TEntity">The type of the entity being configured.</typeparam>
	/// <typeparam name="TKey">The type of the primary key for the entity.</typeparam>
	/// <param name="builder">The builder for the entity type being configured.</param>
	internal static void ApplyIdentityKey<TEntity, TKey>(EntityTypeBuilder<TEntity> builder)
		where TEntity : class, IIdentity<TKey>
		where TKey : IEquatable<TKey>
	{
		builder.HasKey(e => e.Id);

		builder.Property(e => e.Id)
			.HasColumnOrder(1)
			.ValueGeneratedOnAdd();
	}

	/// <summary>
	/// Configures the concurrency token of the entity.
	/// </summary>
	/// <remarks>
	/// The token type has to be named explicitly — it appears nowhere in the parameter list, so
	/// there is nothing for the compiler to infer it from.
	/// </remarks>
	/// <typeparam name="TEntity">The type of the entity being configured.</typeparam>
	/// <typeparam name="TToken">The type of the concurrency token.</typeparam>
	/// <param name="builder">The builder for the entity type being configured.</param>
	/// <param name="columnOrder">The zero based ordering of the column within the table.</param>
	internal static void ApplyConcurrencyToken<TEntity, TToken>(EntityTypeBuilder<TEntity> builder, int columnOrder)
		where TEntity : class, IConcurrency<TToken>
	{
		builder.Property(e => e.Timestamp)
			.HasColumnOrder(columnOrder)
			.IsConcurrencyToken()
			.ValueGeneratedOnAddOrUpdate();
	}

	/// <summary>
	/// Configures the creator and editor columns of the entity.
	/// </summary>
	/// <typeparam name="TEntity">The type of the entity being configured.</typeparam>
	/// <typeparam name="TCreator">The type representing the creator of the entity.</typeparam>
	/// <typeparam name="TEditor">The type representing the editor of the entity.</typeparam>
	/// <param name="builder">The builder for the entity type being configured.</param>
	/// <param name="createdByOrder">The ordering of the creator column within the table.</param>
	/// <param name="editedByOrder">The ordering of the editor column within the table.</param>
	internal static void ApplyUserAuditColumns<TEntity, TCreator, TEditor>(EntityTypeBuilder<TEntity> builder, int createdByOrder, int editedByOrder)
		where TEntity : class, IUserAudited<TCreator, TEditor>
		where TCreator : notnull
	{
		builder.Property(e => e.CreatedBy)
			.HasColumnOrder(createdByOrder)
			.IsRequired();

		builder.Property(e => e.EditedBy)
			.HasColumnOrder(editedByOrder)
			.IsRequired(false);
	}
}
