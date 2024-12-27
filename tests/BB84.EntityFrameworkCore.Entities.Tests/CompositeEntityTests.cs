using BB84.EntityFrameworkCore.Entities;
using BB84.EntityFrameworkCore.Entities.Abstractions;

namespace BB84.EntityFrameworkCore.EntitiesTests;

[TestClass]
public sealed class CompositeEntityTests
{
	[TestMethod]
	public void CompositeEntityTest()
	{
		ICompositeEntity model;

		model = new TestClass();

		Assert.IsNotNull(model);
		Assert.IsNull(model.Timestamp);
	}

	private sealed class TestClass : CompositeEntity
	{ }
}
