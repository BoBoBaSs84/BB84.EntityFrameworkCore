using BB84.EntityFrameworkCore.RepositoriesTests.Persistence.Models;

using Microsoft.EntityFrameworkCore;

namespace BB84.EntityFrameworkCore.RepositoriesTests.Persistence;

public sealed class TestDbContext(DbContextOptions<TestDbContext> options) : DbContext(options)
{
	public DbSet<Person> People { get; set; }
	public DbSet<Trait> Traits { get; set; }
	public DbSet<Salutation> Salations { get; set; }

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		base.OnModelCreating(modelBuilder);
		_ = modelBuilder.ApplyConfigurationsFromAssembly(typeof(UnitTestBase).Assembly);
	}
}
