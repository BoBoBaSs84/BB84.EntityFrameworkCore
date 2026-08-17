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

/// <inheritdoc cref="Base.FullAuditedConfiguration{TEntity, TKey, TCreator, TEditor}"/>
/// <remarks>
/// Applies the provider-agnostic configuration of
/// <see cref="Base.FullAuditedConfiguration{TEntity, TKey, TCreator, TEditor}"/> and tunes it for
/// SQL Server by declaring the primary key as non clustered.
/// </remarks>
public abstract class FullAuditedConfiguration<TEntity, TKey, TCreator, TEditor> : Base.FullAuditedConfiguration<TEntity, TKey, TCreator, TEditor>, IEntityTypeConfiguration<TEntity>
	where TEntity : class, IFullAuditedEntity<TKey, TCreator, TEditor>
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

/// <inheritdoc cref="FullAuditedConfiguration{TEntity, TKey, TCreator, TEditor}"/>
/// <remarks>
/// <typeparamref name="TKey"/> is supplied; <c>TCreator</c> and <c>TEditor</c> default to
/// <see cref="string"/> and their columns are mapped as <b>sysname</b>. For a custom creator
/// or editor type use <see cref="FullAuditedConfiguration{TEntity, TKey, TCreator, TEditor}"/>
/// and name all four.
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

/// <inheritdoc cref="FullAuditedConfiguration{TEntity, TKey, TCreator, TEditor}"/>
/// <remarks>
/// Only <typeparamref name="TEntity"/> is supplied; <c>TKey</c> defaults to <see cref="Guid"/>
/// and the identifier column defaults to <c>NEWID()</c>, while <c>TCreator</c> and
/// <c>TEditor</c> default to <see cref="string"/> and are mapped as <b>sysname</b>.
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
