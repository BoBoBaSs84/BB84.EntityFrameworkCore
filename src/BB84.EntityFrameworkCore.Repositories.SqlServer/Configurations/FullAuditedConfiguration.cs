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

/// <inheritdoc cref="Base.FullAuditedConfiguration{TEntity, TKey, TCreator, TEdited}"/>
/// <remarks>
/// Applies the provider-agnostic configuration of
/// <see cref="Base.FullAuditedConfiguration{TEntity, TKey, TCreator, TEdited}"/> and tunes it for
/// SQL Server by declaring the primary key as non clustered.
/// </remarks>
public abstract class FullAuditedConfiguration<TEntity, TKey, TCreator, TEdited> : Base.FullAuditedConfiguration<TEntity, TKey, TCreator, TEdited>, IEntityTypeConfiguration<TEntity>
	where TEntity : class, IFullAuditedEntity<TKey, TCreator, TEdited>
	where TKey : IEquatable<TKey>
	where TCreator : notnull
{
	/// <inheritdoc/>
	public override void Configure(EntityTypeBuilder<TEntity> builder)
	{
		base.Configure(builder);

		EntityTypeBuilderDefaults.ApplyKeyClustering<TEntity, TKey>(builder, clustered: false);
	}
}

/// <inheritdoc cref="FullAuditedConfiguration{TEntity, TKey, TCreator, TEdited}"/>
/// <remarks>
/// The creator and editor columns are mapped as <b>sysname</b>.
/// </remarks>
public abstract class FullAuditedConfiguration<TEntity, TKey> : FullAuditedConfiguration<TEntity, TKey, string, string?>, IEntityTypeConfiguration<TEntity>
	where TEntity : class, IFullAuditedEntity<TKey>
	where TKey : IEquatable<TKey>
{
	/// <inheritdoc/>
	public override void Configure(EntityTypeBuilder<TEntity> builder)
	{
		base.Configure(builder);

		EntityTypeBuilderDefaults.ApplySysNameAuditColumns(builder);
	}
}

/// <inheritdoc cref="FullAuditedConfiguration{TEntity, TKey, TCreator, TEdited}"/>
/// <remarks>
/// The identifier column defaults to <c>NEWID()</c>.
/// </remarks>
public abstract class FullAuditedConfiguration<TEntity, TCreator, TEdited> : FullAuditedConfiguration<TEntity, Guid, TCreator, TEdited>, IEntityTypeConfiguration<TEntity>
	where TEntity : class, IFullAuditedEntity<TCreator, TEdited>
	where TCreator : notnull
{
	/// <inheritdoc/>
	public override void Configure(EntityTypeBuilder<TEntity> builder)
	{
		base.Configure(builder);

		EntityTypeBuilderDefaults.ApplyGuidIdDefault(builder);
	}
}

/// <inheritdoc cref="FullAuditedConfiguration{TEntity, TKey, TCreator, TEdited}"/>
/// <remarks>
/// The identifier column defaults to <c>NEWID()</c> and the creator and editor columns are
/// mapped as <b>sysname</b>.
/// </remarks>
public abstract class FullAuditedConfiguration<TEntity> : FullAuditedConfiguration<TEntity, Guid, string, string?>, IEntityTypeConfiguration<TEntity>
	where TEntity : class, IFullAuditedEntity
{
	/// <inheritdoc/>
	public override void Configure(EntityTypeBuilder<TEntity> builder)
	{
		base.Configure(builder);

		EntityTypeBuilderDefaults.ApplyGuidIdDefault(builder);
		EntityTypeBuilderDefaults.ApplySysNameAuditColumns(builder);
	}
}
