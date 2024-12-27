﻿using BB84.EntityFrameworkCore.Repositories.SqlServer.Configurations;
using BB84.EntityFrameworkCore.Repositories.Tests.Persistence.Entities;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BB84.EntityFrameworkCore.Repositories.Tests.Persistence.Configurations;

internal sealed class PersonJobConfiguration : AuditedCompositeConfiguration<PersonJob>
{
	public override void Configure(EntityTypeBuilder<PersonJob> builder)
	{
		base.Configure(builder);

		_ = builder.HasKey(e => new { e.PersonId, e.JobId })
			.IsClustered();
	}
}
