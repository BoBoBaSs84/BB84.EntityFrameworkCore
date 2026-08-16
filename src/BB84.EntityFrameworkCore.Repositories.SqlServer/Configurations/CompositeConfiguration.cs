// Copyright: 2024 Robert Peter Meyer
// License: MIT
//
// This source code is licensed under the MIT license found in the
// LICENSE file in the root directory of this source tree.
using System.Diagnostics.CodeAnalysis;

using BB84.EntityFrameworkCore.Entities.Abstractions;

using Microsoft.EntityFrameworkCore;

using Base = BB84.EntityFrameworkCore.Repositories.Configurations;

namespace BB84.EntityFrameworkCore.Repositories.SqlServer.Configurations;

/// <inheritdoc cref="Base.CompositeConfiguration{TEntity}"/>
/// <remarks>
/// Nothing in this configuration is SQL Server specific. The type exists so that the ladder is
/// complete in both namespaces and a consumer can pick the namespace rather than the type.
/// </remarks>
[SuppressMessage("Style", "IDE0058", Justification = "Not relevant here, entity type configuration.")]
public abstract class CompositeConfiguration<TEntity> : Base.CompositeConfiguration<TEntity>, IEntityTypeConfiguration<TEntity>
	where TEntity : class, ICompositeEntity
{ }
