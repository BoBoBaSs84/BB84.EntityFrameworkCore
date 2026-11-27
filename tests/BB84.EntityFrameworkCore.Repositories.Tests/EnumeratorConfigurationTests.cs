// Copyright: 2024 Robert Peter Meyer
// License: MIT
//
// This source code is licensed under the MIT license found in the
// LICENSE file in the root directory of this source tree.
using BB84.EntityFrameworkCore.TestSupport.Entities;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace BB84.EntityFrameworkCore.Repositories.Tests;

[TestClass]
public sealed class EnumeratorConfigurationTests : UnitTestBase
{
	[TestMethod]
	public void NameIndexIsFilteredToNotDeletedRowsTest()
	{
		IEntityType entityType = DbContext.Model.FindEntityType(typeof(PersonTypeEntity))!;

		IIndex index = entityType.GetIndexes().Single(i => i.Properties.Single().Name == nameof(PersonTypeEntity.Name));

		Assert.IsTrue(index.IsUnique);
		Assert.AreEqual("[IsDeleted] = 0", index.GetFilter());
	}

	[TestMethod]
	public void SoftDeletedNameCanBeCreatedAgainTest()
	{
		string name = $"Type-{Guid.NewGuid():N}"[..32];

		PersonTypeEntity original = new() { Name = name };
		_ = DbContext.Set<PersonTypeEntity>().Add(original);
		_ = DbContext.SaveChanges();

		_ = DbContext.Set<PersonTypeEntity>().Remove(original);
		_ = DbContext.SaveChanges();

		PersonTypeEntity recreated = new() { Name = name };
		_ = DbContext.Set<PersonTypeEntity>().Add(recreated);
		_ = DbContext.SaveChanges();

		int rows = DbContext.Set<PersonTypeEntity>()
			.IgnoreQueryFilters()
			.Count(e => e.Name == name);

		Assert.AreEqual(2, rows);
	}
}
