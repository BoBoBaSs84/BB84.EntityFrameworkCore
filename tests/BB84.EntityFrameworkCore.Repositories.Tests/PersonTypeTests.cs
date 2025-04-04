// Copyright: 2024 Robert Peter Meyer
// License: MIT
//
// This source code is licensed under the MIT license found in the
// LICENSE file in the root directory of this source tree.
using BB84.EntityFrameworkCore.Repositories.Tests.Persistence;
using BB84.EntityFrameworkCore.Repositories.Tests.Persistence.Entities;
using BB84.EntityFrameworkCore.Repositories.Tests.Persistence.Repositories;

namespace BB84.EntityFrameworkCore.Repositories.Tests;

[TestClass]
public sealed class PersonTypeTests : UnitTestBase
{
	[TestMethod]
	public void GetByNameTest()
	{
		using TestDbContext dbContext = GetTestContext();
		PersonTypeRepository repository = new(dbContext);

		PersonTypeEntity? result = repository.GetByName("Male");

		Assert.IsNotNull(result);
	}

	[TestMethod]
	public void GetByNamesTest()
	{
		using TestDbContext dbContext = GetTestContext();
		PersonTypeRepository repository = new(dbContext);

		IEnumerable<PersonTypeEntity> result = repository.GetByNames(["Male", "Female"]);

		Assert.IsNotNull(result);
		Assert.AreEqual(2, result.Count());
	}

	[TestMethod]
	public async Task GetByNameAsyncTest()
	{
		using TestDbContext dbContext = GetTestContext();
		PersonTypeRepository repository = new(dbContext);

		PersonTypeEntity? result = await repository.GetByNameAsync("Male")
			.ConfigureAwait(false);

		Assert.IsNotNull(result);
	}

	[TestMethod]
	public async Task GetByNamesAsyncTest()
	{
		using TestDbContext dbContext = GetTestContext();
		PersonTypeRepository repository = new(dbContext);

		IEnumerable<PersonTypeEntity> result = await repository.GetByNamesAsync(["Male", "Female"])
			.ConfigureAwait(false);

		Assert.IsNotNull(result);
		Assert.AreEqual(2, result.Count());
	}

	[TestMethod]
	public void GetByConditionTest()
	{
		using TestDbContext dbContext = GetTestContext();
		PersonRepository repository = new(dbContext);

		PersonEntity? person = repository.GetByCondition(
			expression: x => x.Id.Equals(Guid.Empty),
			includeProperties: [nameof(PersonEntity.Type)]
			);

		Assert.IsNull(person);
	}

	[TestMethod]
	public async Task GetByConditionAsyncTest()
	{
		using TestDbContext dbContext = GetTestContext();
		PersonRepository repository = new(dbContext);

		PersonEntity? person = await repository.GetByConditionAsync(
			expression: x => x.Id.Equals(Guid.Empty),
			includeProperties: [nameof(PersonEntity.Type)]
			).ConfigureAwait(false);

		Assert.IsNull(person);
	}

	[TestMethod]
	public void SoftDeleteTest()
	{
		using TestDbContext dbContext = GetTestContext();
		PersonTypeRepository repository = new(dbContext);

		PersonTypeEntity? result = repository.GetById(1, false, true);
		Assert.IsNotNull(result);

		repository.Delete(result);
		_ = dbContext.SaveChanges();
		Assert.IsTrue(result.IsDeleted);
	}

	[TestMethod]
	public async Task SoftDeleteAsyncTest()
	{
		using TestDbContext dbContext = GetTestContext();
		PersonTypeRepository repository = new(dbContext);

		PersonTypeEntity? result = await repository.GetByIdAsync(2, false, true);
		Assert.IsNotNull(result);

		await repository.DeleteAsync(result);
		_ = await dbContext.SaveChangesAsync();
		Assert.IsTrue(result.IsDeleted);
	}
}
