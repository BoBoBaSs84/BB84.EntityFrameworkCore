// Copyright: 2024 Robert Peter Meyer
// License: MIT
//
// This source code is licensed under the MIT license found in the
// LICENSE file in the root directory of this source tree.
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BB84.EntityFrameworkCore.Repositories.SqlServer.Extensions;

/// <summary>
/// Provides extension methods for configuring SQL Server column types using the
/// <see cref="PropertyBuilder"/> and <see cref="PropertyBuilder{TProperty}"/> classes.
/// </summary>
/// <remarks>
/// <para>
/// The <see cref="PropertyBuilderExtensions"/> class provides extension methods that allow
/// developers to specify the SQL Server data type for a property when using Entity Framework
/// Core to map their domain models to the database. Each method configures the column type for
/// the property being built, enabling precise control over the database schema.
/// </para>
/// <para>
/// Every method comes in two forms. The generic form extends
/// <see cref="PropertyBuilder{TProperty}"/> and returns it unchanged, so the property type
/// survives the call and anything typed further along the chain — <c>HasDefaultValue</c>,
/// <c>HasConversion</c>, <c>HasValueGenerator</c> — stays compile-time checked. The non-generic
/// form exists for the <see cref="EntityTypeBuilder.Property(string)"/> path used by shadow
/// properties, where no property type is available.
/// </para>
/// </remarks>
public static class PropertyBuilderExtensions
{
	/// <summary>
	/// Configures the data type of the column to <c>binary</c>.
	/// </summary>
	/// <param name="builder">The builder for the property being configured.</param>
	/// <param name="length">This can be a value from 1 through 8,000.</param>
	/// <returns>The same builder instance so that multiple calls can be chained.</returns>
	/// <exception cref="ArgumentOutOfRangeException">
	/// <paramref name="length"/> is less than 1 or greater than 8,000.
	/// </exception>
	public static PropertyBuilder IsBinaryColumn(this PropertyBuilder builder, int length = 8000)
		=> builder.HasColumnType(BinaryColumnType(length));

	/// <inheritdoc cref="IsBinaryColumn(PropertyBuilder, int)"/>
	/// <typeparam name="TProperty">The type of the property being configured.</typeparam>
	public static PropertyBuilder<TProperty> IsBinaryColumn<TProperty>(this PropertyBuilder<TProperty> builder, int length = 8000)
		=> builder.HasColumnType(BinaryColumnType(length));

	/// <summary>
	/// Configures the data type of the column to <c>date</c>.
	/// </summary>
	/// <param name="builder">The builder for the property being configured.</param>
	/// <returns>The same builder instance so that multiple calls can be chained.</returns>
	public static PropertyBuilder IsDateColumn(this PropertyBuilder builder)
		=> builder.HasColumnType("date");

	/// <inheritdoc cref="IsDateColumn(PropertyBuilder)"/>
	/// <typeparam name="TProperty">The type of the property being configured.</typeparam>
	public static PropertyBuilder<TProperty> IsDateColumn<TProperty>(this PropertyBuilder<TProperty> builder)
		=> builder.HasColumnType("date");

	/// <summary>
	/// Configures the data type of the column to <c>datetime</c> or <c>smalldatetime</c>.
	/// </summary>
	/// <param name="builder">The builder for the property being configured.</param>
	/// <param name="small">Indicates if <b>small date time</b> should be used.</param>
	/// <returns>The same builder instance so that multiple calls can be chained.</returns>
	public static PropertyBuilder IsDateTimeColumn(this PropertyBuilder builder, bool small = false)
		=> builder.HasColumnType(small ? "smalldatetime" : "datetime");

	/// <inheritdoc cref="IsDateTimeColumn(PropertyBuilder, bool)"/>
	/// <typeparam name="TProperty">The type of the property being configured.</typeparam>
	public static PropertyBuilder<TProperty> IsDateTimeColumn<TProperty>(this PropertyBuilder<TProperty> builder, bool small = false)
		=> builder.HasColumnType(small ? "smalldatetime" : "datetime");

	/// <summary>
	/// Configures the data type of the column to <c>datetime2</c>.
	/// </summary>
	/// <param name="builder">The builder for the property being configured.</param>
	/// <param name="precision">The optional type parameter fractional seconds precision specifies the number
	/// of digits for the fractional part of the seconds.</param>
	/// <returns>The same builder instance so that multiple calls can be chained.</returns>
	/// <exception cref="ArgumentOutOfRangeException">
	/// <paramref name="precision"/> is less than 0 or greater than 7.
	/// </exception>
	public static PropertyBuilder IsDateTime2Column(this PropertyBuilder builder, int precision = 7)
		=> builder.HasColumnType(DateTime2ColumnType(precision));

