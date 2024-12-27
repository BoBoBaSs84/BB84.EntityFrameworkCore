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
		Assert.AreNotEqual(DateTime.MinValue, dbSkill.Created);

		dbSkill.IsCritical = false;
		result = context.SaveChanges();
		Assert.AreEqual(1, result);

		dbSkill = repository.GetByCondition(x => x.Name == newSkill.Name, trackChanges: true);
		Assert.IsNotNull(dbSkill);
		Assert.AreNotEqual(DateTime.MinValue, dbSkill.Edited);
	}
}
