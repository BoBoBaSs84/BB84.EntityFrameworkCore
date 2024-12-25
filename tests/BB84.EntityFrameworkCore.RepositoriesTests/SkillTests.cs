using BB84.EntityFrameworkCore.RepositoriesTests.Persistence.Models;
using BB84.EntityFrameworkCore.RepositoriesTests.Persistence.Repositories;

namespace BB84.EntityFrameworkCore.RepositoriesTests;

[TestClass]
public sealed class SkillTests : UnitTestBase
{
	[TestMethod]
	public void SkillCreateTest()
	{
		using Persistence.TestDbContext context = GetTestContext();
		SkillRepository repository = new(context);
		Skill newSkill = new()
		{
			Name = "FancySkill",
			Description = "This is a fancy skill",
			IsCritical = true
		};

		repository.Create(newSkill);
		int result = context.SaveChanges();
		Assert.AreEqual(1, result);

		Skill? dbSkill = repository.GetByCondition(x => x.Name == newSkill.Name, trackChanges: true);
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
