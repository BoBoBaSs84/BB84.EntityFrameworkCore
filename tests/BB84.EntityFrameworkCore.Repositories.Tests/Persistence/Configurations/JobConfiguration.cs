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

internal sealed class JobConfiguration : IdentityConfiguration<JobEntity>
{
	public override void Configure(EntityTypeBuilder<JobEntity> builder)
	{
		builder.ToHistoryTable("Jobs", "tab", "OldJobs", "hist");

		builder.Property(p => p.Salary)
			.IsDecimalColumn(10, 2);

		base.Configure(builder);
	}
}
