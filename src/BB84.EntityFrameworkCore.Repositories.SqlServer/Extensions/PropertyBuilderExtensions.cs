using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BB84.EntityFrameworkCore.Repositories.SqlServer.Extensions;

/// <summary>
/// The <see cref="PropertyBuilder"/>
/// </summary>
public static class PropertyBuilderExtensions
{
	/// <summary>
	/// Configures the data type of the column to <b>date</b> when targeting a relational database.
	/// </summary>
	/// <param name="builder">The builder for the property being configured.</param>
	/// <returns>The same builder instance so that multiple calls can be chained.</returns>
	public static PropertyBuilder IsDateType(this PropertyBuilder builder)
		=> builder.HasColumnType("date");

	/// <summary>
	/// Configures the data type of the column to <b>money</b> when targeting a relational database.
	/// </summary>
	/// <param name="builder">The builder for the property being configured.</param>
	/// <returns>The same builder instance so that multiple calls can be chained.</returns>
	public static PropertyBuilder IsMoneyType(this PropertyBuilder builder)
		=> builder.HasColumnType("money");

	/// <summary>
	/// Configures the data type of the column to <b>money</b> when targeting a relational database.
	/// </summary>
	/// <param name="builder">The builder for the property being configured.</param>
	/// <returns>The same builder instance so that multiple calls can be chained.</returns>
	public static PropertyBuilder IsSmallMoneyType(this PropertyBuilder builder)
		=> builder.HasColumnType("smallmoney");

	/// <summary>
	/// Configures the data type of the column to <b>time</b> when targeting a relational database.
	/// </summary>
	/// <param name="builder">The builder for the property being configured.</param>
	/// <param name="precision">This can be an integer from 0 to 7.
	/// The default fractional scale is 7 (100ns).</param>
	/// <returns>The same builder instance so that multiple calls can be chained.</returns>
	public static PropertyBuilder IsTimeType(this PropertyBuilder builder, int precision = 7)
		=> precision is < 0 or > 7
			? throw new ArgumentOutOfRangeException(nameof(precision), "Must be between 0 to 7.")
			: builder.HasColumnType($"time({precision})");

	/// <summary>
	/// Configures the data type of the column to <b>xml</b> when targeting a relational database.
	/// </summary>
	/// <param name="builder">The builder for the property being configured.</param>
	/// <returns>The same builder instance so that multiple calls can be chained.</returns>
	public static PropertyBuilder IsXmlType(this PropertyBuilder builder)
		=> builder.HasColumnType("xml");
}
