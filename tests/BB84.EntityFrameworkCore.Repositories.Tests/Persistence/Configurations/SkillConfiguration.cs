﻿using BB84.EntityFrameworkCore.Repositories.SqlServer.Configurations;
using BB84.EntityFrameworkCore.Repositories.Tests.Persistence.Entities;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BB84.EntityFrameworkCore.Repositories.Tests.Persistence.Configurations;

internal sealed class SkillConfiguration : FullAuditeConfiguration<Skill>
{
	public override void Configure(EntityTypeBuilder<Skill> builder)
	{
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
