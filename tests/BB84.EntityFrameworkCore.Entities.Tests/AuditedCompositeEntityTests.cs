// Copyright: 2024 Robert Peter Meyer
// License: MIT
//
// This source code is licensed under the MIT license found in the
// LICENSE file in the root directory of this source tree.
using BB84.EntityFrameworkCore.Entities.Abstractions;

namespace BB84.EntityFrameworkCore.Entities.Tests;

[TestClass]
public sealed class AuditedCompositeEntityTests
{
	[TestMethod]
	public void AuditedCompositeEntityTest()
	{
		IAuditedCompositeEntity? entity;
		string creator = "UnitTest";
		string editor = "UnitTest";

		entity = new TestClass()
		{
			CreatedBy = creator,
			EditedBy = editor,
		};

		Assert.IsNotNull(entity);
		Assert.AreEqual(creator, entity.CreatedBy);
		Assert.AreEqual(editor, entity.EditedBy);
	}

	private sealed class TestClass : AuditedCompositeEntity
	{ }
}
