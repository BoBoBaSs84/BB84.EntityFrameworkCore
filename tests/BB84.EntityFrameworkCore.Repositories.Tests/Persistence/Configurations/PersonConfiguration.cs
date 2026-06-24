// Copyright: 2024 Robert Peter Meyer
// License: MIT
//
// This source code is licensed under the MIT license found in the
// LICENSE file in the root directory of this source tree.
using BB84.EntityFrameworkCore.Repositories.SqlServer.Configurations;
using BB84.EntityFrameworkCore.Repositories.SqlServer.Extensions;
using BB84.EntityFrameworkCore.Repositories.Tests.Persistence.Entities;

using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BB84.EntityFrameworkCore.Repositories.Tests.Persistence.Configurations;

internal sealed class PersonConfiguration : AuditedConfiguration<PersonEntity>
{
	public override void Configure(EntityTypeBuilder<PersonEntity> builder)
	{
		builder.ToHistoryTable("Persons", TestDbContext.DefaultSchema);

		builder.Property(x => x.Settings)
			.IsXmlColumn();

		builder.Property(x => x.DateOfBirth)
			.IsDateColumn();

		builder.Property(p => p.StartDate)
			.IsDateTimeOffsetColumn();

		builder.Property(p => p.Evaluation)
			.IsDateTime2Column();

		builder.Property(x => x.Salary)
			.IsMoneyColumn(true);

		base.Configure(builder);
	}
}
