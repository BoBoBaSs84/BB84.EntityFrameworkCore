using BB84.EntityFrameworkCore.Repositories.SqlServer.Configurations;
using BB84.EntityFrameworkCore.Repositories.SqlServer.Extensions;
using BB84.EntityFrameworkCore.Repositories.Tests.Persistence.Entities;

using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BB84.EntityFrameworkCore.Repositories.Tests.Persistence.Configurations;

internal sealed class JobTypeConfiguration : CompositeConfiguration<JobType>
{
	public override void Configure(EntityTypeBuilder<JobType> builder)
	{
		_ = builder.HasNoKey();

		_ = builder.Property(x => x.Color)
			.IsVarbinaryColumn(3);

		base.Configure(builder);
	}
}