	/// <inheritdoc cref="IsDateTime2Column(PropertyBuilder, int)"/>
	/// <typeparam name="TProperty">The type of the property being configured.</typeparam>
	public static PropertyBuilder<TProperty> IsDateTime2Column<TProperty>(this PropertyBuilder<TProperty> builder, int precision = 7)
		=> builder.HasColumnType(DateTime2ColumnType(precision));

	/// <summary>
	/// Configures the data type of the column to <c>datetimeoffset</c>.
	/// </summary>
	/// <param name="builder">The builder for the property being configured.</param>
	/// <param name="precision">The optional type parameter fractional seconds precision specifies the number
	/// of digits for the fractional part of the seconds.</param>
	/// <returns>The same builder instance so that multiple calls can be chained.</returns>
	/// <exception cref="ArgumentOutOfRangeException">
	/// <paramref name="precision"/> is less than 0 or greater than 7.
	/// </exception>
	public static PropertyBuilder IsDateTimeOffsetColumn(this PropertyBuilder builder, int precision = 7)
		=> builder.HasColumnType(DateTimeOffsetColumnType(precision));

	/// <inheritdoc cref="IsDateTimeOffsetColumn(PropertyBuilder, int)"/>
	/// <typeparam name="TProperty">The type of the property being configured.</typeparam>
	public static PropertyBuilder<TProperty> IsDateTimeOffsetColumn<TProperty>(this PropertyBuilder<TProperty> builder, int precision = 7)
		=> builder.HasColumnType(DateTimeOffsetColumnType(precision));

	/// <summary>
	/// Configures the data type of the column to <c>decimal</c>.
	/// </summary>
	/// <param name="builder">The builder for the property being configured.</param>
	/// <param name="precision">The maximum total number of decimal digits to be stored.</param>
	/// <param name="scale">The number of decimal digits that are stored to the right of the decimal point.</param>
	/// <returns>The same builder instance so that multiple calls can be chained.</returns>
	/// <exception cref="ArgumentOutOfRangeException">
	/// <paramref name="precision"/> is less than 1 or greater than 38, or <paramref name="scale"/>
	/// is less than 0 or greater than <paramref name="precision"/>.
	/// </exception>
	public static PropertyBuilder IsDecimalColumn(this PropertyBuilder builder, int precision = 18, int scale = 2)
		=> builder.HasColumnType(DecimalColumnType(precision, scale));

	/// <inheritdoc cref="IsDecimalColumn(PropertyBuilder, int, int)"/>
	/// <typeparam name="TProperty">The type of the property being configured.</typeparam>
	public static PropertyBuilder<TProperty> IsDecimalColumn<TProperty>(this PropertyBuilder<TProperty> builder, int precision = 18, int scale = 2)
		=> builder.HasColumnType(DecimalColumnType(precision, scale));

	/// <summary>
	/// Configures the data type of the column to <c>money</c> or <c>smallmoney</c>.
	/// </summary>
	/// <param name="builder">The builder for the property being configured.</param>
	/// <param name="small">Indicates if <b>small money</b> should be used.</param>
	/// <returns>The same builder instance so that multiple calls can be chained.</returns>
	public static PropertyBuilder IsMoneyColumn(this PropertyBuilder builder, bool small = false)
		=> builder.HasColumnType(small ? "smallmoney" : "money");

	/// <inheritdoc cref="IsMoneyColumn(PropertyBuilder, bool)"/>
	/// <typeparam name="TProperty">The type of the property being configured.</typeparam>
	public static PropertyBuilder<TProperty> IsMoneyColumn<TProperty>(this PropertyBuilder<TProperty> builder, bool small = false)
		=> builder.HasColumnType(small ? "smallmoney" : "money");

	/// <summary>
	/// Configures the data type of the column to <c>sysname</c>.
	/// </summary>
	/// <param name="builder">The builder for the property being configured.</param>
	/// <returns>The same builder instance so that multiple calls can be chained.</returns>
	public static PropertyBuilder IsSysNameColumn(this PropertyBuilder builder)
		=> builder.HasColumnType("sysname");

	/// <inheritdoc cref="IsSysNameColumn(PropertyBuilder)"/>
	/// <typeparam name="TProperty">The type of the property being configured.</typeparam>
	public static PropertyBuilder<TProperty> IsSysNameColumn<TProperty>(this PropertyBuilder<TProperty> builder)
		=> builder.HasColumnType("sysname");

	/// <summary>
	/// Configures the data type of the column to <c>time</c>.
	/// </summary>
	/// <param name="builder">The builder for the property being configured.</param>
	/// <param name="precision">This can be an integer from 0 to 7.
	/// The default fractional scale is 7 (100ns).</param>
	/// <returns>The same builder instance so that multiple calls can be chained.</returns>
	/// <exception cref="ArgumentOutOfRangeException">
	/// <paramref name="precision"/> is less than 0 or greater than 7.
	/// </exception>
	public static PropertyBuilder IsTimeColumn(this PropertyBuilder builder, int precision = 7)
		=> builder.HasColumnType(TimeColumnType(precision));

