using BB84.EntityFrameworkCore.Repositories.Configurations;
using BB84.EntityFrameworkCore.RepositoriesTests.Persistence.Models;

using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BB84.EntityFrameworkCore.RepositoriesTests.Persistence.Configurations;

internal sealed class PersonTraitConfiguration : CompositeBaseConfiguration<PersonTrait>
{
	public override void Configure(EntityTypeBuilder<PersonTrait> builder)
	{
		builder.HasKey(e => new { e.PersonId, e.TraitId });

		base.Configure(builder);
	}
}
