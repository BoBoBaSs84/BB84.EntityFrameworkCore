﻿// Copyright: 2024 Robert Peter Meyer
// License: MIT
//
// This source code is licensed under the MIT license found in the
// LICENSE file in the root directory of this source tree.
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
