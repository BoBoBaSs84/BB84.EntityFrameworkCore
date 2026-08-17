// Copyright: 2024 Robert Peter Meyer
// License: MIT
//
// This source code is licensed under the MIT license found in the
// LICENSE file in the root directory of this source tree.
using BB84.EntityFrameworkCore.Repositories.SqlServer.Extensions;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BB84.EntityFrameworkCore.Repositories.Tests;

/// <summary>
/// Verifies the column types produced by <see cref="PropertyBuilderExtensions"/>.
/// </summary>
/// <remarks>
/// These tests build a model in memory and never touch the database, so they do not derive from
/// <see cref="UnitTestBase"/>. Each test calls the extension through
/// <see cref="PropertyBuilder{TProperty}"/>, which is what the generic overloads exist for and
/// what overload resolution picks for a lambda based <c>Property</c> call.
/// </remarks>
[TestClass]
public sealed class PropertyBuilderExtensionsTests
{
	[TestMethod]
	public void IsBinaryColumnShouldUseTheGivenLength()
		=> AssertColumnType("binary(16)", b => b.Property(e => e.Blob).IsBinaryColumn(16));

	[TestMethod]
	public void IsBinaryColumnShouldDefaultToTheMaximumLength()
		=> AssertColumnType("binary(8000)", b => b.Property(e => e.Blob).IsBinaryColumn());

	[TestMethod]
	[DataRow(0)]
	[DataRow(8001)]
	public void IsBinaryColumnShouldThrowOnAnOutOfRangeLength(int length)
		=> AssertOutOfRange("length", b => b.Property(e => e.Blob).IsBinaryColumn(length));

	[TestMethod]
	public void IsDateColumnShouldUseDate()
		=> AssertColumnType("date", b => b.Property(e => e.Date).IsDateColumn());

	[TestMethod]
	[DataRow(false, "datetime")]
	[DataRow(true, "smalldatetime")]
	public void IsDateTimeColumnShouldHonourTheSmallFlag(bool small, string expected)
		=> AssertColumnType(expected, b => b.Property(e => e.Date).IsDateTimeColumn(small));

	[TestMethod]
	public void IsDateTime2ColumnShouldUseTheGivenPrecision()
		=> AssertColumnType("datetime2(3)", b => b.Property(e => e.Date).IsDateTime2Column(3));

	[TestMethod]
	[DataRow(-1)]
	[DataRow(8)]
	public void IsDateTime2ColumnShouldThrowOnAnOutOfRangePrecision(int precision)
		=> AssertOutOfRange("precision", b => b.Property(e => e.Date).IsDateTime2Column(precision));

	[TestMethod]
	public void IsDateTimeOffsetColumnShouldUseTheGivenPrecision()
		=> AssertColumnType("datetimeoffset(0)", b => b.Property(e => e.Offset).IsDateTimeOffsetColumn(0));

	[TestMethod]
	[DataRow(-1)]
	[DataRow(8)]
	public void IsDateTimeOffsetColumnShouldThrowOnAnOutOfRangePrecision(int precision)
		=> AssertOutOfRange("precision", b => b.Property(e => e.Offset).IsDateTimeOffsetColumn(precision));

	[TestMethod]
	public void IsDecimalColumnShouldUseTheGivenPrecisionAndScale()
		=> AssertColumnType("decimal(10,4)", b => b.Property(e => e.Amount).IsDecimalColumn(10, 4));

	[TestMethod]
	[DataRow(0, 2, "precision")]
	[DataRow(39, 2, "precision")]
	[DataRow(10, -1, "scale")]
	[DataRow(10, 11, "scale")]
	public void IsDecimalColumnShouldThrowOnAnOutOfRangeArgument(int precision, int scale, string parameterName)
		=> AssertOutOfRange(parameterName, b => b.Property(e => e.Amount).IsDecimalColumn(precision, scale));

	[TestMethod]
	[DataRow(false, "money")]
	[DataRow(true, "smallmoney")]
	public void IsMoneyColumnShouldHonourTheSmallFlag(bool small, string expected)
		=> AssertColumnType(expected, b => b.Property(e => e.Amount).IsMoneyColumn(small));

	[TestMethod]
	public void IsSysNameColumnShouldUseSysName()
		=> AssertColumnType("sysname", b => b.Property(e => e.Name).IsSysNameColumn());

