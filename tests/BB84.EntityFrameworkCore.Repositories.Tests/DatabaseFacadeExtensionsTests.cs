// Copyright: 2024 Robert Peter Meyer
// License: MIT
//
// This source code is licensed under the MIT license found in the
// LICENSE file in the root directory of this source tree.
using System.Data;

using BB84.EntityFrameworkCore.Repositories.Extensions;
using BB84.EntityFrameworkCore.Repositories.Tests.Persistence;

using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace BB84.EntityFrameworkCore.Repositories.Tests;

[TestClass]
public sealed class DatabaseFacadeExtensionsTests : UnitTestBase
{
	private const string Schema = TestDbContext.DefaultSchema;
	private const string GetScalarValueFunctionName = "fn_GetScalarValue";
	private const string GetTableValuesFunctionName = "fn_GetTableValues";
	private const string GetValuesProcedureName = "sp_GetValues";
	private const string SetValueProcedureName = "sp_SetValue";
	private readonly CancellationToken _testToken = CancellationToken.None;

	[ClassInitialize]
	public static void ClassInitialize(TestContext context)
	{
		using TestDbContext dbContext = GetTestContext();

		dbContext.Database.ExecuteSqlRaw($"""
      CREATE FUNCTION [{Schema}].[{GetScalarValueFunctionName}](@Input INT)
      RETURNS INT
      AS
      BEGIN
        RETURN @Input * 2;
      END
      """);

		dbContext.Database.ExecuteSqlRaw($"""
      CREATE FUNCTION [{Schema}].[{GetTableValuesFunctionName}](@Count INT)
      RETURNS TABLE
      AS
      RETURN
      (
        SELECT TOP (@Count) [Value] = 1
        FROM sys.objects
      )
      """);

		dbContext.Database.ExecuteSqlRaw($"""
      CREATE PROCEDURE [{Schema}].[{GetValuesProcedureName}]
        @Input INT
      AS
      BEGIN
        SELECT [Value] = @Input;
      END
      """);

		dbContext.Database.ExecuteSqlRaw($"""
      CREATE PROCEDURE [{Schema}].[{SetValueProcedureName}]
        @Input INT,
        @Output INT OUTPUT
      AS
      BEGIN
        SET @Output = @Input * 3;
      END
      """);
	}

	[ClassCleanup]
	public static void ClassCleanup()
	{
		using TestDbContext dbContext = GetTestContext();

		dbContext.Database.ExecuteSqlRaw($"DROP FUNCTION IF EXISTS [{Schema}].[{GetScalarValueFunctionName}]");
		dbContext.Database.ExecuteSqlRaw($"DROP FUNCTION IF EXISTS [{Schema}].[{GetTableValuesFunctionName}]");
		dbContext.Database.ExecuteSqlRaw($"DROP PROCEDURE IF EXISTS [{Schema}].[{GetValuesProcedureName}]");
		dbContext.Database.ExecuteSqlRaw($"DROP PROCEDURE IF EXISTS [{Schema}].[{SetValueProcedureName}]");
	}

	[TestMethod]
	public void ExecuteScalarFunctionTest()
	{
		IEnumerable<SqlParameter> sqlParameters = [new("@Input", SqlDbType.Int) { Value = 5 }];

		int result = DbContext.Database
			.ExecuteScalarFunction<int>(Schema, GetScalarValueFunctionName, sqlParameters);

		Assert.AreEqual(10, result);
	}

	[TestMethod]
	public async Task ExecuteScalarFunctionAsyncTest()
	{
		IEnumerable<SqlParameter> sqlParameters = [new("@Input", SqlDbType.Int) { Value = 7 }];

		int result = await DbContext.Database
			.ExecuteScalarFunctionAsync<int>(Schema, GetScalarValueFunctionName, sqlParameters, _testToken)
			.ConfigureAwait(false);

		Assert.AreEqual(14, result);
	}

	[TestMethod]
	public void ExecuteTableFunctionTest()
	{
		IEnumerable<SqlParameter> sqlParameters = [new("@Count", SqlDbType.Int) { Value = 3 }];

		IReadOnlyList<int> result = DbContext.Database
			.ExecuteTableFunction<int>(Schema, GetTableValuesFunctionName, sqlParameters);

		Assert.HasCount(3, result);
	}

	[TestMethod]
	public async Task ExecuteTableFunctionAsyncTest()
	{
		IEnumerable<SqlParameter> sqlParameters = [new("@Count", SqlDbType.Int) { Value = 2 }];

		IReadOnlyList<int> result = await DbContext.Database
			.ExecuteTableFunctionAsync<int>(Schema, GetTableValuesFunctionName, sqlParameters, _testToken)
			.ConfigureAwait(false);

		Assert.HasCount(2, result);
	}

	[TestMethod]
	public void ExecuteProcedureWithResultTest()
	{
		SqlParameter input = new("@Input", SqlDbType.Int) { Value = 42 };

		IReadOnlyList<int> result = DbContext.Database
			.ExecuteProcedure<int>(Schema, GetValuesProcedureName, [input]);

		Assert.HasCount(1, result);
		Assert.AreEqual(42, result[0]);
	}

	[TestMethod]
	public async Task ExecuteProcedureWithResultAsyncTest()
	{
		SqlParameter input = new("@Input", SqlDbType.Int) { Value = 99 };

		IReadOnlyList<int> result = await DbContext.Database
			.ExecuteProcedureAsync<int>(Schema, GetValuesProcedureName, [input], cancellationToken: _testToken)
			.ConfigureAwait(false);

		Assert.HasCount(1, result);
		Assert.AreEqual(99, result[0]);
	}

	[TestMethod]
	public void ExecuteProcedureWithOutputParameterTest()
	{
		SqlParameter input = new("@Input", SqlDbType.Int) { Value = 4 };
		SqlParameter output = new("@Output", SqlDbType.Int) { Direction = ParameterDirection.Output };

		IReadOnlyList<int> result = DbContext.Database
			.ExecuteProcedure<int>(Schema, SetValueProcedureName, [input, output]);

		Assert.AreEqual(12, output.Value);
	}

	[TestMethod]
	public async Task ExecuteProcedureWithOutputParameterAsyncTest()
	{
		SqlParameter input = new("@Input", SqlDbType.Int) { Value = 5 };
		SqlParameter output = new("@Output", SqlDbType.Int) { Direction = ParameterDirection.Output };

		IReadOnlyList<int> result = await DbContext.Database
			.ExecuteProcedureAsync<int>(Schema, SetValueProcedureName, [input, output], _testToken)
			.ConfigureAwait(false);

		Assert.AreEqual(15, output.Value);
	}

	[TestMethod]
	public void ExecuteProcedureNonQueryTest()
	{
		SqlParameter input = new("@Input", SqlDbType.Int) { Value = 1 };

		int rowsAffected = DbContext.Database
			.ExecuteProcedure(Schema, GetValuesProcedureName, [input]);

		Assert.AreEqual(-1, rowsAffected);
	}

	[TestMethod]
	public async Task ExecuteProcedureNonQueryAsyncTest()
	{
		SqlParameter input = new("@Input", SqlDbType.Int) { Value = 1 };

		int rowsAffected = await DbContext.Database
			.ExecuteProcedureAsync(Schema, GetValuesProcedureName, [input], _testToken)
			.ConfigureAwait(false);

		Assert.AreEqual(-1, rowsAffected);
	}
}
