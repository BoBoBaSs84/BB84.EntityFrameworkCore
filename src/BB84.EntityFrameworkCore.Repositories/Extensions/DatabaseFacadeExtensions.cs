using System.Data.Common;

using Microsoft.EntityFrameworkCore.Infrastructure;

namespace BB84.EntityFrameworkCore.Repositories.Extensions;

/// <summary>
/// Represents extension methods for the <see cref="DatabaseFacade"/> class to execute stored
/// procedures, table-valued functions and scalar functions with parameters.
/// </summary>
public static partial class DatabaseFacadeExtensions
{
	/// <summary>
	/// Sanitizes the parameter name and returns the token to be used in the SQL command.
	/// </summary>
	/// <param name="parameter">The parameter to get the token for.</param>
	/// <returns>The token for the specified parameter.</returns>
	/// <exception cref="ArgumentException">Thrown when the parameter name is null or whitespace.</exception>
	private static string GetParameterToken(DbParameter parameter)
	{
		ArgumentNullException.ThrowIfNull(parameter);

		if (string.IsNullOrWhiteSpace(parameter.ParameterName))
			throw new ArgumentException("ParameterName cannot be null or whitespace.", nameof(parameter));

		string parameterName = parameter.ParameterName.TrimStart('@');

		return $"@{parameterName}";
	}

	/// <summary>
	/// Sanitizes the schema and name and returns the token to be used in the SQL command
	/// for an object (stored procedure, function, etc.).
	/// </summary>
	/// <param name="schema">The schema of the object.</param>
	/// <param name="name">The name of the object.</param>
	/// <returns>The token for the specified object.</returns>
	private static string GetObjectToken(string schema, string name)
	{
		ArgumentException.ThrowIfNullOrWhiteSpace(schema);
		ArgumentException.ThrowIfNullOrWhiteSpace(name);

		schema = schema.Trim().TrimEnd('.').TrimEnd(']').TrimStart('[');
		name = name.Trim().TrimStart('.').TrimStart('[').TrimEnd(']');

		return $"[{schema}].[{name}]";
	}
}
