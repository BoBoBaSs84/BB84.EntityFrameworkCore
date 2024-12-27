using BB84.EntityFrameworkCore.Entities;
using BB84.EntityFrameworkCore.Entities.Abstractions;

namespace BB84.EntityFrameworkCore.EntitiesTests;

[TestClass]
public sealed class IdentityEntityTests
{
	[TestMethod]
	public void IdentityEntityTest()
	{
		IIdentityEntity model;

		model = new TestClass();

		Assert.IsNotNull(model);
		Assert.AreEqual(Guid.Empty, model.Id);
		Assert.IsNull(model.Timestamp);
	}

	private sealed class TestClass : IdentityEntity
	{ }
}
