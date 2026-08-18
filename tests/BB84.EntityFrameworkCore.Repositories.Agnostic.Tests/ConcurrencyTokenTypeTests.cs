// Copyright: 2024 Robert Peter Meyer
// License: MIT
//
// This source code is licensed under the MIT license found in the
// LICENSE file in the root directory of this source tree.
using BB84.EntityFrameworkCore.Repositories.Agnostic.Tests.Persistence;
using BB84.EntityFrameworkCore.Repositories.Agnostic.Tests.Persistence.Entities;

using Microsoft.EntityFrameworkCore;

namespace BB84.EntityFrameworkCore.Repositories.Agnostic.Tests;

/// <summary>
/// Covers a concurrency token that is not a row version.
/// </summary>
/// <remarks>
/// The rest of this suite runs on a <see cref="byte"/> array token propped up by
/// <see cref="RowVersionEmulation"/>, which is exactly the scaffolding a real consumer does not
/// get. These tests take the other route the token type parameter opened up: a
/// <see cref="Guid"/> the writer rotates, needing no emulation at all.
/// </remarks>
[TestClass]
public sealed class ConcurrencyTokenTypeTests : UnitTestBase
{
	[TestMethod]
	public void EntityWithNonRowVersionTokenRoundTrips()
	{
		TokenTypeEntity entity = Create("Round tripping");

		using TestDbContext other = CreateContext();
		TokenTypeEntity loaded = other.Set<TokenTypeEntity>().Single(x => x.Id == entity.Id);

		Assert.AreEqual(entity.Timestamp, loaded.Timestamp);
		Assert.AreNotEqual(Guid.Empty, loaded.Timestamp);
	}

	[TestMethod]
	public void TokenIsMarkedAsAConcurrencyToken()
	{
		// The base configuration rung is what applies this, so the assertion is really that the
		// rung accepted a token type other than the one it defaults to.
		Microsoft.EntityFrameworkCore.Metadata.IProperty timestamp = DbContext.Model
			.FindEntityType(typeof(TokenTypeEntity))!
			.FindProperty(nameof(TokenTypeEntity.Timestamp))!;

		Assert.IsTrue(timestamp.IsConcurrencyToken);
		Assert.AreEqual(typeof(Guid), timestamp.ClrType);
	}

	[TestMethod]
	public void StaleTokenRaisesConcurrencyException()
	{
		TokenTypeEntity entity = Create("Contested");

		// A competing writer rotates the token, which the entity tracked here knows nothing about.
		using (TestDbContext other = CreateContext())
		{
			TokenTypeEntity contested = other.Set<TokenTypeEntity>().Single(x => x.Id == entity.Id);

			contested.Name = "Taken.";
			contested.Timestamp = Guid.NewGuid();

			_ = other.SaveChanges();
		}

		entity.Name = "Too late.";
		entity.Timestamp = Guid.NewGuid();

		_ = Assert.ThrowsExactly<DbUpdateConcurrencyException>(() => DbContext.SaveChanges());
	}

	[TestMethod]
	public void CurrentTokenAllowsTheUpdate()
	{
		TokenTypeEntity entity = Create("Uncontested");

		entity.Name = "Edited.";
		entity.Timestamp = Guid.NewGuid();

		Assert.AreEqual(1, DbContext.SaveChanges());
	}

	private TokenTypeEntity Create(string name)
	{
		TokenTypeEntity entity = new() { Name = name, Timestamp = Guid.NewGuid() };

		_ = DbContext.Set<TokenTypeEntity>().Add(entity);
		_ = DbContext.SaveChanges();

		return entity;
	}
}
