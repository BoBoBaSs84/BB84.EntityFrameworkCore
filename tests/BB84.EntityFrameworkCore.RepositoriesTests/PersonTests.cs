using BB84.EntityFrameworkCore.RepositoriesTests.Persistence;
using BB84.EntityFrameworkCore.RepositoriesTests.Persistence.Models;
using BB84.EntityFrameworkCore.RepositoriesTests.Persistence.Repositories;

namespace BB84.EntityFrameworkCore.RepositoriesTests;

[TestClass]
public sealed class PersonTests : UnitTestBase
{
	[TestMethod]
	public void GetByIdTest()
	{
		using TestDbContext dbContext = new(GetContextOptions());
		PersonRepository repository = new(dbContext);

		Person? person = repository.GetById(Guid.Empty);

		Assert.IsNull(person);
	}

	[TestMethod]
	public void GetByIdsTest()
	{
		using TestDbContext dbContext = new(GetContextOptions());
		PersonRepository repository = new(dbContext);

		IEnumerable<Person> persons = repository.GetByIds([Guid.NewGuid(), Guid.NewGuid()]);

		Assert.IsFalse(persons.Any());
	}

	[TestMethod]
	public async Task GetByIdAsyncTest()
	{
		using TestDbContext dbContext = new(GetContextOptions());
		PersonRepository repository = new(dbContext);

		Person? person = await repository.GetByIdAsync(Guid.Empty)
			.ConfigureAwait(false);

		Assert.IsNull(person);
	}

	[TestMethod]
	public async Task GetByIdsAsyncTest()
	{
		using TestDbContext dbContext = new(GetContextOptions());
		PersonRepository repository = new(dbContext);

		IEnumerable<Person> persons = await repository.GetByIdsAsync([Guid.NewGuid(), Guid.NewGuid()])
			.ConfigureAwait(false);

		Assert.IsFalse(persons.Any());
	}
}
