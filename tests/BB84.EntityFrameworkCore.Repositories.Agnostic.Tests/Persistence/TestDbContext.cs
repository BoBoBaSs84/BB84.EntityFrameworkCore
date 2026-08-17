// Copyright: 2024 Robert Peter Meyer
// License: MIT
//
// This source code is licensed under the MIT license found in the
// LICENSE file in the root directory of this source tree.
using BB84.EntityFrameworkCore.Repositories.Interceptors;
using BB84.EntityFrameworkCore.TestSupport.Abstractions;

using Microsoft.EntityFrameworkCore;

namespace BB84.EntityFrameworkCore.Repositories.Agnostic.Tests.Persistence;

/// <summary>
/// The context for the provider-agnostic suite.
/// </summary>
/// <remarks>
/// Deliberately schema-less: SQLite has no schemas, so there is no counterpart to the
/// <c>HasDefaultSchema</c> call the SQL Server context makes. The configurations are discovered
/// from this assembly, which is what keeps the two suites' models apart.
/// </remarks>
public sealed class TestDbContext(
	DbContextOptions<TestDbContext> options,
	SoftDeletableInterceptor softDeletableInterceptor,
	TimeAuditedInterceptor timeAuditedInterceptor,
	UserAuditedInterceptor userAuditedInterceptor) : DbContext(options), ITestDbContext
{
	/// <inheritdoc/>
	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		base.OnModelCreating(modelBuilder);

		_ = modelBuilder.ApplyConfigurationsFromAssembly(typeof(TestDbContext).Assembly);

		RowVersionEmulation.ApplyToModel(modelBuilder);
	}

	/// <inheritdoc/>
	protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
	{
		base.OnConfiguring(optionsBuilder);

		_ = optionsBuilder.AddInterceptors(softDeletableInterceptor, timeAuditedInterceptor, userAuditedInterceptor);
	}
}
