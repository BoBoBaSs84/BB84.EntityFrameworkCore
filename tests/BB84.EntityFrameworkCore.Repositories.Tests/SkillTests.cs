// Copyright: 2024 Robert Peter Meyer
// License: MIT
//
// This source code is licensed under the MIT license found in the
// LICENSE file in the root directory of this source tree.
using BB84.EntityFrameworkCore.Repositories.Tests.Persistence;
using BB84.EntityFrameworkCore.Repositories.Tests.Persistence.Entities;
using BB84.EntityFrameworkCore.Repositories.Tests.Persistence.Repositories;

namespace BB84.EntityFrameworkCore.Repositories.Tests;

[TestClass]
public sealed class SkillTests : UnitTestBase
{
	[TestMethod]
	public void SkillCreateTest()
	{
		using TestDbContext context = GetTestContext();
		SkillRepository repository = new(context);
		SkillEntity newSkill = new()
		{
			Name = "FancySkill",
			Description = "This is a fancy skill",
			IsCritical = true
		};

		repository.Create(newSkill);
		int result = context.SaveChanges();
		Assert.AreEqual(1, result);

		SkillEntity? dbSkill = repository.GetByCondition(x => x.Name == newSkill.Name, trackChanges: true);
		Assert.IsNotNull(dbSkill);
		Assert.AreNotEqual(DateTime.MinValue, dbSkill.CreatedAt);

		dbSkill.IsCritical = false;
		result = context.SaveChanges();
		Assert.AreEqual(1, result);

		dbSkill = repository.GetByCondition(x => x.Name == newSkill.Name, trackChanges: true);
		Assert.IsNotNull(dbSkill);
		Assert.AreNotEqual(DateTime.MinValue, dbSkill.EditedAt);
	}
}
