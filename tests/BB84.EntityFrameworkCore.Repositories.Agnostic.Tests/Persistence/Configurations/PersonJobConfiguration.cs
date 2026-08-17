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
/// Identical to the SQL Server counterpart apart from the <c>IsClustered(false)</c> call, which is
/// an annotation no other provider reads.
/// </remarks>
internal sealed class PersonJobConfiguration : AuditedCompositeConfiguration<PersonJobEntity>
{
	public override void Configure(EntityTypeBuilder<PersonJobEntity> builder)
	{
		_ = builder.ToTable("PersonJobs");

		_ = builder.HasKey(e => new { e.PersonId, e.JobId });

		base.Configure(builder);
	}
}
