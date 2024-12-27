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
		using TestDbContext dbContext = GetTestContext();
		PersonRepository repository = new(dbContext);

		int deleted = repository.Delete(Guid.NewGuid());

		Assert.AreEqual(0, deleted);
	}

	[TestMethod]
	public void DeleteByIdsTest()
	{
		using TestDbContext dbContext = GetTestContext();
		PersonRepository repository = new(dbContext);

		int deleted = repository.Delete([Guid.NewGuid(), Guid.NewGuid()]);

		Assert.AreEqual(0, deleted);
	}

	[TestMethod]
	public async Task DeleteByIdAsyncTest()
	{
		using TestDbContext dbContext = GetTestContext();
		PersonRepository repository = new(dbContext);

		int deleted = await repository.DeleteAsync(Guid.NewGuid())
			.ConfigureAwait(false);

		Assert.AreEqual(0, deleted);
	}

	[TestMethod]
	public async Task DeleteByIdsAsyncTest()
	{
		using TestDbContext dbContext = GetTestContext();
		PersonRepository repository = new(dbContext);

		int deleted = await repository.DeleteAsync([Guid.NewGuid(), Guid.NewGuid()])
			.ConfigureAwait(false);

		Assert.AreEqual(0, deleted);
	}

	[TestMethod]
	public void GetByIdTest()
	{
		using TestDbContext dbContext = GetTestContext();
		PersonRepository repository = new(dbContext);

		Person? person = repository.GetById(Guid.Empty);

		Assert.IsNull(person);
	}

	[TestMethod]
	public void GetByIdsTest()
	{
		using TestDbContext dbContext = GetTestContext();
		PersonRepository repository = new(dbContext);

		IEnumerable<Person> persons = repository.GetByIds([Guid.NewGuid(), Guid.NewGuid()]);

		Assert.IsFalse(persons.Any());
	}

	[TestMethod]
	public async Task GetByIdAsyncTest()
	{
		using TestDbContext dbContext = GetTestContext();
		PersonRepository repository = new(dbContext);

		Person? person = await repository.GetByIdAsync(Guid.Empty)
			.ConfigureAwait(false);

		Assert.IsNull(person);
	}

	[TestMethod]
	public async Task GetByIdsAsyncTest()
	{
		using TestDbContext dbContext = GetTestContext();
		PersonRepository repository = new(dbContext);

		IEnumerable<Person> persons = await repository.GetByIdsAsync([Guid.NewGuid(), Guid.NewGuid()])
			.ConfigureAwait(false);

		Assert.IsFalse(persons.Any());
	}

	[TestMethod]
	public void GetAllTest()
	{
		using TestDbContext dbContext = GetTestContext();
		PersonRepository repository = new(dbContext);

		IEnumerable<Person> persons = repository.GetAll(true, true);

		Assert.IsFalse(persons.Any());
	}

	[TestMethod]
	public async Task GetAllAsyncTest()
	{
		using TestDbContext dbContext = GetTestContext();
		PersonRepository repository = new(dbContext);

		IEnumerable<Person> persons = await repository.GetAllAsync(true, true)
			.ConfigureAwait(false);

		Assert.IsFalse(persons.Any());
	}

	[TestMethod]
	public void GetManyByConditionTest()
	{
		using TestDbContext dbContext = GetTestContext();
		PersonRepository repository = new(dbContext);

		IEnumerable<Person> persons = repository.GetManyByCondition(
			x => x.Id.Equals(Guid.Empty),
			x => x.Where(x => x.Id.Equals(Guid.Empty)),
			false, x => x.OrderBy(x => x.Id), 1, 1, false
			);

		Assert.IsFalse(persons.Any());
	}

	[TestMethod]
	public async Task GetManyByConditionAsyncTest()
	{
		using TestDbContext dbContext = GetTestContext();
		PersonRepository repository = new(dbContext);

		IEnumerable<Person> persons = await repository.GetManyByConditionAsync(
			x => x.Id.Equals(Guid.Empty),
			x => x.Where(x => x.Id.Equals(Guid.Empty)),
			false, x => x.OrderBy(x => x.Id), 1, 1, false
			).ConfigureAwait(false);

		Assert.IsFalse(persons.Any());
	}
}
