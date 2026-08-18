// Copyright: 2024 Robert Peter Meyer
// License: MIT
//
// This source code is licensed under the MIT license found in the
// LICENSE file in the root directory of this source tree.
using BB84.EntityFrameworkCore.Repositories.Agnostic.Tests.Persistence.Entities;
using BB84.EntityFrameworkCore.Repositories.Configurations;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BB84.EntityFrameworkCore.Repositories.Agnostic.Tests.Persistence.Configurations;

/// <remarks>
/// Names the token type on the configuration rung as well as on the entity, which is the pairing
/// the whole feature comes down to. The base still marks the property as a concurrency token; only
/// the generation strategy is overridden, because no provider generates a <see cref="Guid"/> row
/// version by itself and this one is rotated by the writer instead.
/// </remarks>
internal sealed class TokenTypeConfiguration : IdentityConfiguration<TokenTypeEntity, Guid, Guid>
{
	public override void Configure(EntityTypeBuilder<TokenTypeEntity> builder)
	{
		base.Configure(builder);

		_ = builder.Property(e => e.Timestamp)
			.ValueGeneratedNever();

		_ = builder.Property(e => e.Name)
			.IsRequired();
	}
}
