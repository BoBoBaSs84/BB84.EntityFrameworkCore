// Copyright: 2024 Robert Peter Meyer
// License: MIT
//
// This source code is licensed under the MIT license found in the
// LICENSE file in the root directory of this source tree.
using System.Data.Common;
using System.Text.RegularExpressions;

using Microsoft.EntityFrameworkCore.Infrastructure;

namespace BB84.EntityFrameworkCore.Repositories.SqlServer.Extensions;

/// <summary>
/// Represents extension methods for the <see cref="DatabaseFacade"/> class to execute stored
/// procedures, table-valued functions and scalar functions with parameters.
/// </summary>
public static partial class DatabaseFacadeExtensions
{
	/// <summary>
	/// Validates the parameter name and returns the token to be used in the SQL command.
	/// </summary>
	/// <remarks>
	/// The name is written into the command text, so anything but a plain T-SQL variable name
	/// is rejected rather than escaped: there is no quoting syntax for variable names.
	/// </remarks>
	/// <param name="parameter">The parameter to get the token for.</param>
	/// <returns>The token for the specified parameter.</returns>
	/// <exception cref="ArgumentException">
	/// Thrown when the parameter name is null, whitespace or not a valid T-SQL variable name.
	/// </exception>
	private static string GetParameterToken(DbParameter parameter)
	{
		ArgumentNullException.ThrowIfNull(parameter);

		if (string.IsNullOrWhiteSpace(parameter.ParameterName))
			throw new ArgumentException("ParameterName cannot be null or whitespace.", nameof(parameter));

		string parameterName = parameter.ParameterName.TrimStart('@');

		if (!ParameterNameRegex().IsMatch(parameterName))
			throw new ArgumentException($"ParameterName '{parameter.ParameterName}' is not a valid T-SQL variable name.", nameof(parameter));

		return $"@{parameterName}";
	}

	/// <summary>
	/// Sanitizes the schema and name and returns the token to be used in the SQL command
	/// for an object (stored procedure, function, etc.).
	/// </summary>
	/// <remarks>
	/// Both parts are delimited the way <c>QUOTENAME</c> does it: a closing bracket inside a
	/// part is doubled, so the part can never end the delimited identifier early.
	/// </remarks>
	/// <param name="schema">The schema of the object.</param>
	/// <param name="name">The name of the object.</param>
	/// <returns>The token for the specified object.</returns>
	private static string GetObjectToken(string schema, string name)
	{
		ArgumentException.ThrowIfNullOrWhiteSpace(schema);
		ArgumentException.ThrowIfNullOrWhiteSpace(name);

		schema = schema.Trim().TrimEnd('.').TrimEnd(']').TrimStart('[');
		name = name.Trim().TrimStart('.').TrimStart('[').TrimEnd(']');

		return $"[{schema.Replace("]", "]]", StringComparison.Ordinal)}].[{name.Replace("]", "]]", StringComparison.Ordinal)}]";
	}

	/// <summary>
	/// Matches a T-SQL variable name without its leading <c>@</c>.
	/// </summary>
	[GeneratedRegex(@"^[\p{L}_#][\p{L}\p{Nd}_@#$]{0,126}$", RegexOptions.CultureInvariant)]
	private static partial Regex ParameterNameRegex();
}
