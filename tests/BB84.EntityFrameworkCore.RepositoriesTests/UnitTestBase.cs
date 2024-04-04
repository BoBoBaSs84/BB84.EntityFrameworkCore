using BB84.EntityFrameworkCore.RepositoriesTests.Persistence;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace BB84.EntityFrameworkCore.RepositoriesTests;

[TestClass]
public abstract class UnitTestBase
{
	public static DbContextOptions<TestDbContext> GetContextOptions()
	{
		return new DbContextOptionsBuilder<TestDbContext>()
			.UseInMemoryDatabase("TestContext")
			.ConfigureWarnings(config => config.Ignore(InMemoryEventId.TransactionIgnoredWarning))
			.Options;
	}
}
