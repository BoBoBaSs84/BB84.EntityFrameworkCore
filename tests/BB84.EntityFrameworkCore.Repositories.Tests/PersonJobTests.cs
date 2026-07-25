// Copyright: 2024 Robert Peter Meyer
// License: MIT
//
// This source code is licensed under the MIT license found in the
// LICENSE file in the root directory of this source tree.
using BB84.EntityFrameworkCore.Repositories.Tests.Persistence;
using BB84.EntityFrameworkCore.Repositories.Tests.Persistence.Entities;
using BB84.EntityFrameworkCore.Repositories.Tests.Persistence.Repositories;

using Microsoft.EntityFrameworkCore;

namespace BB84.EntityFrameworkCore.Repositories.Tests;

[TestClass]
public sealed class PersonJobTests : UnitTestBase
{
	[TestMethod]
	public void CreateTest()
	{
		PersonJobRepository repository = new(DbContext);

		PersonJobEntity personJob = new();

		repository.Create(personJob);
	}

	[TestMethod]
	public void CreateRangeTest()
	{
		PersonJobRepository repository = new(DbContext);

		List<PersonJobEntity> personJobs = [new(), new()];

		repository.Create(personJobs);
	}

	[TestMethod]
	public async Task CreateAsyncTest()
	{
		PersonJobRepository repository = new(DbContext);

		PersonJobEntity personJob = new();

		await repository.CreateAsync(personJob)
			.ConfigureAwait(false);
	}

	[TestMethod]
	public async Task CreateRangeAsyncTest()
	{
		PersonJobRepository repository = new(DbContext);

		List<PersonJobEntity> personJobs = [new(), new()];

		await repository.CreateAsync(personJobs)
			.ConfigureAwait(false);
	}

	[TestMethod]
	public void CountAllTest()
	{
		PersonJobRepository repository = new(DbContext);

		int count = repository.CountAll(false);

		Assert.AreEqual(0, count);
	}

	[TestMethod]
	public void CountByConditionTest()
	{
		PersonJobRepository repository = new(DbContext);

		int count = repository.CountByCondition(x => x.PersonId.Equals(Guid.Empty));

		Assert.AreEqual(0, count);
	}

	[TestMethod]
	public async Task CountAllAsyncTest()
	{
		PersonJobRepository repository = new(DbContext);

		int count = await repository.CountAllAsync(false)
			.ConfigureAwait(false);

		Assert.AreEqual(0, count);
	}

	[TestMethod]
	public async Task CountByConditionAsyncTest()
	{
		PersonJobRepository repository = new(DbContext);

		int count = await repository.CountByConditionAsync(expression: x => x.PersonId.Equals(Guid.Empty))
			.ConfigureAwait(false);

		Assert.AreEqual(0, count);
	}

	[TestMethod]
	public void DeleteTest()
	{
		PersonJobRepository repository = new(DbContext);

		PersonJobEntity personJob = new() { PersonId = Guid.NewGuid(), JobId = Guid.NewGuid() };

		repository.Delete(personJob);
	}

	[TestMethod]
	public void DeleteRangeTest()
	{
		PersonJobRepository repository = new(DbContext);

		List<PersonJobEntity> personJobs = [new() { PersonId = Guid.NewGuid(), JobId = Guid.NewGuid() }];

		repository.Delete(personJobs);
	}

	[TestMethod]
	public async Task DeleteAsyncTest()
	{
		PersonJobRepository repository = new(DbContext);

		PersonJobEntity personJob = new() { PersonId = Guid.NewGuid(), JobId = Guid.NewGuid() };

		await repository.DeleteAsync(personJob)
			.ConfigureAwait(false);

		Assert.AreEqual(EntityState.Deleted, DbContext.Entry(personJob).State);
	}

	[TestMethod]
	public async Task DeleteRangeAsyncTest()
	{
		PersonJobRepository repository = new(DbContext);

		List<PersonJobEntity> personJobs = [new() { PersonId = Guid.NewGuid(), JobId = Guid.NewGuid() }];

		await repository.DeleteAsync(personJobs)
			.ConfigureAwait(false);

		Assert.AreEqual(EntityState.Deleted, DbContext.Entry(personJobs[0]).State);
	}

	[TestMethod]
	public void UpdateTest()
	{
		PersonJobRepository repository = new(DbContext);

		PersonJobEntity personJob = new() { PersonId = Guid.NewGuid(), JobId = Guid.NewGuid() };

		repository.Update(personJob);
	}

	[TestMethod]
	public void UpdateRangeTest()
	{
		PersonJobRepository repository = new(DbContext);

		List<PersonJobEntity> personJobs = [new() { PersonId = Guid.NewGuid(), JobId = Guid.NewGuid() }];

		repository.Update(personJobs);
	}

	[TestMethod]
	public async Task UpdateAsyncTest()
	{
		PersonJobRepository repository = new(DbContext);

		PersonJobEntity personJob = new() { PersonId = Guid.NewGuid(), JobId = Guid.NewGuid() };

		await repository.UpdateAsync(personJob)
			.ConfigureAwait(false);

		Assert.AreEqual(EntityState.Modified, DbContext.Entry(personJob).State);
	}

	[TestMethod]
	public async Task UpdateRangeAsyncTest()
	{
		PersonJobRepository repository = new(DbContext);

		List<PersonJobEntity> personJobs = [new() { PersonId = Guid.NewGuid(), JobId = Guid.NewGuid() }];

		await repository.UpdateAsync(personJobs)
			.ConfigureAwait(false);

		Assert.AreEqual(EntityState.Modified, DbContext.Entry(personJobs[0]).State);
	}

	[TestMethod]
	public async Task DeleteAsyncCancelledTest()
	{
		PersonJobRepository repository = new(DbContext);

		PersonJobEntity personJob = new() { PersonId = Guid.NewGuid(), JobId = Guid.NewGuid() };

		_ = await Assert.ThrowsExactlyAsync<TaskCanceledException>(
			() => repository.DeleteAsync(personJob, new CancellationToken(true)))
			.ConfigureAwait(false);

		Assert.AreEqual(EntityState.Detached, DbContext.Entry(personJob).State);
	}
}
