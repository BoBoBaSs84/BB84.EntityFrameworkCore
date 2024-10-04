using BB84.EntityFrameworkCore.Repositories.SqlServer.Configurations;
using BB84.EntityFrameworkCore.Repositories.SqlServer.Extensions;
using BB84.EntityFrameworkCore.RepositoriesTests.Persistence.Models;

using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BB84.EntityFrameworkCore.RepositoriesTests.Persistence.Configurations;

internal sealed class PersonConfiguration : AuditedConfiguration<Person>
{
	public override void Configure(EntityTypeBuilder<Person> builder)
	{
		builder.ToHistoryTable();

		builder.Property(x => x.Settings)
			.HasXmlColumnType();

		builder.Property(x=>x.DateOfBirth)
			.HasDateColumnType();

		builder.Property(x => x.Salary)
			.HasMoneyColumnType(true);

		base.Configure(builder);
	}
}
