// Copyright: 2024 Robert Peter Meyer
// License: MIT
//
// This source code is licensed under the MIT license found in the
// LICENSE file in the root directory of this source tree.
using BB84.EntityFrameworkCore.Repositories.Agnostic.Tests.Persistence;
using BB84.EntityFrameworkCore.TestSupport.Entities;

using Microsoft.EntityFrameworkCore;

namespace BB84.EntityFrameworkCore.Repositories.Agnostic.Tests;

/// <summary>
/// Covers the fixture's own row-version emulation.
/// </summary>
/// <remarks>
/// This tests the harness rather than the library, and it earns its place: every other test in
/// this suite performs at most one update per entity, so a broken emulation — one that leaves a
/// stale token on the tracked entity — would pass all of them and only surface much later as an
/// inexplicable concurrency failure.
/// </remarks>
[TestClass]
public sealed class RowVersionEmulationTests : UnitTestBase
{
	[TestMethod]
	public void TokenIsGeneratedOnInsert()
	{
		SkillEntity entity = Create("Generated");

		Assert.IsNotEmpty(entity.Timestamp);
		Assert.HasCount(8, entity.Timestamp, "the emulation stands in for an eight byte rowversion");
	}

	[TestMethod]
	public void TokenRotatesOnUpdateAndStaysInStepWithTheEntity()
	{
		SkillEntity entity = Create("Rotating");
		byte[] afterInsert = entity.Timestamp;

		entity.Description = "Edited once.";
		_ = DbContext.SaveChanges();

		Assert.IsFalse(afterInsert.SequenceEqual(entity.Timestamp), "the trigger should have rotated the token");

		// The real point: the rotated token must have been read back, or this second save fails.
		entity.Description = "Edited twice.";

		Assert.AreEqual(1, DbContext.SaveChanges());
	}

	[TestMethod]
	public void StaleTokenRaisesConcurrencyException()
	{
		SkillEntity entity = Create("Contested");

		// Stand in for a competing writer. The trigger rotates the token, and the entity tracked
		// by this context knows nothing about it.
		using (TestDbContext other = CreateContext())
		{
			_ = other.Set<SkillEntity>()
				.Where(x => x.Id == entity.Id)
				.ExecuteUpdate(setters => setters.SetProperty(x => x.Description, "Taken."));
		}

		entity.Description = "Too late.";

		_ = Assert.ThrowsExactly<DbUpdateConcurrencyException>(() => DbContext.SaveChanges());
	}

	[TestMethod]
	public void SeededRowsCarryAToken()
	{
		// HasData supplies no Timestamp, so the seed rows depend on the column default alone.
		PersonTypeEntity seeded = DbContext.Set<PersonTypeEntity>().Single(x => x.Name == "Male");

		Assert.IsNotEmpty(seeded.Timestamp);
	}

	private SkillEntity Create(string name)
	{
		SkillEntity entity = new() { Name = name, Description = "Created for the emulation tests." };

		_ = DbContext.Set<SkillEntity>().Add(entity);
		_ = DbContext.SaveChanges();

		return entity;
	}
}
