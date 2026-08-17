// Copyright: 2024 Robert Peter Meyer
// License: MIT
//
// This source code is licensed under the MIT license found in the
// LICENSE file in the root directory of this source tree.
using BB84.EntityFrameworkCore.Repositories.Configurations;
using BB84.EntityFrameworkCore.TestSupport.Entities;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BB84.EntityFrameworkCore.Repositories.Agnostic.Tests.Persistence.Configurations;

/// <remarks>
/// <para>
/// Only the <c>uniqueidentifier</c> column type is dropped relative to the SQL Server counterpart.
/// </para>
/// <para>
/// The lengths and the unicode flags are kept so the two files stay easy to compare, but SQLite
/// ignores both — do not assert that a length is enforced here. The unique index on <c>Name</c>
/// <b>is</b> enforced.
/// </para>
/// </remarks>
internal sealed class SkillConfiguration : FullAuditedConfiguration<SkillEntity>
{
	public override void Configure(EntityTypeBuilder<SkillEntity> builder)
	{
		_ = builder.ToTable("Skills");

		_ = builder.Property(p => p.Name)
			.HasMaxLength(128)
			.IsRequired()
			.IsUnicode(false);

		_ = builder.HasIndex(p => p.Name)
			.IsUnique();

		_ = builder.Property(p => p.Description)
			.HasMaxLength(512)
			.IsRequired()
			.IsUnicode();

		_ = builder.Property(p => p.IsCritical)
			.HasDefaultValue(false);

		base.Configure(builder);
	}
}
