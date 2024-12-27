using BB84.EntityFrameworkCore.Repositories.SqlServer.Interceptors;
using BB84.EntityFrameworkCore.Repositories.Tests.Persistence;
using BB84.EntityFrameworkCore.Repositories.Tests.Persistence.Interceptors;

using Microsoft.EntityFrameworkCore;

namespace BB84.EntityFrameworkCore.Repositories.Tests;

[TestClass]
[SuppressMessage("Style", "IDE0058", Justification = "Not relevant here, unit testing.")]
public abstract class UnitTestBase
{
	private static readonly SoftDeletableInterceptor SoftDeletableInterceptor = new();
	private static readonly TimeAuditedInterceptor TimeAuditedInterceptor = new();
	private static readonly UserAuditedInterceptor UserAuditedInterceptor = new();

	[AssemblyInitialize]
	public static void AssemblyInitialize(TestContext context)
	{
		using TestDbContext dbContext = GetTestContext();
		dbContext.Database.EnsureCreated();
	}

	[AssemblyCleanup]
	public static void AssemblyCleanup()
	{
		using TestDbContext dbContext = GetTestContext();
		dbContext.Database.EnsureDeleted();
	}

	[TestMethod]
	public void GenerateCreateScriptTest()
	{
		using TestDbContext dbContext = GetTestContext();
		string sqlScript = dbContext.Database.GenerateCreateScript();
		File.WriteAllText("CreateScript.sql", sqlScript);
	}

	public static TestDbContext GetTestContext()
		=> new(GetContextOptions(), SoftDeletableInterceptor, TimeAuditedInterceptor, UserAuditedInterceptor);

	private static DbContextOptions<TestDbContext> GetContextOptions()
	{
		return new DbContextOptionsBuilder<TestDbContext>()
			.UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=TestDb;Integrated Security=True")
			.Options;
	}
}