	/// <inheritdoc cref="IsTimeColumn(PropertyBuilder, int)"/>
	/// <typeparam name="TProperty">The type of the property being configured.</typeparam>
	public static PropertyBuilder<TProperty> IsTimeColumn<TProperty>(this PropertyBuilder<TProperty> builder, int precision = 7)
		=> builder.HasColumnType(TimeColumnType(precision));

	/// <summary>
	/// Configures the data type of the column to <c>uniqueidentifier</c>.
	/// </summary>
	/// <param name="builder">The builder for the property being configured.</param>
	/// <returns>The same builder instance so that multiple calls can be chained.</returns>
	public static PropertyBuilder IsUniqueIdentifierColumn(this PropertyBuilder builder)
		=> builder.HasColumnType("uniqueidentifier");

	/// <inheritdoc cref="IsUniqueIdentifierColumn(PropertyBuilder)"/>
	/// <typeparam name="TProperty">The type of the property being configured.</typeparam>
	public static PropertyBuilder<TProperty> IsUniqueIdentifierColumn<TProperty>(this PropertyBuilder<TProperty> builder)
		=> builder.HasColumnType("uniqueidentifier");

	/// <summary>
	/// Configures the data type of the column to <c>varbinary</c>.
	/// </summary>
	/// <param name="builder">The builder for the property being configured.</param>
	/// <param name="length">This can be a value from 1 through 8,000, or 0 (the default) for
	/// <c>varbinary(max)</c>, which stores up to <c>2^31-1</c> bytes. Note that <c>binary</c>
	/// has no <c>max</c> form, which is why <see cref="IsBinaryColumn(PropertyBuilder, int)"/>
	/// has no such sentinel.</param>
	/// <returns>The same builder instance so that multiple calls can be chained.</returns>
	/// <exception cref="ArgumentOutOfRangeException">
	/// <paramref name="length"/> is less than 0 or greater than 8,000.
	/// </exception>
	public static PropertyBuilder IsVarbinaryColumn(this PropertyBuilder builder, int length = 0)
		=> builder.HasColumnType(VarbinaryColumnType(length));

	/// <inheritdoc cref="IsVarbinaryColumn(PropertyBuilder, int)"/>
	/// <typeparam name="TProperty">The type of the property being configured.</typeparam>
	public static PropertyBuilder<TProperty> IsVarbinaryColumn<TProperty>(this PropertyBuilder<TProperty> builder, int length = 0)
		=> builder.HasColumnType(VarbinaryColumnType(length));

	/// <summary>
	/// Configures the data type of the column to <c>xml</c>.
	/// </summary>
	/// <param name="builder">The builder for the property being configured.</param>
	/// <returns>The same builder instance so that multiple calls can be chained.</returns>
	public static PropertyBuilder IsXmlColumn(this PropertyBuilder builder)
		=> builder.HasColumnType("xml");

	/// <inheritdoc cref="IsXmlColumn(PropertyBuilder)"/>
	/// <typeparam name="TProperty">The type of the property being configured.</typeparam>
	public static PropertyBuilder<TProperty> IsXmlColumn<TProperty>(this PropertyBuilder<TProperty> builder)
		=> builder.HasColumnType("xml");

	private static string BinaryColumnType(int length)
		=> length is < 1 or > 8000
			? throw new ArgumentOutOfRangeException(nameof(length), "Must be between 1 and 8000.")
			: $"binary({length})";

	private static string DateTime2ColumnType(int precision)
		=> precision is < 0 or > 7
			? throw new ArgumentOutOfRangeException(nameof(precision), "Must be between 0 and 7.")
			: $"datetime2({precision})";

	private static string DateTimeOffsetColumnType(int precision)
		=> precision is < 0 or > 7
			? throw new ArgumentOutOfRangeException(nameof(precision), "Must be between 0 and 7.")
			: $"datetimeoffset({precision})";

	private static string DecimalColumnType(int precision, int scale)
		=> precision is < 1 or > 38
			? throw new ArgumentOutOfRangeException(nameof(precision), "Must be between 1 and 38.")
			: scale < 0 || scale > precision
				? throw new ArgumentOutOfRangeException(nameof(scale), "Must be between 0 and precision.")
				: $"decimal({precision},{scale})";

	private static string TimeColumnType(int precision)
		=> precision is < 0 or > 7
			? throw new ArgumentOutOfRangeException(nameof(precision), "Must be between 0 and 7.")
			: $"time({precision})";

	private static string VarbinaryColumnType(int length)
		=> length is < 0 or > 8000
			? throw new ArgumentOutOfRangeException(nameof(length), "Must be between 0 and 8000.")
			: length == 0 ? "varbinary(max)" : $"varbinary({length})";
}
