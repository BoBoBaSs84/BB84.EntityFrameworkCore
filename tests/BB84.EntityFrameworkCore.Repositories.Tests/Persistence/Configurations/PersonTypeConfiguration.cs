using BB84.EntityFrameworkCore.Repositories.SqlServer.Configurations;
using BB84.EntityFrameworkCore.Repositories.Tests.Persistence.Entities;

using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BB84.EntityFrameworkCore.Repositories.Tests.Persistence.Configurations;

internal sealed class PersonTypeConfiguration : EnumeratorConfiguration<PersonType>
{
	public override void Configure(EntityTypeBuilder<PersonType> builder)
	{
		_ = builder.HasData(new List<PersonType>()
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
