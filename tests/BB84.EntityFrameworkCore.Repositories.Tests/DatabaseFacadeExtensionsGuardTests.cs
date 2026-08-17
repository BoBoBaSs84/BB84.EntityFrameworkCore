// Copyright: 2024 Robert Peter Meyer
// License: MIT
//
// This source code is licensed under the MIT license found in the
// LICENSE file in the root directory of this source tree.
using System.Data.Common;

using BB84.EntityFrameworkCore.Repositories.SqlServer.Extensions;

using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace BB84.EntityFrameworkCore.Repositories.Tests;

/// <summary>
/// Covers the argument guards of <see cref="DatabaseFacadeExtensions"/>.
/// </summary>
/// <remarks>
/// The guards run before any SQL is composed, so these need no container: the context below
/// points at a connection string that is never opened. <see cref="DatabaseFacadeExtensionsTests"/>
/// covers the behaviour that does need a database.
/// </remarks>
[TestClass]
public sealed class DatabaseFacadeExtensionsGuardTests
{
	private const string Schema = "testing";
	private const string Name = "fn_Whatever";

	private static readonly DbParameter[] NoParameters = [];

	[TestMethod]
	public void ShouldRejectANullFacade()
	{
		DatabaseFacade facade = null!;

		AssertRejects("databaseFacade", () => facade.ExecuteScalarFunction<int>(Schema, Name, NoParameters));
		AssertRejects("databaseFacade", () => facade.ExecuteTableFunction<int>(Schema, Name, NoParameters));
		AssertRejects("databaseFacade", () => facade.ExecuteProcedure<int>(Schema, Name, NoParameters));
		AssertRejects("databaseFacade", () => facade.ExecuteProcedure(Schema, Name, NoParameters));
	}

	[TestMethod]
	public async Task ShouldRejectANullFacadeAsynchronously()
	{
		DatabaseFacade facade = null!;

		await AssertRejectsAsync("databaseFacade", () => facade.ExecuteScalarFunctionAsync<int>(Schema, Name, NoParameters));
		await AssertRejectsAsync("databaseFacade", () => facade.ExecuteTableFunctionAsync<int>(Schema, Name, NoParameters));
		await AssertRejectsAsync("databaseFacade", () => facade.ExecuteProcedureAsync<int>(Schema, Name, NoParameters));
		await AssertRejectsAsync("databaseFacade", () => facade.ExecuteProcedureAsync(Schema, Name, NoParameters));
	}

	[TestMethod]
	public void ShouldRejectNullParameters()
	{
		using OfflineContext context = new();
		DatabaseFacade facade = context.Database;

		AssertRejects("parameters", () => facade.ExecuteScalarFunction<int>(Schema, Name, null!));
		AssertRejects("parameters", () => facade.ExecuteTableFunction<int>(Schema, Name, null!));
	}

	[TestMethod]
	[DataRow(null)]
	[DataRow("")]
	[DataRow("   ")]
	public void ShouldRejectABlankSchema(string? schema)
	{
		using OfflineContext context = new();

		ArgumentException exception = Assert.Throws<ArgumentException>(
			() => context.Database.ExecuteScalarFunction<int>(schema!, Name, NoParameters));

		Assert.AreEqual("schema", exception.ParamName);
	}

	[TestMethod]
	[DataRow(null)]
	[DataRow("")]
	[DataRow("   ")]
	public void ShouldRejectABlankName(string? name)
	{
		using OfflineContext context = new();

		ArgumentException exception = Assert.Throws<ArgumentException>(
			() => context.Database.ExecuteScalarFunction<int>(Schema, name!, NoParameters));

		Assert.AreEqual("name", exception.ParamName);
	}

	/// <summary>
	/// The hand-rolled guard, which is the one most likely to drift from the others.
	/// </summary>
	[TestMethod]
	public void ShouldRejectAParameterWithoutAName()
	{
		using OfflineContext context = new();
		SqlParameter[] unnamed = [new()];

		ArgumentException exception = Assert.ThrowsExactly<ArgumentException>(
			() => context.Database.ExecuteScalarFunction<int>(Schema, Name, unnamed));

		Assert.AreEqual("parameter", exception.ParamName);
	}

	private static void AssertRejects(string parameterName, Action action)
	{
		ArgumentNullException exception = Assert.ThrowsExactly<ArgumentNullException>(action);

		Assert.AreEqual(parameterName, exception.ParamName);
	}

	private static async Task AssertRejectsAsync(string parameterName, Func<Task> action)
	{
		ArgumentNullException exception = await Assert.ThrowsExactlyAsync<ArgumentNullException>(action);

		Assert.AreEqual(parameterName, exception.ParamName);
	}

	/// <summary>
	/// A context whose connection is never opened. Enough to hand out a
	/// <see cref="DatabaseFacade"/>, which is all the guards need.
	/// </summary>
	private sealed class OfflineContext : DbContext
	{
		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
			=> optionsBuilder.UseSqlServer("Server=nowhere;Database=none;Trusted_Connection=True;");
	}
}
