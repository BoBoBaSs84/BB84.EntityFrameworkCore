// Copyright: 2024 Robert Peter Meyer
// License: MIT
//
// This source code is licensed under the MIT license found in the
// LICENSE file in the root directory of this source tree.
using BB84.EntityFrameworkCore.Repositories.Interceptors;
using BB84.EntityFrameworkCore.Repositories.Tests.Persistence;

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
	private static readonly UserAuditedInterceptor UserAuditedInterceptor = new(new EnvironmentUserProvider());
	private static readonly MsSqlContainer Container = new MsSqlBuilder(ContainerImage).Build();

	private static string? s_connectionString;

	[AssemblyInitialize]
	public static async Task AssemblyInitialize(TestContext context)
	{
		await Container.StartAsync(context.CancellationToken)
			.ConfigureAwait(false);

		// The container hands out a connection string pointing at "master";
		// the test database itself is created by "EnsureCreated" below.
		s_connectionString = new SqlConnectionStringBuilder(Container.GetConnectionString())
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

	/// <summary>
	/// The options pointing at the shared test database.
	/// </summary>
	/// <remarks>
	/// Public so that a test needing its own interceptors can build a context itself instead
	/// of going through <see cref="GetTestContext"/>, which uses the shared ones.
	/// </remarks>
	public static DbContextOptions<TestDbContext> GetContextOptions()
	{
		return s_connectionString is null
			? throw new InvalidOperationException($"The database container has not been started, '{nameof(AssemblyInitialize)}' must run first.")
			: new DbContextOptionsBuilder<TestDbContext>()
				.UseSqlServer(s_connectionString)
				.Options;
	}
}
