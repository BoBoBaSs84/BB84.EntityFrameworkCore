// Copyright: 2024 Robert Peter Meyer
// License: MIT
//
// This source code is licensed under the MIT license found in the
// LICENSE file in the root directory of this source tree.
using BB84.EntityFrameworkCore.Entities.Abstractions;

namespace BB84.EntityFrameworkCore.Entities.Tests;

[TestClass]
public sealed class IdentityEntityTests
{
	[TestMethod]
	public void IdentityEntityTest()
	{
		IIdentityEntity? entity;
		Guid id = Guid.NewGuid();

		entity = new TestClass()
		{
			Id = id
		};

		Assert.IsNotNull(entity);
		Assert.AreEqual(id, entity.Id);
		Assert.IsNull(entity.Timestamp);
	}

	private sealed class TestClass : IdentityEntity
	{ }
}
