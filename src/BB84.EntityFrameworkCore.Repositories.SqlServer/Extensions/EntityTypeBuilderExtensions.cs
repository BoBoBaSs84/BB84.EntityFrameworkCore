// Copyright: 2024 Robert Peter Meyer
// License: MIT
//
// This source code is licensed under the MIT license found in the
// LICENSE file in the root directory of this source tree.
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BB84.EntityFrameworkCore.Repositories.SqlServer.Extensions;

/// <summary>
/// Provides extension methods for configuring entity types in a relational database context.
/// </summary>
/// <remarks>
/// The <see cref="EntityTypeBuilderExtensions"/> class contains methods to simplify the
/// configuration of entity types, including support for temporal tables and history table
/// mappings.
/// </remarks>
public static class EntityTypeBuilderExtensions
{
	/// <summary>
	/// Configures the entity to use a temporal table with a history table for tracking changes over time.
	/// </summary>
	/// <remarks>
	/// This method configures the entity to use SQL Server temporal tables, which automatically track 
	/// historical changes to the data. The history table is created in the specified schema and is used
	/// to store historical versions of the data.
	/// </remarks>
	/// <param name="builder">The <see cref="EntityTypeBuilder"/> used to configure the entity type.</param>
	/// <param name="tableName">The name of the database table.</param>
	/// <param name="tableSchema">The schema of the database table.</param>
	/// <param name="historyTableName">The name of the database history table.</param>
	/// <param name="historySchema">The schema of the database history table.</param>
	/// <returns>
	/// The same <see cref="EntityTypeBuilder"/> instance, so that multiple calls can be chained together.
	/// </returns>
	/// <exception cref="ArgumentNullException">Thrown if the <paramref name="builder"/> is null.</exception>
	/// <exception cref="ArgumentException">
	/// Thrown if the <paramref name="tableSchema"/> or <paramref name="historySchema"/> is null, empty, or whitespace.
	/// </exception>
	public static EntityTypeBuilder ToHistoryTable(this EntityTypeBuilder builder, string? tableName = null, string tableSchema = "dbo", string? historyTableName = null, string historySchema = "history")
	{
		ArgumentNullException.ThrowIfNull(builder);
		ArgumentException.ThrowIfNullOrWhiteSpace(tableSchema);
		ArgumentException.ThrowIfNullOrWhiteSpace(historySchema);

		tableName ??= builder.Metadata.ClrType.Name;
		historyTableName ??= tableName;

		return builder.ToTable(tableName, tableSchema, tb
			=> tb.IsTemporal(ttb
				=> ttb.UseHistoryTable(historyTableName, historySchema)));
	}
}
