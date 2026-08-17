// Copyright: 2024 Robert Peter Meyer
// License: MIT
//
// This source code is licensed under the MIT license found in the
// LICENSE file in the root directory of this source tree.
using BB84.EntityFrameworkCore.Repositories.SqlServer.Extensions;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BB84.EntityFrameworkCore.Repositories.Tests;

/// <summary>
/// Covers <see cref="EntityTypeBuilderExtensions.ToHistoryTable{TEntity}"/> and its non-generic
/// counterpart.
/// </summary>
/// <remarks>
/// These build a model in memory and never open a connection, so they do not derive from
/// <see cref="UnitTestBase"/>. Both overloads are covered: the non-generic one is reachable only
/// through <c>modelBuilder.Entity(Type)</c> and had no coverage at all.
/// </remarks>
[TestClass]
public sealed class EntityTypeBuilderExtensionsTests
{
	[TestMethod]
	public void GenericOverloadShouldFallBackToTheEntityName()
	{
		IReadOnlyEntityType entityType = ConfigureGeneric(builder => builder.ToHistoryTable());

		Assert.AreEqual("Probe", entityType.GetTableName());
		Assert.AreEqual("dbo", entityType.GetSchema());
		Assert.AreEqual("Probe", entityType.GetHistoryTableName());
		Assert.AreEqual("history", entityType.GetHistoryTableSchema());
		Assert.IsTrue(entityType.IsTemporal());
	}

	[TestMethod]
	public void GenericOverloadShouldHonourEveryArgument()
	{
		IReadOnlyEntityType entityType = ConfigureGeneric(
			builder => builder.ToHistoryTable("Jobs", "tab", "OldJobs", "hist"));

		Assert.AreEqual("Jobs", entityType.GetTableName());
		Assert.AreEqual("tab", entityType.GetSchema());
		Assert.AreEqual("OldJobs", entityType.GetHistoryTableName());
		Assert.AreEqual("hist", entityType.GetHistoryTableSchema());
	}

	[TestMethod]
	public void GenericOverloadShouldFallBackToTheTableNameForTheHistoryTable()
	{
		IReadOnlyEntityType entityType = ConfigureGeneric(builder => builder.ToHistoryTable("Jobs"));

		Assert.AreEqual("Jobs", entityType.GetHistoryTableName(), "the history table should default to the table name");
	}

	[TestMethod]
	public void GenericOverloadShouldReturnTheSameBuilder()
	{
		EntityTypeBuilder<Probe> builder = new ModelBuilder().Entity<Probe>();

		Assert.AreSame(builder, builder.ToHistoryTable(), "the documented contract is that calls can be chained");
	}

	[TestMethod]
	public void NonGenericOverloadShouldConfigureTheTemporalTable()
	{
		EntityTypeBuilder builder = new ModelBuilder().Entity<Probe>();

		Assert.AreSame(builder, builder.ToHistoryTable("Jobs", "tab", "OldJobs", "hist"));
		Assert.AreEqual("Jobs", builder.Metadata.GetTableName());
		Assert.AreEqual("tab", builder.Metadata.GetSchema());
		Assert.AreEqual("OldJobs", builder.Metadata.GetHistoryTableName());
		Assert.AreEqual("hist", builder.Metadata.GetHistoryTableSchema());
		Assert.IsTrue(builder.Metadata.IsTemporal());
	}

	[TestMethod]
	public void GenericOverloadShouldRejectANullBuilder()
	{
		EntityTypeBuilder<Probe> builder = null!;

		ArgumentNullException exception = Assert.ThrowsExactly<ArgumentNullException>(() => builder.ToHistoryTable());

		Assert.AreEqual("builder", exception.ParamName);
	}

	[TestMethod]
	public void NonGenericOverloadShouldRejectANullBuilder()
	{
		EntityTypeBuilder builder = null!;

		ArgumentNullException exception = Assert.ThrowsExactly<ArgumentNullException>(() => builder.ToHistoryTable());

		Assert.AreEqual("builder", exception.ParamName);
	}

	[TestMethod]
	[DataRow(null)]
	[DataRow("")]
	[DataRow("   ")]
	public void ShouldRejectABlankTableSchema(string? tableSchema)
	{
		EntityTypeBuilder<Probe> builder = new ModelBuilder().Entity<Probe>();

		// A null argument surfaces as ArgumentNullException, which derives from ArgumentException.
		ArgumentException exception = Assert.Throws<ArgumentException>(
			() => builder.ToHistoryTable(tableSchema: tableSchema!));

		Assert.AreEqual("tableSchema", exception.ParamName);
	}

	[TestMethod]
	[DataRow(null)]
	[DataRow("")]
	[DataRow("   ")]
	public void ShouldRejectABlankHistoryTableSchema(string? historyTableSchema)
	{
		EntityTypeBuilder<Probe> builder = new ModelBuilder().Entity<Probe>();

		ArgumentException exception = Assert.Throws<ArgumentException>(
			() => builder.ToHistoryTable(historyTableSchema: historyTableSchema!));

		Assert.AreEqual("historyTableSchema", exception.ParamName);
	}

	private static IReadOnlyEntityType ConfigureGeneric(Action<EntityTypeBuilder<Probe>> configure)
	{
		EntityTypeBuilder<Probe> builder = new ModelBuilder().Entity<Probe>();

		configure(builder);

		return builder.Metadata;
	}

	private sealed class Probe
	{
		public Guid Id { get; set; }
		public string Name { get; set; } = string.Empty;
	}
}
