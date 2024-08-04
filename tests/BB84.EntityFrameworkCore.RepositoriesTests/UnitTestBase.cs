using BB84.EntityFrameworkCore.Repositories.SqlServer.Interceptors;
using BB84.EntityFrameworkCore.RepositoriesTests.Persistence;

using Microsoft.EntityFrameworkCore;

namespace BB84.EntityFrameworkCore.RepositoriesTests;

[TestClass]
[SuppressMessage("Style", "IDE0058", Justification = "Not relevant here, unit testing.")]
public abstract class UnitTestBase
{
	private static readonly SoftDeletableInterceptor Interceptor = new();

	[AssemblyInitialize]
	public static void AssemblyInitialize(TestContext context)
	{
		using TestDbContext dbContext = new(GetContextOptions(), Interceptor);

		dbContext.Database.EnsureCreated();
	}

	[AssemblyCleanup]
	public static void AssemblyCleanup()
	{
		using TestDbContext dbContext = new(GetContextOptions(), Interceptor);

		dbContext.Database.EnsureDeleted();
	}

	public static DbContextOptions<TestDbContext> GetContextOptions()
	{
		return new DbContextOptionsBuilder<TestDbContext>()
			.UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=TestDb;Integrated Security=True")
			.Options;
	}

	public static TestDbContext GetTestContext()
		=> new(GetContextOptions(), Interceptor);
}
