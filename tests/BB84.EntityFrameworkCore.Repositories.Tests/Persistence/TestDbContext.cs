using BB84.EntityFrameworkCore.Repositories.SqlServer.Interceptors;
using BB84.EntityFrameworkCore.Repositories.Tests.Abstractions;
using BB84.EntityFrameworkCore.Repositories.Tests.Persistence.Interceptors;

using Microsoft.EntityFrameworkCore;

namespace BB84.EntityFrameworkCore.Repositories.Tests.Persistence;

[SuppressMessage("Style", "IDE0058", Justification = "Not relevant here, unit testing.")]
public sealed class TestDbContext(DbContextOptions<TestDbContext> options, SoftDeletableInterceptor softDeletableInterceptor, TimeAuditedInterceptor timeAuditedInterceptor, UserAuditedInterceptor userAuditedInterceptor) : DbContext(options), ITestDbContext
{
	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		base.OnModelCreating(modelBuilder);

		modelBuilder.HasDefaultSchema("testing");
		modelBuilder.ApplyConfigurationsFromAssembly(typeof(UnitTestBase).Assembly);
	}

	protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
	{
		base.OnConfiguring(optionsBuilder);

		optionsBuilder.AddInterceptors(softDeletableInterceptor, timeAuditedInterceptor, userAuditedInterceptor);
	}
}
