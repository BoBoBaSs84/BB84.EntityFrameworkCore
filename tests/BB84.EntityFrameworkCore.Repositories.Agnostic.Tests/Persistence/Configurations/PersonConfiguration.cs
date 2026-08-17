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
/// The densest of the SQL Server configurations: a temporal history table plus <c>xml</c>,
/// <c>date</c>, <c>datetimeoffset(7)</c>, <c>datetime2(7)</c> and <c>smallmoney</c> columns. None
/// of that survives the port, which is the point — what is left is what the agnostic package
/// actually provides.
/// </remarks>
internal sealed class PersonConfiguration : AuditedConfiguration<PersonEntity>
{
	public override void Configure(EntityTypeBuilder<PersonEntity> builder)
	{
		_ = builder.ToTable("Persons");

		_ = builder.Property(p => p.Salary)
			.HasPrecision(10, 2);

		base.Configure(builder);
	}
}
