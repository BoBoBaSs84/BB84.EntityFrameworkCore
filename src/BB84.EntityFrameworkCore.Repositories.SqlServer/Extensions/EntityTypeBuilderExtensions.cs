using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BB84.EntityFrameworkCore.Repositories.SqlServer.Extensions;

/// <summary>
/// The entity type builder extensions class.
/// </summary>
public static class EntityTypeBuilderExtensions
{
	/// <summary>
	/// Configures the table that the entity type maps to when targeting a relational database and configures a history table for the entity mapped to a temporal table.
	/// </summary>
	/// <param name="builder">The entity type builder to work with.</param>
	/// <param name="tableName">The schema of the table.</param>
	/// <param name="tableSchema">The name of the table.</param>
	/// <param name="historySchema">The schema of the history table.</param>
	/// <returns></returns>
	public static EntityTypeBuilder ToHistoryTable(this EntityTypeBuilder builder, string? tableName = null, string? tableSchema = null, string historySchema = "history")
	{
		tableName ??= builder.Metadata.ClrType.Name;

		return builder.ToTable(tableName, tableSchema,
			buildAction => buildAction.IsTemporal(
				buildAction => buildAction.UseHistoryTable(tableName, historySchema)
				)
			);
	}
}
