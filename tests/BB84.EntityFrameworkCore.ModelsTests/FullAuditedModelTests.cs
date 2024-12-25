using BB84.EntityFrameworkCore.Models;
using BB84.EntityFrameworkCore.Models.Abstractions;

namespace BB84.EntityFrameworkCore.ModelsTests;

[TestClass]
public sealed class FullAuditedModelTests
{
	[TestMethod]
	public void PropertySetCorrectValuesTest()
	{
		IFullAuditedModel? model;

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

	private sealed class TestClass : FullAuditedModel
	{ }
}
