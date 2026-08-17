// Copyright: 2024 Robert Peter Meyer
// License: MIT
//
// This source code is licensed under the MIT license found in the
// LICENSE file in the root directory of this source tree.
using BB84.EntityFrameworkCore.Repositories.Abstractions;
using BB84.EntityFrameworkCore.Repositories.Agnostic.Tests.Persistence;
using BB84.EntityFrameworkCore.Repositories.Interceptors;

using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace BB84.EntityFrameworkCore.Repositories.Agnostic.Tests;

/// <summary>
/// The fixture for the provider-agnostic suite: a private SQLite database per test.
/// </summary>
/// <remarks>
/// <para>
/// Each test gets its own in-memory database, created and seeded in
/// <see cref="TestInitialize"/> and destroyed in <see cref="TestCleanup"/>. The database lives
/// exactly as long as the connection, which is why the connection object rather than a connection
/// string is handed to <c>UseSqlite</c> — EF leaves an already-open connection open.
/// </para>
/// <para>
/// This is the one deliberate difference from the SQL Server fixture, which shares one stateful
/// database across the whole assembly and needs tests to clean up after themselves. Per-test
/// isolation costs milliseconds here and removes that coupling: every test starts from the three
/// seeded person types and empty tables otherwise.
/// </para>
/// <para>
/// Two SQLite traits to keep in mind when writing assertions. <c>HasMaxLength</c> and
/// <c>IsUnicode</c> are ignored, so a length limit is not enforced. String comparison differs from
/// SQL Server's default collation: <c>Contains</c> becomes a case-sensitive <c>instr()</c> while
/// <c>StartsWith</c> becomes a case-insensitive <c>LIKE</c>.
/// </para>
/// </remarks>
[TestClass]
public abstract class UnitTestBase : IDisposable
{
	private SqliteConnection _connection = default!;

	/// <summary>
	/// The database context for the currently running test.
	/// </summary>
	protected TestDbContext DbContext { get; private set; } = default!;

	[TestInitialize]
	public void TestInitialize()
	{
		_connection = new SqliteConnection("Data Source=:memory:");
		_connection.Open();

		DbContext = CreateContext();

		_ = DbContext.Database.EnsureCreated();

		RowVersionEmulation.CreateTriggers(DbContext);
	}

	[TestCleanup]
	public void TestCleanup()
		=> Dispose();

	/// <summary>
	/// Builds a context against the database of the currently running test.
	/// </summary>
	/// <remarks>
	/// Pass a clock or a user provider to exercise the audit interceptors under controlled
	/// conditions; the defaults match what a consumer would get out of the box.
	/// </remarks>
	/// <param name="timeProvider">The clock the time audit interceptor reads.</param>
	/// <param name="currentUserProvider">The identity the user audit interceptor records.</param>
	protected TestDbContext CreateContext(TimeProvider? timeProvider = null, ICurrentUserProvider? currentUserProvider = null)
		=> new(
			GetContextOptions(),
			new SoftDeletableInterceptor(),
			new TimeAuditedInterceptor(timeProvider),
			new UserAuditedInterceptor(currentUserProvider ?? new EnvironmentUserProvider()));

	/// <summary>
	/// The options pointing at the database of the currently running test.
	/// </summary>
	protected DbContextOptions<TestDbContext> GetContextOptions()
		=> new DbContextOptionsBuilder<TestDbContext>()
			.UseSqlite(_connection)
			.Options;

	/// <summary>
	/// Disposes the context and the connection, which destroys the in-memory database.
	/// </summary>
	/// <remarks>
	/// Both are idempotent, so it does not matter whether MSTest reaches this through
	/// <see cref="TestCleanup"/> or through <see cref="IDisposable"/>.
	/// </remarks>
	public void Dispose()
	{
		DbContext?.Dispose();
		_connection?.Dispose();

		GC.SuppressFinalize(this);
	}
}
