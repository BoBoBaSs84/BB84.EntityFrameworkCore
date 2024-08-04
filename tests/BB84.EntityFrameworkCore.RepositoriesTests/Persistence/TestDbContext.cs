using BB84.EntityFrameworkCore.Repositories.SqlServer.Interceptors;
using BB84.EntityFrameworkCore.RepositoriesTests.Abstractions;
using BB84.EntityFrameworkCore.RepositoriesTests.Persistence.Models;

using Microsoft.EntityFrameworkCore;

namespace BB84.EntityFrameworkCore.RepositoriesTests.Persistence;

[SuppressMessage("Style", "IDE0058", Justification = "Not relevant here, unit testing.")]
public sealed class TestDbContext(DbContextOptions<TestDbContext> options, SoftDeletableInterceptor softDeletableInterceptor) : DbContext(options), ITestDbContext
{
	private readonly SoftDeletableInterceptor _softDeletableInterceptor = softDeletableInterceptor;

	public DbSet<Person> Persons { get; set; }
	public DbSet<PersonType> PersonType { get; set; }

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		base.OnModelCreating(modelBuilder);
		modelBuilder.ApplyConfigurationsFromAssembly(typeof(UnitTestBase).Assembly);
	}

	protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
	{
		base.OnConfiguring(optionsBuilder);
		optionsBuilder.AddInterceptors(_softDeletableInterceptor);
	}
}
