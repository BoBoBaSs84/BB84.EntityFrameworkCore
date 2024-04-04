using BB84.EntityFrameworkCore.RepositoriesTests.Persistence;
using BB84.EntityFrameworkCore.RepositoriesTests.Persistence.Models;
using BB84.EntityFrameworkCore.RepositoriesTests.Persistence.Repositories;

namespace BB84.EntityFrameworkCore.RepositoriesTests;

[TestClass]
public sealed class PersonJobTests : UnitTestBase
{
	[TestMethod]
	public void CreateTest()
	{
		using TestDbContext dbContext = new(GetContextOptions());
		PersonJobRepository repository = new(dbContext);

		PersonJob personJob = new();

		repository.Create(personJob);
	}

	[TestMethod]
	public void CreateRangeTest()
	{
		using TestDbContext dbContext = new(GetContextOptions());
		PersonJobRepository repository = new(dbContext);

		List<PersonJob> personJobs = [new(), new()];

		repository.Create(personJobs);
	}

	[TestMethod]
	public async Task CreateAsyncTest()
	{
		using TestDbContext dbContext = new(GetContextOptions());
		PersonJobRepository repository = new(dbContext);

		PersonJob personJob = new();

		await repository.CreateAsync(personJob)
			.ConfigureAwait(false);
	}

	[TestMethod]
	public async Task CreateRangeAsyncTest()
	{
		using TestDbContext dbContext = new(GetContextOptions());
		PersonJobRepository repository = new(dbContext);

		List<PersonJob> personJobs = [new(), new()];

		await repository.CreateAsync(personJobs)
			.ConfigureAwait(false);
	}

	[TestMethod]
	public void CountTest()
	{
		using TestDbContext dbContext = new(GetContextOptions());
		PersonJobRepository repository = new(dbContext);

		var count = repository.Count();

		Assert.AreEqual(0, count);
	}

	[TestMethod]
	public void CountByConditionTest()
	{
		using TestDbContext dbContext = new(GetContextOptions());
		PersonJobRepository repository = new(dbContext);

		var count = repository.Count(x => x.PersonId.Equals(Guid.Empty));

		Assert.AreEqual(0, count);
	}

	[TestMethod]
	public async Task CountAsyncTest()
	{
		using TestDbContext dbContext = new(GetContextOptions());
		PersonJobRepository repository = new(dbContext);

		var count = await repository.CountAsync(true)
			.ConfigureAwait(false);

		Assert.AreEqual(0, count);
	}

	[TestMethod]
	public async Task CountByConditionAsyncTest()
	{
		using TestDbContext dbContext = new(GetContextOptions());
		PersonJobRepository repository = new(dbContext);

		var count = await repository.CountAsync(expression: x => x.PersonId.Equals(Guid.Empty))
			.ConfigureAwait(false);

		Assert.AreEqual(0, count);
	}
}
