// Copyright: 2024 Robert Peter Meyer
// License: MIT
//
// This source code is licensed under the MIT license found in the
// LICENSE file in the root directory of this source tree.
using BB84.EntityFrameworkCore.Entities.Abstractions;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Base = BB84.EntityFrameworkCore.Repositories.Configurations;

namespace BB84.EntityFrameworkCore.Repositories.SqlServer.Configurations;

/// <inheritdoc cref="Base.EnumeratorConfiguration{TEntity, TKey}"/>
/// <remarks>
/// Applies the provider-agnostic configuration of
/// <see cref="Base.EnumeratorConfiguration{TEntity, TKey}"/> and tunes it for SQL Server by
/// declaring the primary key as non clustered and by filtering the unique index on
/// <c>Name</c> to rows that are not soft deleted, so that the name of a soft deleted
/// enumerator can be used again.
/// </remarks>
public abstract class EnumeratorConfiguration<TEntity, TKey> : Base.EnumeratorConfiguration<TEntity, TKey>, IEntityTypeConfiguration<TEntity>
	where TEntity : class, IEnumeratorEntity<TKey>
	where TKey : IEquatable<TKey>
{
	/// <inheritdoc/>
	public override void Configure(EntityTypeBuilder<TEntity> builder)
	{
		base.Configure(builder);

		EntityTypeBuilderDefaults.ApplyKeyClustering<TEntity, TKey>(builder, clustered: false);

		string isDeletedColumn = builder.Property(e => e.IsDeleted).Metadata.GetColumnName();

		_ = builder.HasIndex(e => e.Name)
			.HasFilter($"[{isDeletedColumn}] = 0");
	}
}

/// <inheritdoc cref="EnumeratorConfiguration{TEntity, TKey}"/>
/// <remarks>
/// The identity column is of type <see cref="int"/> and the primary key is clustered, which
/// suits the narrow, densely packed lookup tables this rung is meant for.
/// </remarks>
public abstract class EnumeratorConfiguration<TEntity> : EnumeratorConfiguration<TEntity, int>
	where TEntity : class, IEnumeratorEntity
{
	/// <inheritdoc/>
	public override void Configure(EntityTypeBuilder<TEntity> builder)
	{
		base.Configure(builder);

		EntityTypeBuilderDefaults.ApplyKeyClustering<TEntity, int>(builder, clustered: true);
	}
}
