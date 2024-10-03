using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BB84.EntityFrameworkCore.Repositories.SqlServer.Extensions;

/// <summary>
/// The <see cref="PropertyBuilder"/> extension class.
/// </summary>
public static class PropertyBuilderExtensions
{
	/// <summary>
	/// Configures the data type of the column to <b>date</b> when targeting a relational database.
	/// </summary>
	/// <param name="builder">The builder for the property being configured.</param>
	/// <returns>The same builder instance so that multiple calls can be chained.</returns>
	public static PropertyBuilder HasDateColumnType(this PropertyBuilder builder)
		=> builder.HasColumnType("date");

	/// <summary>
	/// Configures the data type of the column to <b>money</b> when targeting a relational database.
	/// </summary>
	/// <param name="builder">The builder for the property being configured.</param>
	/// <param name="small">Inidactes if <b>small money</b> should be used.</param>
	/// <returns>The same builder instance so that multiple calls can be chained.</returns>
	public static PropertyBuilder HasMoneyColumnType(this PropertyBuilder builder, bool small = false)
		=> builder.HasColumnType(small ? "smallmoney" : "money");

	/// <summary>
	/// Configures the data type of the column to <b>time</b> when targeting a relational database.
	/// </summary>
	/// <param name="builder">The builder for the property being configured.</param>
	/// <param name="precision">This can be an integer from 0 to 7.
	/// The default fractional scale is 7 (100ns).</param>
	/// <returns>The same builder instance so that multiple calls can be chained.</returns>
	public static PropertyBuilder HasTimeColumnType(this PropertyBuilder builder, int precision = 7)
		=> precision is < 0 or > 7
			? throw new ArgumentOutOfRangeException(nameof(precision), "Must be between 0 to 7.")
			: builder.HasColumnType($"time({precision})");

	/// <summary>
	/// Configures the data type of the column to <b>varbinary</b> when targeting a relational database.
	/// </summary>
	/// <param name="builder">The builder for the property being configured.</param>
	/// <param name="precision">This can be an integer from 0 to 4000.
	/// The default fractional scale is 0 (max).</param>
	/// <returns>The same builder instance so that multiple calls can be chained.</returns>
	public static PropertyBuilder HasVarbinaryColumnType(this PropertyBuilder builder, int precision = 0)
		=> precision is < 0 or > 4000
			? throw new ArgumentOutOfRangeException(nameof(precision), "Must be between 0 to 4000.")
			: builder.HasColumnType(precision == 0 ? $"varbinary(max)" : $"varbinary({precision})");

	/// <summary>
	/// Configures the data type of the column to <b>xml</b> when targeting a relational database.
	/// </summary>
	/// <param name="builder">The builder for the property being configured.</param>
	/// <returns>The same builder instance so that multiple calls can be chained.</returns>
	public static PropertyBuilder HasXmlColumnType(this PropertyBuilder builder)
		=> builder.HasColumnType("xml");
}
