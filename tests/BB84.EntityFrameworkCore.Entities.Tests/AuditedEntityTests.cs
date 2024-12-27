using BB84.EntityFrameworkCore.Entities;
using BB84.EntityFrameworkCore.Entities.Abstractions;

namespace BB84.EntityFrameworkCore.EntitiesTests;

[TestClass]
public sealed class AuditedEntityTests
{
	[TestMethod]
	public void AuditedEntityTest()
	{
		IAuditedEntity? model;

		model = new TestClass();

		Assert.IsNotNull(model);
		Assert.AreEqual(Guid.Empty, model.Id);
		Assert.IsNull(model.Timestamp);
		Assert.IsNull(model.Creator);
		Assert.IsNull(model.Editor);
	}

	private sealed class TestClass : AuditedEntity
	{ }
}
