using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BB84.EntityFrameworkCore.Repositories.SqlServer.Extensions;

/// <summary>
/// The <see cref="PropertyBuilder"/> extension class.
/// </summary>
public static class PropertyBuilderExtensions
{
	/// <summary>
	/// Configures the data type of the column to <b>binary</b> when targeting a relational database.
	/// </summary>
	/// <param name="builder">The builder for the property being configured.</param>
	/// <param name="precision">This can be an integer from 1 to 8000.
	/// The default fractional scale is 8000.</param>
	/// <returns>The same builder instance so that multiple calls can be chained.</returns>
	public static PropertyBuilder IsBinaryColumn(this PropertyBuilder builder, int precision = 8000)
		=> precision is < 1 or > 8000
			? throw new ArgumentOutOfRangeException(nameof(precision), "Must be between 1 to 8000.")
			: builder.HasColumnType($"binary({precision})");

	/// <summary>
	/// Configures the data type of the column to <b>date</b> when targeting a relational database.
	/// </summary>
	/// <param name="builder">The builder for the property being configured.</param>
	/// <returns>The same builder instance so that multiple calls can be chained.</returns>
	public static PropertyBuilder IsDateColumn(this PropertyBuilder builder)
		=> builder.HasColumnType("date");

	/// <summary>
	/// Configures the data type of the column to <b>date time</b> when targeting a relational database.
	/// </summary>
	/// <param name="builder">The builder for the property being configured.</param>
	/// <param name="small">Inidactes if <b>small date time</b> should be used.</param>
	/// <returns>The same builder instance so that multiple calls can be chained.</returns>
	public static PropertyBuilder IsDateTimeColumn(this PropertyBuilder builder, bool small = false)
		=> builder.HasColumnType(small ? "smalldatetime" : "datetime");

	/// <summary>
	/// Configures the data type of the column to <b>money</b> when targeting a relational database.
	/// </summary>
	/// <param name="builder">The builder for the property being configured.</param>
	/// <param name="small">Inidactes if <b>small money</b> should be used.</param>
	/// <returns>The same builder instance so that multiple calls can be chained.</returns>
	public static PropertyBuilder IsMoneyColumn(this PropertyBuilder builder, bool small = false)
		=> builder.HasColumnType(small ? "smallmoney" : "money");

	/// <summary>
	/// Configures the data type of the column to <b>sysname</b> when targeting a relational database.
	/// </summary>
	/// <param name="builder">The builder for the property being configured.</param>
	/// <returns>The same builder instance so that multiple calls can be chained.</returns>
	public static PropertyBuilder IsSysNameColumn(this PropertyBuilder builder)
		=> builder.HasColumnType("sysname");

	/// <summary>
	/// Configures the data type of the column to <b>time</b> when targeting a relational database.
	/// </summary>
	/// <param name="builder">The builder for the property being configured.</param>
	/// <param name="precision">This can be an integer from 0 to 7.
	/// The default fractional scale is 7 (100ns).</param>
	/// <returns>The same builder instance so that multiple calls can be chained.</returns>
	public static PropertyBuilder IsTimeColumn(this PropertyBuilder builder, int precision = 7)
		=> precision is < 0 or > 7
			? throw new ArgumentOutOfRangeException(nameof(precision), "Must be between 0 to 7.")
			: builder.HasColumnType($"time({precision})");

	/// <summary>
	/// Configures the data type of the column to <b>varbinary</b> when targeting a relational database.
	/// </summary>
	/// <param name="builder">The builder for the property being configured.</param>
	/// <param name="precision">This can be an integer from 0 to 8000.
	/// The default fractional scale is 0 (max).</param>
	/// <returns>The same builder instance so that multiple calls can be chained.</returns>
	public static PropertyBuilder IsVarbinaryColumn(this PropertyBuilder builder, int precision = 0)
		=> precision is < 0 or > 8000
			? throw new ArgumentOutOfRangeException(nameof(precision), "Must be between 0 to 8000.")
			: builder.HasColumnType(precision == 0 ? $"varbinary(max)" : $"varbinary({precision})");

	/// <summary>
	/// Configures the data type of the column to <b>xml</b> when targeting a relational database.
	/// </summary>
	/// <param name="builder">The builder for the property being configured.</param>
	/// <returns>The same builder instance so that multiple calls can be chained.</returns>
	public static PropertyBuilder IsXmlColumn(this PropertyBuilder builder)
		=> builder.HasColumnType("xml");
}
