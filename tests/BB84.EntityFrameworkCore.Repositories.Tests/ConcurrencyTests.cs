// Copyright: 2024 Robert Peter Meyer
// License: MIT
//
// This source code is licensed under the MIT license found in the
// LICENSE file in the root directory of this source tree.
using BB84.EntityFrameworkCore.Repositories.Tests.Persistence;
using BB84.EntityFrameworkCore.TestSupport.Entities;
using BB84.EntityFrameworkCore.TestSupport.Repositories;

using Microsoft.EntityFrameworkCore;

namespace BB84.EntityFrameworkCore.Repositories.Tests;

/// <summary>
/// Covers the disconnected update scenario the concurrency token exists for: an entity is read,
/// mapped to a data transfer object, sent over the wire and posted back.
/// </summary>
/// <remarks>
/// The token was get-only before 5.0, so the original value could not be restored onto the
/// reconstructed entity and the <c>WHERE</c> predicate never guarded anything.
/// </remarks>
[TestClass]
public sealed class ConcurrencyTests : UnitTestBase
{
	[TestMethod]
	public void StaleTimestampRaisesConcurrencyExceptionTest()
	{
		Guid id = Guid.NewGuid();
		string uniqueName = $"Skill-{Guid.NewGuid():N}";

		byte[] staleTimestamp = CreateSkill(id, uniqueName);

		try
		{
			// Another transaction changes the row, which moves the row version on.
			using (TestDbContext other = GetTestContext())
			{
				_ = other.Set<SkillEntity>()
					.Where(x => x.Id == id)
					.ExecuteUpdate(s => s.SetProperty(p => p.Description, "Changed elsewhere."));
			}

			using TestDbContext posting = GetTestContext();
			SkillRepository repository = new(posting);

			SkillEntity detached = new()
			{
				Id = id,
				Name = uniqueName,
				Description = "Posted back.",
				Timestamp = staleTimestamp
			};

			repository.Update(detached);

			_ = Assert.ThrowsExactly<DbUpdateConcurrencyException>(() => posting.SaveChanges());
		}
		finally
		{
			DeleteSkill(id);
		}
	}

	[TestMethod]
	public void CurrentTimestampUpdatesTest()
	{
		Guid id = Guid.NewGuid();
		string uniqueName = $"Skill-{Guid.NewGuid():N}";

		_ = CreateSkill(id, uniqueName);

		try
		{
			SkillEntity read;

			using (TestDbContext reading = GetTestContext())
			{
				SkillEntity? entity = new SkillRepository(reading).GetById(id);

				Assert.IsNotNull(entity);
				read = entity;
			}

			using TestDbContext posting = GetTestContext();
			SkillRepository repository = new(posting);

			// A fresh instance standing in for what comes back over the wire. Every column has
			// to be carried, not just the token: the update marks them all as modified, so a
			// partial entity writes nulls over whatever it left out.
			SkillEntity detached = new()
			{
				Id = read.Id,
				Name = read.Name,
				Description = "Posted back.",
				IsCritical = read.IsCritical,
				Reference = read.Reference,
				CreatedBy = read.CreatedBy,
				CreatedAt = read.CreatedAt,
				Timestamp = read.Timestamp
			};

			repository.Update(detached);

			Assert.AreEqual(1, posting.SaveChanges());

			using TestDbContext verifying = GetTestContext();
			SkillEntity? reloaded = new SkillRepository(verifying).GetById(id);

			Assert.IsNotNull(reloaded);
			Assert.AreEqual("Posted back.", reloaded.Description);
		}
		finally
		{
			DeleteSkill(id);
		}
	}

	[TestMethod]
	public void MissingTimestampRaisesConcurrencyExceptionTest()
	{
		Guid id = Guid.NewGuid();
		string uniqueName = $"Skill-{Guid.NewGuid():N}";

		_ = CreateSkill(id, uniqueName);

		try
		{
			using TestDbContext posting = GetTestContext();
			SkillRepository repository = new(posting);

			// The token is left at its default, which is what a data transfer object that
			// dropped the property on the way out and back produces.
			SkillEntity detached = new()
			{
				Id = id,
				Name = uniqueName,
				Description = "Posted back without a token."
			};

			repository.Update(detached);

			_ = Assert.ThrowsExactly<DbUpdateConcurrencyException>(() => posting.SaveChanges());
		}
		finally
		{
			DeleteSkill(id);
		}
	}

	/// <summary>
	/// Creates a skill and returns the store generated concurrency token it came back with.
	/// </summary>
	private static byte[] CreateSkill(Guid id, string name)
	{
		using TestDbContext dbContext = GetTestContext();

		SkillEntity entity = new()
		{
			Id = id,
			Name = name,
			Description = "Created for the concurrency test.",
			IsCritical = false
		};

		new SkillRepository(dbContext).Create(entity);
		_ = dbContext.SaveChanges();

		Assert.IsNotEmpty(entity.Timestamp, "the store generated token should be materialized onto the entity");

		return entity.Timestamp;
	}

	private static void DeleteSkill(Guid id)
	{
		using TestDbContext dbContext = GetTestContext();

		_ = dbContext.Set<SkillEntity>()
			.Where(x => x.Id == id)
			.ExecuteDelete();
	}
}
