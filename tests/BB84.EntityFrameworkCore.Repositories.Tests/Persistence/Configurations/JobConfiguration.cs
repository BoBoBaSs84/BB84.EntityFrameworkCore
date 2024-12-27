using BB84.EntityFrameworkCore.Repositories.SqlServer.Configurations;
using BB84.EntityFrameworkCore.Repositories.Tests.Persistence.Entities;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BB84.EntityFrameworkCore.Repositories.Tests.Persistence.Configurations;

internal sealed class JobConfiguration : IdentityConfiguration<JobEntity>
{
	public override void Configure(EntityTypeBuilder<JobEntity> builder)
	{
		_ = builder.ToTable("Job");

		base.Configure(builder);
	}
}
