using BB84.EntityFrameworkCore.RepositoriesTests.Persistence;
using BB84.EntityFrameworkCore.RepositoriesTests.Persistence.Models;
using BB84.EntityFrameworkCore.RepositoriesTests.Persistence.Repositories;

namespace BB84.EntityFrameworkCore.RepositoriesTests;

[TestClass]
public sealed class PersonTypeTests : UnitTestBase
{
	[TestMethod]
	public void GetByNameTest()
	{
		using TestDbContext dbContext = new(GetContextOptions());
		PersonTypeRepository repository = new(dbContext);

		PersonType? result = repository.GetByName("Male");

		Assert.IsNotNull(result);
	}

	[TestMethod]
	public void GetByNamesTest()
	{
		using TestDbContext dbContext = new(GetContextOptions());
		PersonTypeRepository repository = new(dbContext);

		IEnumerable<PersonType> result = repository.GetByNames(["Male", "Female"]);

		Assert.IsNotNull(result);
		Assert.AreEqual(2, result.Count());
	}

	[TestMethod]
	public async Task GetByNameAsyncTest()
	{
		using TestDbContext dbContext = new(GetContextOptions());
		PersonTypeRepository repository = new(dbContext);

		PersonType? result = await repository.GetByNameAsync("Male")
			.ConfigureAwait(false);

		Assert.IsNotNull(result);
	}

	[TestMethod]
	public async Task GetByNamesAsyncTest()
	{
		using TestDbContext dbContext = new(GetContextOptions());
		PersonTypeRepository repository = new(dbContext);

		IEnumerable<PersonType> result = await repository.GetByNamesAsync(["Male", "Female"])
			.ConfigureAwait(false);

		Assert.IsNotNull(result);
		Assert.AreEqual(2, result.Count());
	}

	[TestMethod]
	public void GetByConditionTest()
	{
		using TestDbContext dbContext = new(GetContextOptions());
		PersonRepository repository = new(dbContext);

		Person? person = repository.GetByCondition(
			expression: x => x.Id.Equals(Guid.Empty),
			includeProperties: [nameof(Person.Type)]
			);

		Assert.IsNull(person);
	}

	[TestMethod]
	public async Task GetByConditionAsyncTest()
	{
		using TestDbContext dbContext = new(GetContextOptions());
		PersonRepository repository = new(dbContext);

		Person? person = await repository.GetByConditionAsync(
			expression: x => x.Id.Equals(Guid.Empty),
			includeProperties: [nameof(Person.Type)]
			).ConfigureAwait(false);

		Assert.IsNull(person);
	}
}
