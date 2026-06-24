// Copyright: 2024 Robert Peter Meyer
// License: MIT
//
// This source code is licensed under the MIT license found in the
// LICENSE file in the root directory of this source tree.
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BB84.EntityFrameworkCore.Repositories.SqlServer.Extensions;

/// <summary>
/// Provides extension methods for configuring property data types in a relational
/// database using the <see cref="PropertyBuilder"/> class.
/// </summary>
/// <remarks>
/// The <see cref="PropertyBuilderExtensions"/> class provides extension methods that allow
/// developers to specify the SQL data type for a property when using Entity Framework Core
/// to map their domain models to a relational database. Each method configures the column
/// type for the property being built, enabling precise control over the database schema.
/// </remarks>
public static class PropertyBuilderExtensions
{
	/// <summary>
	/// Configures the data type of the column to <c>binary</c> when targeting a relational database.
	/// </summary>
	/// <param name="builder">The builder for the property being configured.</param>
	/// <param name="length">This can be a value from 1 through 8,000. max indicates that the maximum
	/// storage size is <c>2^31-1</c> bytes.</param>
	/// <returns>The same builder instance so that multiple calls can be chained.</returns>
	public static PropertyBuilder IsBinaryColumn(this PropertyBuilder builder, int length = 8000)
		=> length is < 1 or > 8000
			? throw new ArgumentOutOfRangeException(nameof(length), "Must be between 1 and 8000.")
			: builder.HasColumnType($"binary({length})");

	/// <summary>
	/// Configures the data type of the column to <b>date</b> when targeting a relational database.
	/// </summary>
	/// <param name="builder">The builder for the property being configured.</param>
	/// <returns>The same builder instance so that multiple calls can be chained.</returns>
	public static PropertyBuilder IsDateColumn(this PropertyBuilder builder)
		=> builder.HasColumnType("date");

	/// <summary>
	/// Configures the data type of the column to <c>datetime</c> or <c>smalldatetime</c> when targeting a
	/// relational database.
	/// </summary>
	/// <param name="builder">The builder for the property being configured.</param>
	/// <param name="small">Indicates if <b>small date time</b> should be used.</param>
	/// <returns>The same builder instance so that multiple calls can be chained.</returns>
	public static PropertyBuilder IsDateTimeColumn(this PropertyBuilder builder, bool small = false)
		=> builder.HasColumnType(small ? "smalldatetime" : "datetime");

	/// <summary>
	/// Configures the data type of the column to <c>datetime2</c> when targeting a relational database.
	/// </summary>
	/// <param name="builder">The builder for the property being configured.</param>
	/// <param name="precision">The optional type parameter fractional seconds precision specifies the number
	/// of digits for the fractional part of the seconds.</param>
	/// <returns>The same builder instance so that multiple calls can be chained.</returns>
	public static PropertyBuilder IsDateTime2Column(this PropertyBuilder builder, int precision = 7)
		=> precision is < 0 or > 7
			? throw new ArgumentOutOfRangeException(nameof(precision), "Must be between 0 and 7.")
			: builder.HasColumnType($"datetime2({precision})");

	/// <summary>
	/// Configures the data type of the column to <c>datetimeoffset</c> when targeting a relational database.
	/// </summary>
	/// <param name="builder">The builder for the property being configured.</param>
	/// <param name="precision">The optional type parameter fractional seconds precision specifies the number
	/// of digits for the fractional part of the seconds.</param>
	/// <returns>The same builder instance so that multiple calls can be chained.</returns>
	/// <exception cref="ArgumentOutOfRangeException"></exception>
	public static PropertyBuilder IsDateTimeOffsetColumn(this PropertyBuilder builder, int precision = 7)
		=> precision is < 0 or > 7
			? throw new ArgumentOutOfRangeException(nameof(precision), "Must be between 0 and 7.")
			: builder.HasColumnType($"datetimeoffset({precision})");

	/// <summary>
	/// Configures the data type of the column to <c>decimal</c> when targeting a relational database.
	/// </summary>
	/// <param name="builder">The builder for the property being configured.</param>
	/// <param name="precision">The maximum total number of decimal digits to be stored.</param>
	/// <param name="scale">The number of decimal digits that are stored to the right of the decimal point.</param>
	/// <returns>The same builder instance so that multiple calls can be chained.</returns>
	/// <exception cref="ArgumentOutOfRangeException"></exception>
	public static PropertyBuilder IsDecimalColumn(this PropertyBuilder builder, int precision = 18, int scale = 2)
		=> precision is < 1 or > 38
			? throw new ArgumentOutOfRangeException(nameof(precision), "Must be between 1 and 38.")
			: scale < 0 || scale > precision
				? throw new ArgumentOutOfRangeException(nameof(scale), "Must be between 0 and precision.")
				: builder.HasColumnType($"decimal({precision},{scale})");

	/// <summary>
	/// Configures the data type of the column to <b>money</b> when targeting a relational database.
	/// </summary>
	/// <param name="builder">The builder for the property being configured.</param>
	/// <param name="small">Indicates if <b>small money</b> should be used.</param>
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
			? throw new ArgumentOutOfRangeException(nameof(precision), "Must be between 0 and 7.")
			: builder.HasColumnType($"time({precision})");

	/// <summary>
	/// Configures the data type of the column to <c>uniqueidentifier</c> when targeting a relational database.
	/// </summary>
	/// <param name="builder">The builder for the property being configured.</param>
	/// <returns>The same builder instance so that multiple calls can be chained.</returns>
	public static PropertyBuilder IsUniqueIdentifierColumn(this PropertyBuilder builder)
		=> builder.HasColumnType("uniqueidentifier");

	/// <summary>
	/// Configures the data type of the column to <b>varbinary</b> when targeting a relational database.
	/// </summary>
	/// <param name="builder">The builder for the property being configured.</param>
	/// <param name="length">This can be a value from 0 through 8,000. The default value is 0 (max).</param>
	/// <returns>The same builder instance so that multiple calls can be chained.</returns>
	public static PropertyBuilder IsVarbinaryColumn(this PropertyBuilder builder, int length = 0)
		=> length is < 0 or > 8000
			? throw new ArgumentOutOfRangeException(nameof(length), "Must be between 0 and 8000.")
			: builder.HasColumnType(length == 0 ? "varbinary(max)" : $"varbinary({length})");

	/// <summary>
	/// Configures the data type of the column to <b>xml</b> when targeting a relational database.
	/// </summary>
	/// <param name="builder">The builder for the property being configured.</param>
	/// <returns>The same builder instance so that multiple calls can be chained.</returns>
	public static PropertyBuilder IsXmlColumn(this PropertyBuilder builder)
		=> builder.HasColumnType("xml");
}
