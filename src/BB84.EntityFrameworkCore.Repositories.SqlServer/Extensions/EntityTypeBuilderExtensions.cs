using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BB84.EntityFrameworkCore.Repositories.SqlServer.Extensions;

/// <summary>
/// The <see cref="EntityTypeBuilder"/> extensions class.
/// </summary>
public static class EntityTypeBuilderExtensions
{
	/// <summary>
	/// Configures the table that the entity type maps to when targeting a relational database
	/// and configures a history table for the entity mapped to a temporal table.
	/// </summary>
	/// <param name="builder">The builder for the entity type being configured.</param>
	/// <param name="tableName">The schema of the table.</param>
	/// <param name="tableSchema">The name of the table.</param>
	/// <param name="historySchema">The schema of the history table.</param>
	/// <returns>The same builder instance so that multiple calls can be chained.</returns>
	public static EntityTypeBuilder ToHistoryTable(this EntityTypeBuilder builder, string? tableName = null, string? tableSchema = "dbo", string historySchema = "history")
	{
		tableName ??= builder.Metadata.ClrType.Name;

		return builder.ToTable(tableName, tableSchema, tb
			=> tb.IsTemporal(ttb
				=> ttb.UseHistoryTable(tableName, historySchema)));
	}
}
