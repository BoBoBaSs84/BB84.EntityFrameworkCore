using BB84.EntityFrameworkCore.Repositories.SqlServer.Configurations;
using BB84.EntityFrameworkCore.Repositories.SqlServer.Extensions;
using BB84.EntityFrameworkCore.RepositoriesTests.Persistence.Entities;

using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BB84.EntityFrameworkCore.RepositoriesTests.Persistence.Configurations;

internal sealed class PersonConfiguration : AuditedConfiguration<Person>
{
	public override void Configure(EntityTypeBuilder<Person> builder)
	{
		builder.ToHistoryTable();

		builder.Property(x => x.Settings)
			.IsXmlColumn();

		builder.Property(x=>x.DateOfBirth)
			.IsDateColumn();

		builder.Property(x => x.Salary)
			.IsMoneyColumn(true);

		base.Configure(builder);
	}
}
