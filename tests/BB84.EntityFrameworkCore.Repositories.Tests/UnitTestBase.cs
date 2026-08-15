// Copyright: 2024 Robert Peter Meyer
// License: MIT
//
// This source code is licensed under the MIT license found in the
// LICENSE file in the root directory of this source tree.
using BB84.EntityFrameworkCore.Repositories.SqlServer.Interceptors;
using BB84.EntityFrameworkCore.Repositories.Tests.Persistence;
using BB84.EntityFrameworkCore.Repositories.Tests.Persistence.Interceptors;

using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

using Testcontainers.MsSql;

namespace BB84.EntityFrameworkCore.Repositories.Tests;

[TestClass]
[SuppressMessage("Style", "IDE0058", Justification = "Not relevant here, unit testing.")]
public abstract class UnitTestBase
{
	private const string DatabaseName = "TestDb";
	private const string ContainerImage = "mcr.microsoft.com/mssql/server:2022-latest";

	private static readonly SoftDeletableInterceptor SoftDeletableInterceptor = new();
	private static readonly TimeAuditedInterceptor TimeAuditedInterceptor = new();
	private static readonly UserAuditedInterceptor UserAuditedInterceptor = new();
	private static readonly MsSqlContainer Container = new MsSqlBuilder(ContainerImage).Build();

	private static string? connectionString;

	[AssemblyInitialize]
	public static async Task AssemblyInitialize(TestContext context)
	{
		await Container.StartAsync()
			.ConfigureAwait(false);

		// The container hands out a connection string pointing at "master";
		// the test database itself is created by "EnsureCreated" below.
		connectionString = new SqlConnectionStringBuilder(Container.GetConnectionString())
		{
			InitialCatalog = DatabaseName
		}.ConnectionString;

		using TestDbContext dbContext = GetTestContext();
		dbContext.Database.EnsureCreated();
	}

	[AssemblyCleanup]
	public static async Task AssemblyCleanup()
	{
		using (TestDbContext dbContext = GetTestContext())
			dbContext.Database.EnsureDeleted();

		await Container.DisposeAsync()
			.ConfigureAwait(false);
	}

	/// <summary>
	/// The database context for the currently running test.
	/// </summary>
	/// <remarks>
	/// A fresh context is created before each test and disposed afterwards. Static members
	/// such as <c>[ClassInitialize]</c> run outside of that lifetime and must keep calling
	/// <see cref="GetTestContext"/> themselves.
	/// </remarks>
	protected TestDbContext DbContext { get; private set; } = default!;

	[TestInitialize]
	public void TestInitialize()
		=> DbContext = GetTestContext();

	[TestCleanup]
	public void TestCleanup()
		=> DbContext.Dispose();

	public static TestDbContext GetTestContext()
		=> new(GetContextOptions(), SoftDeletableInterceptor, TimeAuditedInterceptor, UserAuditedInterceptor);

	private static DbContextOptions<TestDbContext> GetContextOptions()
	{
		if (connectionString is null)
			throw new InvalidOperationException($"The database container has not been started, '{nameof(AssemblyInitialize)}' must run first.");

		return new DbContextOptionsBuilder<TestDbContext>()
			.UseSqlServer(connectionString)
			.Options;
	}
}
