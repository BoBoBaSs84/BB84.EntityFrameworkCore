using BB84.EntityFrameworkCore.Repositories.SqlServer.Configurations;
using BB84.EntityFrameworkCore.RepositoriesTests.Persistence.Models;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BB84.EntityFrameworkCore.RepositoriesTests.Persistence.Configurations;

internal sealed class SkillConfiguration : FullAuditeConfiguration<Skill>
{
	public override void Configure(EntityTypeBuilder<Skill> builder)
	{
		builder.Property(p => p.Name)
			.HasMaxLength(128)
			.IsRequired()
			.IsUnicode(false);

		builder.HasIndex(p => p.Name)
			.IsUnique();

		builder.Property(p => p.Description)
			.HasMaxLength(512)
			.IsRequired()
			.IsUnicode();

		builder.Property(p => p.IsCritical)
			.HasDefaultValue(false);

		base.Configure(builder);
	}
}
