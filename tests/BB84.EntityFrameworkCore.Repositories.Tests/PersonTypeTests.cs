// Copyright: 2024 Robert Peter Meyer
// License: MIT
//
// This source code is licensed under the MIT license found in the
// LICENSE file in the root directory of this source tree.
using BB84.EntityFrameworkCore.Repositories.Tests.Persistence.Entities;
using BB84.EntityFrameworkCore.Repositories.Tests.Persistence.Repositories;

using Microsoft.EntityFrameworkCore;

namespace BB84.EntityFrameworkCore.Repositories.Tests;

[TestClass]
public sealed class PersonTypeTests : UnitTestBase
{
	[TestMethod]
	public void GetByNameTest()
	{
		PersonTypeRepository repository = new(DbContext);

		PersonTypeEntity? result = repository.GetByName("Male");

		Assert.IsNotNull(result);
	}

	[TestMethod]
	public void GetByNamesTest()
	{
		PersonTypeRepository repository = new(DbContext);

		IEnumerable<PersonTypeEntity> result = repository.GetByNames(["Male", "Female"]);

		Assert.IsNotNull(result);
		Assert.AreEqual(2, result.Count());
	}

	[TestMethod]
	public async Task GetByNameAsyncTest()
	{
		PersonTypeRepository repository = new(DbContext);

		PersonTypeEntity? result = await repository.GetByNameAsync("Male", cancellationToken: TestContext.CancellationToken)
			.ConfigureAwait(false);

		Assert.IsNotNull(result);
	}

	[TestMethod]
	public async Task GetByNamesAsyncTest()
	{
		PersonTypeRepository repository = new(DbContext);

		IEnumerable<PersonTypeEntity> result = await repository.GetByNamesAsync(["Male", "Female"], cancellationToken: TestContext.CancellationToken)
			.ConfigureAwait(false);

		Assert.IsNotNull(result);
		Assert.AreEqual(2, result.Count());
	}

	[TestMethod]
	public void GetByConditionTest()
	{
		PersonRepository repository = new(DbContext);

		PersonEntity? person = repository.GetSingle(new()
		{
			Where = x => x.Id.Equals(Guid.Empty),
			Include = [x => x.Type]
		});

		Assert.IsNull(person);
	}

	[TestMethod]
	public async Task GetByConditionAsyncTest()
	{
		PersonRepository repository = new(DbContext);

		PersonEntity? person = await repository.GetSingleAsync(new()
		{
			Where = x => x.Id.Equals(Guid.Empty),
			Include = [x => x.Type]
		}, TestContext.CancellationToken).ConfigureAwait(false);

		Assert.IsNull(person);
	}

	[TestMethod]
	public void SoftDeleteTest()
	{
		PersonTypeRepository repository = new(DbContext);
		PersonTypeEntity entity = new() { Name = "SoftDeleteTest", Description = "To be soft deleted." };

		repository.Create(entity);
		_ = DbContext.SaveChanges();

		repository.Delete(entity);
		_ = DbContext.SaveChanges();

		Assert.IsTrue(entity.IsDeleted);
		Assert.IsNull(repository.GetById(entity.Id));
		Assert.IsNotNull(repository.GetById(entity.Id, new() { IgnoreQueryFilters = true }));

		Purge(entity);
	}

	[TestMethod]
	public async Task SoftDeleteAsyncTest()
	{
		PersonTypeRepository repository = new(DbContext);
		PersonTypeEntity entity = new() { Name = "SoftDeleteAsyncTest", Description = "To be soft deleted." };

		await repository.CreateAsync(entity, TestContext.CancellationToken);
		_ = await DbContext.SaveChangesAsync(TestContext.CancellationToken);

		repository.Delete(entity);
		_ = await DbContext.SaveChangesAsync(TestContext.CancellationToken);

		Assert.IsTrue(entity.IsDeleted);
		Assert.IsNull(await repository.GetByIdAsync(entity.Id, cancellationToken: TestContext.CancellationToken));
		Assert.IsNotNull(await repository.GetByIdAsync(entity.Id, new() { IgnoreQueryFilters = true }, TestContext.CancellationToken));

		Purge(entity);
	}

	/// <summary>
	/// Removes a soft deleted row for good, so that the shared test database is left as the
	/// seed data defines it.
	/// </summary>
	/// <remarks>
	/// This deliberately bypasses the repository. The expression based delete applies the
	/// global query filter, so it cannot reach a row that is already soft deleted.
	/// </remarks>
	/// <param name="entity">The soft deleted entity to remove.</param>
	private void Purge(PersonTypeEntity entity)
		=> _ = DbContext.Set<PersonTypeEntity>()
			.AsQueryable()
			.IgnoreQueryFilters()
			.Where(x => x.Id == entity.Id)
			.ExecuteDelete();

	public TestContext TestContext { get; set; }
}
