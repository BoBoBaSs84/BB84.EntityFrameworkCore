using BB84.EntityFrameworkCore.Repositories.Configurations;
using BB84.EntityFrameworkCore.RepositoriesTests.Persistence.Models;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BB84.EntityFrameworkCore.RepositoriesTests.Persistence.Configurations;

internal sealed class PersonJobConfiguration : CompositeBaseConfiguration<PersonJob>
{
	public override void Configure(EntityTypeBuilder<PersonJob> builder)
	{
		builder.HasKey(e => new { e.PersonId, e.JobId })
			.IsClustered(false);

		base.Configure(builder);
	}
}
