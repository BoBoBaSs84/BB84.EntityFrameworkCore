using BB84.EntityFrameworkCore.RepositoriesTests.Persistence;

using Microsoft.EntityFrameworkCore;

namespace BB84.EntityFrameworkCore.RepositoriesTests;

[TestClass]
public abstract class UnitTestBase
{
	[AssemblyInitialize]
	public static void AssemblyInitialize(TestContext context)
	{
		using TestDbContext dbContext = new(GetContextOptions());

		dbContext.Database.EnsureCreated();
	}

	[AssemblyCleanup]
	public static void AssemblyCleanup()
	{
		using TestDbContext dbContext = new(GetContextOptions());

		dbContext.Database.EnsureDeleted();
	}

	public static DbContextOptions<TestDbContext> GetContextOptions()
	{
		return new DbContextOptionsBuilder<TestDbContext>()
			.UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=TestDb;Integrated Security=True")
			.Options;
	}
}
