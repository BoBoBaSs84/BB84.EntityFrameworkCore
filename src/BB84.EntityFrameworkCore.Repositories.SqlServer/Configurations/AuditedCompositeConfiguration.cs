// Copyright: 2024 Robert Peter Meyer
// License: MIT
//
// This source code is licensed under the MIT license found in the
// LICENSE file in the root directory of this source tree.
using System.Diagnostics.CodeAnalysis;

using BB84.EntityFrameworkCore.Entities.Abstractions;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Base = BB84.EntityFrameworkCore.Repositories.Configurations;

namespace BB84.EntityFrameworkCore.Repositories.SqlServer.Configurations;

/// <inheritdoc cref="Base.AuditedCompositeConfiguration{TEntity, TCreator, TEdited}"/>
/// <remarks>
/// Nothing in this rung is SQL Server specific. The type exists so that the ladder is complete
/// in both namespaces and a consumer can pick the namespace rather than the type.
/// </remarks>
[SuppressMessage("Style", "IDE0058", Justification = "Not relevant here, entity type configuration.")]
public abstract class AuditedCompositeConfiguration<TEntity, TCreator, TEdited> : Base.AuditedCompositeConfiguration<TEntity, TCreator, TEdited>, IEntityTypeConfiguration<TEntity>
	where TEntity : class, IAuditedCompositeEntity<TCreator, TEdited>
	where TCreator : notnull
{ }

/// <inheritdoc cref="AuditedCompositeConfiguration{TEntity, TCreator, TEdited}"/>
/// <remarks>
/// The creator and editor columns are mapped as <b>sysname</b>.
/// </remarks>
[SuppressMessage("Style", "IDE0058", Justification = "Not relevant here, entity type configuration.")]
public abstract class AuditedCompositeConfiguration<TEntity> : AuditedCompositeConfiguration<TEntity, string, string?>, IEntityTypeConfiguration<TEntity>
	where TEntity : class, IAuditedCompositeEntity
{
	/// <inheritdoc/>
	public override void Configure(EntityTypeBuilder<TEntity> builder)
	{
		base.Configure(builder);

		EntityTypeBuilderDefaults.ApplySysNameAuditColumns(builder);
	}
}
