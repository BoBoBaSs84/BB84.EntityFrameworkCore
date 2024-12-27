using BB84.EntityFrameworkCore.Entities;
using BB84.EntityFrameworkCore.Entities.Abstractions;

namespace BB84.EntityFrameworkCore.EntitiesTests;

[TestClass]
public sealed class FullAuditedEntityTests
{
	[TestMethod]
	public void PropertySetCorrectValuesTest()
	{
		IFullAuditedEntity? model;

		model = new TestClass()
		{
			Id = Guid.NewGuid(),
			Creator = "UnitTest",
			Created = DateTime.UtcNow,
			Editor = "UnitTest",
			Edited = DateTime.UtcNow
		};

		Assert.IsNotNull(model);
		Assert.AreNotEqual(Guid.Empty, model.Id);
		Assert.AreNotEqual("Test", model.Creator);
		Assert.AreNotEqual(DateTime.UtcNow, model.Created);
		Assert.AreNotEqual("Test", model.Editor);
		Assert.AreNotEqual(DateTime.UtcNow, model.Edited);
	}

	private sealed class TestClass : FullAuditedEntity
	{ }
}
