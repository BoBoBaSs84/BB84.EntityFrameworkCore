using BB84.EntityFrameworkCore.Entities.Abstractions;

namespace BB84.EntityFrameworkCore.Entities.Tests;

[TestClass]
public sealed class CompositeEntityTests
{
	[TestMethod]
	public void CompositeEntityTest()
	{
		ICompositeEntity? entity;

		entity = new TestClass();

		Assert.IsNotNull(entity);
	}

	private sealed class TestClass : CompositeEntity
	{ }
}