	[TestMethod]
	public void IsTimeColumnShouldUseTheGivenPrecision()
		=> AssertColumnType("time(7)", b => b.Property(e => e.Duration).IsTimeColumn());

	[TestMethod]
	[DataRow(-1)]
	[DataRow(8)]
	public void IsTimeColumnShouldThrowOnAnOutOfRangePrecision(int precision)
		=> AssertOutOfRange("precision", b => b.Property(e => e.Duration).IsTimeColumn(precision));

	[TestMethod]
	public void IsUniqueIdentifierColumnShouldUseUniqueIdentifier()
		=> AssertColumnType("uniqueidentifier", b => b.Property(e => e.Id).IsUniqueIdentifierColumn());

	[TestMethod]
	public void IsVarbinaryColumnShouldDefaultToMax()
		=> AssertColumnType("varbinary(max)", b => b.Property(e => e.Blob).IsVarbinaryColumn());

	[TestMethod]
	public void IsVarbinaryColumnShouldUseTheGivenLength()
		=> AssertColumnType("varbinary(512)", b => b.Property(e => e.Blob).IsVarbinaryColumn(512));

	[TestMethod]
	[DataRow(-1)]
	[DataRow(8001)]
	public void IsVarbinaryColumnShouldThrowOnAnOutOfRangeLength(int length)
		=> AssertOutOfRange("length", b => b.Property(e => e.Blob).IsVarbinaryColumn(length));

	[TestMethod]
	public void IsXmlColumnShouldUseXml()
		=> AssertColumnType("xml", b => b.Property(e => e.Name).IsXmlColumn());

	/// <summary>
	/// The generic overloads must return <see cref="PropertyBuilder{TProperty}"/> so that a call
	/// placed in the middle of a chain does not erase the property type for what follows.
	/// </summary>
	/// <remarks>
	/// This is a compile-time assertion: before the generic overloads existed the extension bound
	/// through the non-generic base class and the subsequent <c>HasDefaultValue</c> took
	/// <see cref="object"/> instead of the property type.
	/// </remarks>
	[TestMethod]
	public void GenericOverloadsShouldPreserveThePropertyType()
	{
		EntityTypeBuilder<Probe> builder = new ModelBuilder().Entity<Probe>();

		PropertyBuilder<decimal> amount = builder.Property(e => e.Amount)
			.IsDecimalColumn(10, 4)
			.HasDefaultValue(1.5m);

		PropertyBuilder<string> name = builder.Property(e => e.Name)
			.IsSysNameColumn()
			.HasDefaultValue("dbo");

		Assert.AreEqual("decimal(10,4)", amount.Metadata.GetColumnType());
		Assert.AreEqual("sysname", name.Metadata.GetColumnType());
	}

	/// <summary>
	/// The non-generic overloads must survive, because the shadow property path has no property
	/// type to bind to.
	/// </summary>
	[TestMethod]
	public void NonGenericOverloadsShouldStillApplyToShadowProperties()
	{
		EntityTypeBuilder<Probe> builder = new ModelBuilder().Entity<Probe>();

		PropertyBuilder shadow = builder.Property<byte[]>("Shadow")
			.IsVarbinaryColumn(64);

		Assert.AreEqual("varbinary(64)", shadow.Metadata.GetColumnType());
	}

	private static void AssertColumnType(string expected, Func<EntityTypeBuilder<Probe>, PropertyBuilder> configure)
	{
		PropertyBuilder builder = configure(new ModelBuilder().Entity<Probe>());

		Assert.AreEqual(expected, builder.Metadata.GetColumnType());
	}

	private static void AssertOutOfRange(string parameterName, Func<EntityTypeBuilder<Probe>, PropertyBuilder> configure)
	{
		EntityTypeBuilder<Probe> builder = new ModelBuilder().Entity<Probe>();

		ArgumentOutOfRangeException exception = Assert.ThrowsExactly<ArgumentOutOfRangeException>(() => configure(builder));

		Assert.AreEqual(parameterName, exception.ParamName);
	}

	private sealed class Probe
	{
		public Guid Id { get; set; }
		public string Name { get; set; } = string.Empty;
		public decimal Amount { get; set; }
		public DateTime Date { get; set; }
		public DateTimeOffset Offset { get; set; }
		public TimeSpan Duration { get; set; }
		public byte[] Blob { get; set; } = [];
	}
}
