// Copyright: 2024 Robert Peter Meyer
// License: MIT
//
// This source code is licensed under the MIT license found in the
// LICENSE file in the root directory of this source tree.
using BB84.EntityFrameworkCore.Entities.Abstractions.Components;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace BB84.EntityFrameworkCore.Repositories.Agnostic.Tests.Persistence;

/// <summary>
/// Gives SQLite something that behaves like a SQL Server <c>rowversion</c>.
/// </summary>
/// <remarks>
/// <para>
/// Only <see cref="byte"/> array tokens are emulated. Since the token type became a parameter of
/// <see cref="IConcurrency{TToken}"/>, an entity is free to use one this class knows nothing about
/// — <c>randomblob(8)</c> would be the wrong value and the rotating trigger the wrong mechanism —
/// so anything else is left to its own configuration.
/// </para>
/// <para>
/// The agnostic configurations mark <see cref="IConcurrency.Timestamp"/> as a concurrency token
/// that is generated on add or update. On SQL Server the <see cref="byte"/> array plus that
/// combination is convention-mapped to <c>rowversion</c> and the store fills it in. SQLite has no
/// equivalent, so without help the column is omitted from every <c>INSERT</c> and the
/// <c>NOT NULL</c> constraint fails.
/// </para>
/// <para>
/// Two pieces are needed. A column default supplies the token on insert. An <c>AFTER UPDATE</c>
/// trigger rotates it afterwards, because SQLite triggers cannot assign to <c>NEW</c> and so
/// cannot do it in place.
/// </para>
/// <para>
/// The third piece is subtle and the reason this class exists at all:
/// <see cref="SqliteTableBuilderExtensions.UseSqlReturningClause(TableBuilder, bool)"/> has to be
/// turned <b>off</b>. SQLite computes a <c>RETURNING</c> clause before <c>AFTER</c> triggers run,
/// so EF would read the pre-rotation token back onto the tracked entity and the next update of
/// that entity would fail its concurrency check for no reason. With the clause off, EF issues a
/// separate <c>SELECT</c> after the statement — and therefore after the trigger — and the entity
/// stays in step with the store.
/// </para>
/// </remarks>
internal static class RowVersionEmulation
{
	private const string DefaultValueSql = "randomblob(8)";

	/// <summary>
	/// Applies the column default and disables the <c>RETURNING</c> clause for every entity type
	/// that carries a concurrency token.
	/// </summary>
	/// <remarks>
	/// Driven off the model rather than a hardcoded list, so an entity added later cannot quietly
	/// miss it.
	/// </remarks>
	/// <param name="modelBuilder">The builder for the model being configured.</param>
	internal static void ApplyToModel(ModelBuilder modelBuilder)
	{
		foreach (IMutableEntityType entityType in modelBuilder.Model.GetEntityTypes().ToList())
		{
			IMutableProperty? timestamp = entityType.FindProperty(nameof(IConcurrency.Timestamp));

			if (timestamp is null || !timestamp.IsConcurrencyToken || timestamp.ClrType != typeof(byte[]))
				continue;

			timestamp.SetDefaultValueSql(DefaultValueSql);

			_ = modelBuilder.Entity(entityType.ClrType)
				.ToTable(table => table.UseSqlReturningClause(false));
		}
	}

	/// <summary>
	/// Creates the token rotating triggers. Must run after the schema exists.
	/// </summary>
	/// <remarks>
	/// Keyless entity types are skipped: EF never issues an <c>UPDATE</c> for one, so the trigger
	/// would be dead weight.
	/// </remarks>
	/// <param name="context">The context whose database the triggers are created in.</param>
	internal static void CreateTriggers(DbContext context)
	{
		foreach (IEntityType entityType in context.Model.GetEntityTypes())
		{
			if (entityType.FindPrimaryKey() is null)
				continue;

			IProperty? timestamp = entityType.FindProperty(nameof(IConcurrency.Timestamp));

			if (timestamp is null || !timestamp.IsConcurrencyToken || timestamp.ClrType != typeof(byte[]))
				continue;

			string table = entityType.GetTableName()!;
			string column = timestamp.GetColumnName(StoreObjectIdentifier.Table(table))!;

			// ExecuteSqlRaw, not ExecuteSql: the interpolated values are identifiers, and an
			// identifier cannot be a bind variable. ExecuteSql would parameterise them and SQLite
			// would report "no such table: main.@p2". Both values come from the EF model, not
			// from any caller, so there is nothing here to inject.
			//
			// The WHEN guard keeps this correct whether or not recursive triggers are enabled,
			// and leaves an update that sets the token deliberately alone.
#pragma warning disable EF1002 // Risk of vulnerability to SQL injection.
			_ = context.Database.ExecuteSqlRaw($"""
				CREATE TRIGGER IF NOT EXISTS "TR_{table}_{column}"
				AFTER UPDATE ON "{table}"
				FOR EACH ROW
				WHEN NEW."{column}" = OLD."{column}"
				BEGIN
					UPDATE "{table}" SET "{column}" = {DefaultValueSql} WHERE "rowid" = NEW."rowid";
				END;
				""");
#pragma warning restore EF1002
		}
	}
}
