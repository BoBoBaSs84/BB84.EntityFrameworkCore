using BB84.EntityFrameworkCore.Entities;
using BB84.EntityFrameworkCore.Entities.Abstractions;

namespace BB84.EntityFrameworkCore.EntitiesTests;

[TestClass]
public sealed class AuditedCompositeEntityTests
{
	[TestMethod]
	public void AuditedCompositeEntityTest()
	{
		IAuditedCompositeEntity model;

		model = new TestClass();

		Assert.IsNotNull(model);
		Assert.IsNull(model.Timestamp);
		Assert.IsNull(model.Creator);
		Assert.IsNull(model.Editor);
	}

	private sealed class TestClass : AuditedCompositeEntity
	{ }
}
