using System.Data;
using System.Data.Common;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace BB84.EntityFrameworkCore.Repositories.Extensions;

public static partial class DatabaseFacadeExtensions
{
	/// <summary>
	/// Returns the result of executing a stored procedure as a list of type <typeparamref name="T"/>.
	/// </summary>
	/// <typeparam name="T">The type of the result returned by the stored procedure.</typeparam>
	/// <param name="databaseFacade">The database facade to execute the stored procedure on.</param>
	/// <param name="schema">The schema of the stored procedure.</param>
	/// <param name="name">The name of the stored procedure.</param>
	/// <param name="parameters">The parameters to pass to the stored procedure.</param>
	/// <param name="outputParameter">An optional output parameter to capture the value returned by the stored procedure.</param>
	/// <returns>A list of results returned by the stored procedure.</returns>
	public static IReadOnlyList<T> ExecuteProcedure<T>(
		this DatabaseFacade databaseFacade,
		string schema,
		string name,
		DbParameter[] parameters,
		DbParameter? outputParameter = null
		)
	{
		ArgumentNullException.ThrowIfNull(databaseFacade);

		(string sql, object[] sqlParameters) = CreateProcedureCommand(schema, name, parameters, outputParameter);

		return [.. databaseFacade.SqlQueryRaw<T>(sql, sqlParameters)];
	}

	/// <summary>
	/// Returns the result of executing a stored procedure as a list of type <typeparamref name="T"/>.
	/// </summary>
	/// <typeparam name="T">The type of the result returned by the stored procedure.</typeparam>
	/// <param name="databaseFacade">The database facade to execute the stored procedure on.</param>
	/// <param name="schema">The schema of the stored procedure.</param>
	/// <param name="name">The name of the stored procedure.</param>
	/// <param name="parameters">The parameters to pass to the stored procedure.</param>
	/// <param name="outputParameter">An optional output parameter to capture the value returned by the stored procedure.</param>
	/// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
	/// <returns>A list of results returned by the stored procedure.</returns>
	public static async Task<IReadOnlyList<T>> ExecuteProcedureAsync<T>(
		this DatabaseFacade databaseFacade,
		string schema,
		string name,
		DbParameter[] parameters,
		DbParameter? outputParameter = null,
		CancellationToken cancellationToken = default)
	{
		ArgumentNullException.ThrowIfNull(databaseFacade);

		(string sql, object[] sqlParameters) = CreateProcedureCommand(schema, name, parameters, outputParameter);

		return await databaseFacade
			.SqlQueryRaw<T>(sql, sqlParameters)
			.ToListAsync(cancellationToken)
			.ConfigureAwait(false);
	}

	/// <summary>
	/// Returns the number of rows affected by executing a stored procedure.
	/// </summary>
	/// <param name="databaseFacade">The database facade to execute the stored procedure on.</param>
	/// <param name="schema">The schema of the stored procedure.</param>
	/// <param name="name">The name of the stored procedure.</param>
	/// <param name="parameters">The parameters to pass to the stored procedure.</param>
	/// <returns>The number of rows affected by the stored procedure.</returns>
	public static int ExecuteProcedure(
		this DatabaseFacade databaseFacade,
		string schema,
		string name,
		DbParameter[] parameters)
	{
		ArgumentNullException.ThrowIfNull(databaseFacade);

		(string sql, object[] sqlParameters) = CreateProcedureCommand(schema, name, parameters);

		return databaseFacade.ExecuteSqlRaw(sql, sqlParameters);
	}

	/// <summary>
	/// Returns the number of rows affected by executing a stored procedure.
	/// </summary>
	/// <param name="databaseFacade">The database facade to execute the stored procedure on.</param>
	/// <param name="schema">The schema of the stored procedure.</param>
	/// <param name="name">The name of the stored procedure.</param>
	/// <param name="parameters">The parameters to pass to the stored procedure.</param>
	/// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
	/// <returns>The number of rows affected by the stored procedure.</returns>
	public static async Task<int> ExecuteProcedureAsync(
		this DatabaseFacade databaseFacade,
		string schema,
		string name,
		DbParameter[] parameters,
		CancellationToken cancellationToken)
	{
		ArgumentNullException.ThrowIfNull(databaseFacade);

		(string sql, object[] sqlParameters) = CreateProcedureCommand(schema, name, parameters);

		return await databaseFacade
			.ExecuteSqlRawAsync(sql, sqlParameters, cancellationToken)
			.ConfigureAwait(false);
	}

	private static (string Sql, object[] Parameters) CreateProcedureCommand(
		string schema,
		string name,
		DbParameter[] parameters,
		DbParameter? outputParameter = null)
	{
		ArgumentNullException.ThrowIfNull(parameters);

		List<DbParameter> commandParameters = [.. parameters];

		if (outputParameter is not null && commandParameters.Any(p => string.Equals(p.ParameterName, outputParameter.ParameterName, StringComparison.OrdinalIgnoreCase)))
			throw new ArgumentException("The output parameter is already included in the parameters array.", nameof(outputParameter));

		if (outputParameter is not null)
			commandParameters.Add(outputParameter);

		List<string> assignments = [];

		foreach (DbParameter parameter in commandParameters)
		{
			string token = GetParameterToken(parameter);
			bool isOutput = parameter.Direction is ParameterDirection.Output or ParameterDirection.InputOutput;

			assignments.Add(isOutput
				? $"{token} = {token} OUTPUT"
				: $"{token} = {token}");
		}

		string sql = $"EXECUTE {GetObjectToken(schema, name)}";

		if (assignments.Count > 0)
			sql = $"{sql} {string.Join(", ", assignments)}";

		return (sql, [.. commandParameters]);
	}
}
