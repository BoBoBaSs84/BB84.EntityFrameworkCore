// Copyright: 2024 Robert Peter Meyer
// License: MIT
//
// This source code is licensed under the MIT license found in the
// LICENSE file in the root directory of this source tree.
using System.Data.Common;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace BB84.EntityFrameworkCore.Repositories.SqlServer.Extensions;

public static partial class DatabaseFacadeExtensions
{
	/// <summary>
	/// Returns the result of executing a table-valued function with the specified parameters.
	/// </summary>
	/// <typeparam name="T">The type of the result returned by the table-valued function.</typeparam>
	/// <param name="databaseFacade">The database facade to execute the table-valued function on.</param>
	/// <param name="schema">The schema of the table-valued function.</param>
	/// <param name="name">The name of the table-valued function.</param>
	/// <param name="parameters">The parameters to pass to the table-valued function.</param>
	/// <returns>The result of the table-valued function.</returns>
	public static IReadOnlyList<T> ExecuteTableFunction<T>(
		this DatabaseFacade databaseFacade,
		string schema,
		string name,
		IEnumerable<DbParameter> parameters)
	{
		ArgumentNullException.ThrowIfNull(databaseFacade);
		ArgumentNullException.ThrowIfNull(parameters);

		DbParameter[] materialized = [.. parameters];
		string sql = CreateTableFunctionCommand(schema, name, materialized);

		return [.. databaseFacade.SqlQueryRaw<T>(sql, materialized)];
	}

	/// <summary>
	/// Returns the result of executing a table-valued function with the specified parameters asynchronously.
	/// </summary>
	/// <typeparam name="T">The type of the result returned by the table-valued function.</typeparam>
	/// <param name="databaseFacade">The database facade to execute the table-valued function on.</param>
	/// <param name="schema">The schema of the table-valued function.</param>
	/// <param name="name">The name of the table-valued function.</param>
	/// <param name="parameters">The parameters to pass to the table-valued function.</param>
	/// <param name="cancellationToken">The cancellation token to cancel the operation if needed.</param>
	/// <returns>The result of the table-valued function.</returns>
	public static async Task<IReadOnlyList<T>> ExecuteTableFunctionAsync<T>(
		this DatabaseFacade databaseFacade,
		string schema,
		string name,
		IEnumerable<DbParameter> parameters,
		CancellationToken cancellationToken = default)
	{
		ArgumentNullException.ThrowIfNull(databaseFacade);
		ArgumentNullException.ThrowIfNull(parameters);

		DbParameter[] materialized = [.. parameters];
		string sql = CreateTableFunctionCommand(schema, name, materialized);

		return await databaseFacade
			.SqlQueryRaw<T>(sql, materialized)
			.ToListAsync(cancellationToken)
			.ConfigureAwait(false);
	}

	/// <summary>
	/// Returns the result of executing a scalar-valued function with the specified parameters.
	/// </summary>
	/// <typeparam name="T">The scalar type returned by the function.</typeparam>
	/// <param name="databaseFacade">The database facade to execute the scalar-valued function on.</param>
	/// <param name="schema">The schema of the scalar-valued function.</param>
	/// <param name="name">The name of the scalar-valued function.</param>
	/// <param name="parameters">The parameters to pass to the scalar-valued function.</param>
	/// <returns>The scalar result of the function.</returns>
	public static T? ExecuteScalarFunction<T>(
		this DatabaseFacade databaseFacade,
		string schema,
		string name,
		IEnumerable<DbParameter> parameters)
	{
		ArgumentNullException.ThrowIfNull(databaseFacade);
		ArgumentNullException.ThrowIfNull(parameters);

		DbParameter[] materialized = [.. parameters];
		string sql = CreateScalarFunctionCommand(schema, name, materialized);

		return databaseFacade
			.SqlQueryRaw<T>(sql, materialized)
			.SingleOrDefault();
	}

	/// <summary>
	/// Returns the result of executing a scalar-valued function with the specified parameters asynchronously.
	/// </summary>
	/// <typeparam name="T">The scalar type returned by the function.</typeparam>
	/// <param name="databaseFacade">The database facade to execute the scalar-valued function on.</param>
	/// <param name="schema">The schema of the scalar-valued function.</param>
	/// <param name="name">The name of the scalar-valued function.</param>
	/// <param name="parameters">The parameters to pass to the scalar-valued function.</param>
	/// <param name="cancellationToken">The cancellation token to cancel the operation if needed.</param>
	/// <returns>The scalar result of the function.</returns>
	public static async Task<T?> ExecuteScalarFunctionAsync<T>(
		this DatabaseFacade databaseFacade,
		string schema,
		string name,
		IEnumerable<DbParameter> parameters,
		CancellationToken cancellationToken = default)
	{
		ArgumentNullException.ThrowIfNull(databaseFacade);
		ArgumentNullException.ThrowIfNull(parameters);

		DbParameter[] materialized = [.. parameters];
		string sql = CreateScalarFunctionCommand(schema, name, materialized);

		return await databaseFacade
			.SqlQueryRaw<T>(sql, materialized)
			.SingleOrDefaultAsync(cancellationToken)
			.ConfigureAwait(false);
	}

	private static string CreateScalarFunctionCommand(string schema, string name, IReadOnlyCollection<DbParameter> parameters)
	{
		string parameterPlaceholders = string.Join(", ", parameters.Select(GetParameterToken));

		return $"SELECT [Value] = {GetObjectToken(schema, name)}({parameterPlaceholders})";
	}

	private static string CreateTableFunctionCommand(string schema, string name, IReadOnlyCollection<DbParameter> parameters)
	{
		string parameterPlaceholders = string.Join(", ", parameters.Select(GetParameterToken));

		return $"SELECT * FROM {GetObjectToken(schema, name)}({parameterPlaceholders})";
	}
}
