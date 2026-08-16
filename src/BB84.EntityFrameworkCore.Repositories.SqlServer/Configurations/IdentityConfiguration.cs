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

/// <inheritdoc cref="Base.IdentityConfiguration{TEntity, TKey}"/>
/// <remarks>
/// Applies the provider-agnostic configuration of
/// <see cref="Base.IdentityConfiguration{TEntity, TKey}"/> and tunes it for SQL Server by
/// declaring the primary key as non clustered.
/// </remarks>
public abstract class IdentityConfiguration<TEntity, TKey> : Base.IdentityConfiguration<TEntity, TKey>, IEntityTypeConfiguration<TEntity>
	where TEntity : class, IIdentityEntity<TKey>
	where TKey : IEquatable<TKey>
{
	/// <inheritdoc/>
	public override void Configure(EntityTypeBuilder<TEntity> builder)
	{
		base.Configure(builder);

		EntityTypeBuilderDefaults.ApplyKeyClustering<TEntity, TKey>(builder, clustered: false);
	}
}

/// <inheritdoc cref="IdentityConfiguration{TEntity, TKey}"/>
public abstract class IdentityConfiguration<TEntity> : IdentityConfiguration<TEntity, Guid>, IEntityTypeConfiguration<TEntity>
	where TEntity : class, IIdentityEntity
{
	/// <inheritdoc/>
	public override void Configure(EntityTypeBuilder<TEntity> builder)
	{
		base.Configure(builder);

		EntityTypeBuilderDefaults.ApplyGuidIdDefault(builder);
	}
}
