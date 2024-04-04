using BB84.EntityFrameworkCore.RepositoriesTests.Persistence;

namespace BB84.EntityFrameworkCore.RepositoriesTests;

[TestClass]
public sealed class UnitTest : UnitTestBase
{
	[TestMethod]
	public void TestDbContextTest()
	{
		using TestDbContext dbContext = new(GetContextOptions());

		dbContext.Database.EnsureCreated();
	}
}
