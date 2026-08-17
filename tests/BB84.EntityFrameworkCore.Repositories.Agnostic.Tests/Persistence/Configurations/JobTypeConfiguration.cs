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
/// The SQL Server counterpart types <c>Color</c> as <c>varbinary(3)</c>, which is an odd fit for a
/// <see cref="string"/> property. Nothing provider-specific replaces it here.
/// </remarks>
internal sealed class JobTypeConfiguration : CompositeConfiguration<JobTypeEntity>
{
	public override void Configure(EntityTypeBuilder<JobTypeEntity> builder)
	{
		_ = builder.ToTable("JobTypes")
			.HasNoKey();

		base.Configure(builder);
	}
}
