// Copyright: 2024 Robert Peter Meyer
// License: MIT
//
// This source code is licensed under the MIT license found in the
// LICENSE file in the root directory of this source tree.
using BB84.EntityFrameworkCore.Repositories.Configurations;
using BB84.EntityFrameworkCore.TestSupport.Entities;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BB84.EntityFrameworkCore.Repositories.Agnostic.Tests.Persistence.Configurations;

/// <remarks>
/// The SQL Server counterpart maps this to a temporal history table and the salary to
/// <c>decimal(10,2)</c>. Neither is available here, so the table is plain and the precision is
/// declared through the relational API instead.
/// </remarks>
internal sealed class JobConfiguration : IdentityConfiguration<JobEntity>
{
	public override void Configure(EntityTypeBuilder<JobEntity> builder)
	{
		_ = builder.ToTable("Jobs");

		_ = builder.Property(p => p.Salary)
			.HasPrecision(10, 2);

		base.Configure(builder);
	}
}
