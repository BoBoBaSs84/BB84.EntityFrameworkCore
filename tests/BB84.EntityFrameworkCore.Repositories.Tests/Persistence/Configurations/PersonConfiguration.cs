using BB84.EntityFrameworkCore.Repositories.SqlServer.Configurations;
using BB84.EntityFrameworkCore.Repositories.SqlServer.Extensions;
using BB84.EntityFrameworkCore.Repositories.Tests.Persistence.Entities;

using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BB84.EntityFrameworkCore.Repositories.Tests.Persistence.Configurations;

internal sealed class PersonConfiguration : AuditedConfiguration<PersonEntity>
{
	public override void Configure(EntityTypeBuilder<PersonEntity> builder)
	{
		_ = builder.ToHistoryTable("Person");

		_ = builder.Property(x => x.Settings)
			.IsXmlColumn();

		_ = builder.Property(x => x.DateOfBirth)
			.IsDateColumn();

		_ = builder.Property(x => x.Salary)
			.IsMoneyColumn(true);

		base.Configure(builder);
	}
}
