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
public sealed class PersonTests : UnitTestBase
{
	[TestMethod]
	public void DeleteByIdTest()
	{
		PersonRepository repository = new(DbContext);

		int deleted = repository.Delete(Guid.NewGuid());

		Assert.AreEqual(0, deleted);
	}

	[TestMethod]
	public void DeleteByIdsTest()
	{
		PersonRepository repository = new(DbContext);

		int deleted = repository.Delete([Guid.NewGuid(), Guid.NewGuid()]);

		Assert.AreEqual(0, deleted);
	}

	[TestMethod]
	public async Task DeleteByIdAsyncTest()
	{
		PersonRepository repository = new(DbContext);

		int deleted = await repository.DeleteAsync(Guid.NewGuid())
			.ConfigureAwait(false);

		Assert.AreEqual(0, deleted);
	}

	[TestMethod]
	public async Task DeleteByIdsAsyncTest()
	{
		PersonRepository repository = new(DbContext);

		int deleted = await repository.DeleteAsync([Guid.NewGuid(), Guid.NewGuid()])
			.ConfigureAwait(false);

		Assert.AreEqual(0, deleted);
	}

	[TestMethod]
	public void GetByIdTest()
	{
		PersonRepository repository = new(DbContext);

		PersonEntity? person = repository.GetById(Guid.Empty);

		Assert.IsNull(person);
	}

	[TestMethod]
	public void GetByIdsTest()
	{
		PersonRepository repository = new(DbContext);

		IEnumerable<PersonEntity> persons = repository.GetByIds([Guid.NewGuid(), Guid.NewGuid()]);

		Assert.IsFalse(persons.Any());
	}

	[TestMethod]
	public async Task GetByIdAsyncTest()
	{
		PersonRepository repository = new(DbContext);

		PersonEntity? person = await repository.GetByIdAsync(Guid.Empty)
			.ConfigureAwait(false);

		Assert.IsNull(person);
	}

	[TestMethod]
	public async Task GetByIdsAsyncTest()
	{
		PersonRepository repository = new(DbContext);

		IEnumerable<PersonEntity> persons = await repository.GetByIdsAsync([Guid.NewGuid(), Guid.NewGuid()])
			.ConfigureAwait(false);

		Assert.IsFalse(persons.Any());
	}

	[TestMethod]
	public void GetAllTest()
	{
		PersonRepository repository = new(DbContext);

		IEnumerable<PersonEntity> persons = repository.GetAll(true, true);

		Assert.IsFalse(persons.Any());
	}

	[TestMethod]
	public async Task GetAllAsyncTest()
	{
		PersonRepository repository = new(DbContext);

		IEnumerable<PersonEntity> persons = await repository.GetAllAsync(true, true)
			.ConfigureAwait(false);

		Assert.IsFalse(persons.Any());
	}

	[TestMethod]
	public void GetManyByConditionTest()
	{
		PersonRepository repository = new(DbContext);

		IEnumerable<PersonEntity> persons = repository.GetManyByCondition(
			x => x.Id.Equals(Guid.Empty),
			x => x.Where(x => x.Id.Equals(Guid.Empty)),
			false, x => x.OrderBy(x => x.Id), 1, 1, false
			);

		Assert.IsFalse(persons.Any());
	}

	[TestMethod]
	public async Task GetManyByConditionAsyncTest()
	{
		PersonRepository repository = new(DbContext);

		IEnumerable<PersonEntity> persons = await repository.GetManyByConditionAsync(
			x => x.Id.Equals(Guid.Empty),
			x => x.Where(x => x.Id.Equals(Guid.Empty)),
			false, x => x.OrderBy(x => x.Id), 1, 1, false
			).ConfigureAwait(false);

		Assert.IsFalse(persons.Any());
	}
}
