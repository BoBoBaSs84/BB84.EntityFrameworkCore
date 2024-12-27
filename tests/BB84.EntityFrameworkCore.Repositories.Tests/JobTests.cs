using BB84.EntityFrameworkCore.RepositoriesTests.Persistence;
using BB84.EntityFrameworkCore.RepositoriesTests.Persistence.Repositories;

namespace BB84.EntityFrameworkCore.RepositoriesTests;

[TestClass]
public sealed class JobTests : UnitTestBase
{
	[TestMethod]
	public void UpdateByIdTest()
	{
		using TestDbContext dbContext = GetTestContext();
		JobRepository repository = new(dbContext);

		int updated = repository.Update(Guid.NewGuid(), s => s.SetProperty(p => p.Name, "Tester"));

		Assert.AreEqual(0, updated);
	}

	[TestMethod]
	public void UpdateByIdsTest()
	{
		using TestDbContext dbContext = GetTestContext();
		JobRepository repository = new(dbContext);

		int updated = repository.Update([Guid.NewGuid(), Guid.NewGuid()], s => s.SetProperty(p => p.Name, "Tester"));

		Assert.AreEqual(0, updated);
	}

	[TestMethod]
	public async Task UpdateByIdAsyncTest()
	{
		using TestDbContext dbContext = GetTestContext();
		JobRepository repository = new(dbContext);

		int updated = await repository.UpdateAsync(Guid.NewGuid(), s => s.SetProperty(p => p.Name, "Tester"))
			.ConfigureAwait(false);

		Assert.AreEqual(0, updated);
	}

	[TestMethod]
	public async Task UpdateByIdsAsyncTest()
	{
		using TestDbContext dbContext = GetTestContext();
		JobRepository repository = new(dbContext);

		int updated = await repository.UpdateAsync([Guid.NewGuid(), Guid.NewGuid()], s => s.SetProperty(p => p.Name, "Tester"))
			.ConfigureAwait(false);

		Assert.AreEqual(0, updated);
	}
}
