// Copyright: 2024 Robert Peter Meyer
// License: MIT
//
// This source code is licensed under the MIT license found in the
// LICENSE file in the root directory of this source tree.
using BB84.EntityFrameworkCore.Repositories.SqlServer.Configurations;
using BB84.EntityFrameworkCore.Repositories.Tests.Persistence.Entities;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BB84.EntityFrameworkCore.Repositories.Tests.Persistence.Configurations;

internal sealed class PersonTypeConfiguration : EnumeratorConfiguration<PersonTypeEntity>
{
	public override void Configure(EntityTypeBuilder<PersonTypeEntity> builder)
	{
		builder.ToTable("PersonType");

		_ = builder.HasData(new List<PersonTypeEntity>()
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
