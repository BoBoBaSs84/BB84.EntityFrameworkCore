// Copyright: 2024 Robert Peter Meyer
// License: MIT
//
// This source code is licensed under the MIT license found in the
// LICENSE file in the root directory of this source tree.
using BB84.EntityFrameworkCore.TestSupport.Entities;
using BB84.EntityFrameworkCore.TestSupport.Repositories;

namespace BB84.EntityFrameworkCore.Repositories.Agnostic.Tests;

[TestClass]
public sealed class PersonTests : UnitTestBase
{
	[TestMethod]
	public void ExecuteDeleteByIdTest()
	{
		PersonRepository repository = new(DbContext);

		int deleted = repository.ExecuteDelete(Guid.NewGuid());

		Assert.AreEqual(0, deleted);
	}

	[TestMethod]
	public void ExecuteDeleteByIdsTest()
	{
		PersonRepository repository = new(DbContext);

		int deleted = repository.ExecuteDelete([Guid.NewGuid(), Guid.NewGuid()]);

		Assert.AreEqual(0, deleted);
	}

	[TestMethod]
	public async Task ExecuteDeleteByIdAsyncTest()
	{
		PersonRepository repository = new(DbContext);

		int deleted = await repository.ExecuteDeleteAsync(Guid.NewGuid(), TestContext.CancellationToken)
			.ConfigureAwait(false);

		Assert.AreEqual(0, deleted);
	}

	[TestMethod]
	public async Task ExecuteDeleteByIdsAsyncTest()
	{
		PersonRepository repository = new(DbContext);

		int deleted = await repository.ExecuteDeleteAsync([Guid.NewGuid(), Guid.NewGuid()], TestContext.CancellationToken)
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

		PersonEntity? person = await repository.GetByIdAsync(Guid.Empty, cancellationToken: TestContext.CancellationToken)
			.ConfigureAwait(false);

		Assert.IsNull(person);
	}

	[TestMethod]
	public async Task GetByIdsAsyncTest()
	{
		PersonRepository repository = new(DbContext);

		IEnumerable<PersonEntity> persons = await repository.GetByIdsAsync([Guid.NewGuid(), Guid.NewGuid()], cancellationToken: TestContext.CancellationToken)
			.ConfigureAwait(false);

		Assert.IsFalse(persons.Any());
	}

	[TestMethod]
	public void GetAllTest()
	{
		PersonRepository repository = new(DbContext);

		IEnumerable<PersonEntity> persons = repository.GetList(new() { IgnoreQueryFilters = true, TrackChanges = true });

		Assert.IsFalse(persons.Any());
	}

	[TestMethod]
	public async Task GetAllAsyncTest()
	{
		PersonRepository repository = new(DbContext);

		IEnumerable<PersonEntity> persons = await repository.GetListAsync(new() { IgnoreQueryFilters = true, TrackChanges = true }, TestContext.CancellationToken)
			.ConfigureAwait(false);

		Assert.IsFalse(persons.Any());
	}

	[TestMethod]
	public void GetManyByConditionTest()
	{
		PersonRepository repository = new(DbContext);

		IEnumerable<PersonEntity> persons = repository.GetList(new()
		{
			Where = x => x.Id.Equals(Guid.Empty),
			QueryFilter = x => x.Where(x => x.Id.Equals(Guid.Empty)),
			OrderBy = x => x.OrderBy(x => x.Id),
			Skip = 1,
			Take = 1
		});

		Assert.IsFalse(persons.Any());
	}

	[TestMethod]
	public async Task GetManyByConditionAsyncTest()
	{
		PersonRepository repository = new(DbContext);

		IEnumerable<PersonEntity> persons = await repository.GetListAsync(new()
		{
			Where = x => x.Id.Equals(Guid.Empty),
			QueryFilter = x => x.Where(x => x.Id.Equals(Guid.Empty)),
			OrderBy = x => x.OrderBy(x => x.Id),
			Skip = 1,
			Take = 1
		}, TestContext.CancellationToken).ConfigureAwait(false);

		Assert.IsFalse(persons.Any());
	}

	[TestMethod]
	public async Task StreamAllTest()
	{
		PersonRepository repository = new(DbContext);
		int count = 0;

		await foreach (PersonEntity person in repository.Stream(new() { IgnoreQueryFilters = true, TrackChanges = true }, TestContext.CancellationToken).ConfigureAwait(false))
			count++;

		Assert.AreEqual(0, count);
	}

	[TestMethod]
	public async Task StreamByConditionTest()
	{
		PersonRepository repository = new(DbContext);
		int count = 0;

		IAsyncEnumerable<PersonEntity> persons = repository.Stream(new()
		{
			Where = x => x.Id.Equals(Guid.Empty),
			QueryFilter = x => x.Where(x => x.Id.Equals(Guid.Empty)),
			OrderBy = x => x.OrderBy(x => x.Id),
			Skip = 1,
			Take = 1
		}, TestContext.CancellationToken);

		await foreach (PersonEntity person in persons.ConfigureAwait(false))
			count++;

		Assert.AreEqual(0, count);
	}

	public TestContext TestContext { get; set; }
}
