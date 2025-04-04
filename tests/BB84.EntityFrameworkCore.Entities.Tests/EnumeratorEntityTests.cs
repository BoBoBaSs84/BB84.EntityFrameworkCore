// Copyright: 2024 Robert Peter Meyer
// License: MIT
//
// This source code is licensed under the MIT license found in the
// LICENSE file in the root directory of this source tree.
using BB84.EntityFrameworkCore.Entities.Abstractions;

namespace BB84.EntityFrameworkCore.Entities.Tests;

[TestClass]
public sealed class EnumeratorEntityTests
{
	[TestMethod]
	public void EnumeratorEntityTest()
	{
		IEnumeratorEntity? entity;
		int id = int.MaxValue;
		string name = "Name";
		string description = "Description";
		bool isDeleted = true;

		entity = new TestClass()
		{
			Id = id,
			Name = name,
			Description = description,
			IsDeleted = isDeleted
		};

		Assert.IsNotNull(entity);
		Assert.AreEqual(id, entity.Id);
		Assert.AreEqual(name, entity.Name);
		Assert.AreEqual(description, entity.Description);
		Assert.AreEqual(isDeleted, entity.IsDeleted);
	}

	private sealed class TestClass : EnumeratorEntity
	{ }
}
