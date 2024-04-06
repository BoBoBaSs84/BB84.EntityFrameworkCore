using BB84.EntityFrameworkCore.Repositories.Configurations;
using BB84.EntityFrameworkCore.RepositoriesTests.Persistence.Models;

using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BB84.EntityFrameworkCore.RepositoriesTests.Persistence.Configurations;

internal sealed class PersonTypeConfiguration : EnumeratorBaseConfiguration<PersonType>
{
	public override void Configure(EntityTypeBuilder<PersonType> builder)
	{
		builder.HasData(new List<PersonType>()
		{
			new()
			{
				Id = 1,
				Name = "Female",
				Description = "A female person."
			},
			new()
			{
				Id = 2,
				Name = "Male",
				Description = "A male person."
			},
			new()
			{
				Id = 3,
				Name = "Diverse",
				Description = "A diverse person."
			}
		});

		base.Configure(builder);
	}
}
