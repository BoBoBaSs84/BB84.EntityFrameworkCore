﻿using BB84.EntityFrameworkCore.Entities.Abstractions;

namespace BB84.EntityFrameworkCore.Entities.Tests;

[TestClass]
public sealed class FullAuditedEntityTests
{
	[TestMethod]
	public void PropertySetCorrectValuesTest()
	{
		IFullAuditedEntity? entity;

		entity = new TestClass()
		{
			Id = Guid.NewGuid(),
			Creator = "UnitTest",
			Created = DateTime.UtcNow,
			Editor = "UnitTest",
			Edited = DateTime.UtcNow
		};

		Assert.IsNotNull(entity);
		Assert.AreNotEqual(Guid.Empty, entity.Id);
		Assert.AreNotEqual("Test", entity.Creator);
		Assert.AreNotEqual(DateTime.UtcNow, entity.Created);
		Assert.AreNotEqual("Test", entity.Editor);
		Assert.AreNotEqual(DateTime.UtcNow, entity.Edited);
	}

	private sealed class TestClass : FullAuditedEntity
	{ }
}