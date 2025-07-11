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
	/// <param name="tableName">The name of the main table.</param>
	/// <param name="tableSchema">The schema of the main table.</param>
	/// <param name="historySchema">The schema of the history table.</param>
	/// <returns>
	/// The same <see cref="EntityTypeBuilder"/> instance, so that multiple calls can be chained together.
	/// </returns>
	public static EntityTypeBuilder ToHistoryTable(this EntityTypeBuilder builder, string? tableName = null, string? tableSchema = "dbo", string historySchema = "history")
	{
		tableName ??= builder.Metadata.ClrType.Name;

		return builder.ToTable(tableName, tableSchema, tb
			=> tb.IsTemporal(ttb
				=> ttb.UseHistoryTable(tableName, historySchema)));
	}
}
