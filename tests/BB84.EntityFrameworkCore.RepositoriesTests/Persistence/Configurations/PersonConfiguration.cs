using BB84.EntityFrameworkCore.Repositories.SqlServer.Configurations;
using BB84.EntityFrameworkCore.Repositories.SqlServer.Extensions;
using BB84.EntityFrameworkCore.RepositoriesTests.Persistence.Models;

using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BB84.EntityFrameworkCore.RepositoriesTests.Persistence.Configurations;

internal sealed class PersonConfiguration : AuditedConfiguration<Person>
{
	public override void Configure(EntityTypeBuilder<Person> builder)
	{
		base.Configure(builder);

		_ = builder.ToHistoryTable();
	}
}
