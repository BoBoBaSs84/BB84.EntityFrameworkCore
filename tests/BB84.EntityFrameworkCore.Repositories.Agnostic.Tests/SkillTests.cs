// Copyright: 2024 Robert Peter Meyer
// License: MIT
//
// This source code is licensed under the MIT license found in the
// LICENSE file in the root directory of this source tree.
using BB84.EntityFrameworkCore.TestSupport.Entities;
using BB84.EntityFrameworkCore.TestSupport.Repositories;

namespace BB84.EntityFrameworkCore.Repositories.Agnostic.Tests;

[TestClass]
public sealed class SkillTests : UnitTestBase
{
	[TestMethod]
	public void SkillCreateTest()
	{
		SkillRepository repository = new(DbContext);
		SkillEntity newSkill = new()
		{
			Name = "FancySkill",
			Description = "This is a fancy skill",
			IsCritical = true
		};

		repository.Create(newSkill);
		int result = DbContext.SaveChanges();
		Assert.AreEqual(1, result);

		SkillEntity? dbSkill = repository.GetSingle(new() { Where = x => x.Name == newSkill.Name, TrackChanges = true });
		Assert.IsNotNull(dbSkill);
		Assert.AreNotEqual(DateTimeOffset.MinValue, dbSkill.CreatedAt);

		dbSkill.IsCritical = false;
		result = DbContext.SaveChanges();
		Assert.AreEqual(1, result);

		dbSkill = repository.GetSingle(new() { Where = x => x.Name == newSkill.Name, TrackChanges = true });
		Assert.IsNotNull(dbSkill);
		Assert.AreNotEqual(DateTimeOffset.MinValue, dbSkill.EditedAt);
	}

	[TestMethod]
	public void SkillDisconnectedUpdateKeepsCreationAuditTest()
	{
		SkillRepository repository = new(DbContext);
		SkillEntity newSkill = new()
		{
			Name = "DisconnectedSkill",
			Description = "Created before the disconnected update"
		};

		repository.Create(newSkill);
		_ = DbContext.SaveChanges();
		string createdBy = newSkill.CreatedBy;
		DateTimeOffset createdAt = newSkill.CreatedAt;
		DbContext.ChangeTracker.Clear();

		// Rebuilt from a data transfer object that does not carry the creation audit columns.
		SkillEntity detached = new()
		{
			Id = newSkill.Id,
			Name = newSkill.Name,
			Description = "Posted back.",
			Timestamp = newSkill.Timestamp
		};

		repository.Update(detached);
		Assert.AreEqual(1, DbContext.SaveChanges());
		DbContext.ChangeTracker.Clear();

		SkillEntity? dbSkill = repository.GetById(newSkill.Id);
		Assert.IsNotNull(dbSkill);
		Assert.AreEqual("Posted back.", dbSkill.Description);
		Assert.AreEqual(createdBy, dbSkill.CreatedBy);
		Assert.AreEqual(createdAt, dbSkill.CreatedAt);
	}
}
