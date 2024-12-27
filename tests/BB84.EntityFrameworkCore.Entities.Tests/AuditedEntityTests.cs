using BB84.EntityFrameworkCore.Entities.Abstractions;

namespace BB84.EntityFrameworkCore.Entities.Tests;

[TestClass]
public sealed class AuditedEntityTests
{
	[TestMethod]
	public void AuditedEntityTest()
	{
		IAuditedEntity? entity;
		string creator = "UnitTest";
		string editor = "UnitTest";

		entity = new TestClass
		{
			Creator = creator,
			Editor = editor
		};

		Assert.IsNotNull(entity);
		Assert.AreEqual(Guid.Empty, entity.Id);
		Assert.AreEqual(creator, entity.Creator);
		Assert.AreEqual(editor, entity.Editor);
	}

	private sealed class TestClass : AuditedEntity
	{ }
}
