// Copyright: 2024 Robert Peter Meyer
// License: MIT
//
// This source code is licensed under the MIT license found in the
// LICENSE file in the root directory of this source tree.
using BB84.EntityFrameworkCore.TestSupport.Repositories;

namespace BB84.EntityFrameworkCore.Repositories.Agnostic.Tests;

[TestClass]
public sealed class JobTests : UnitTestBase
{
	[TestMethod]
	public void ExecuteUpdateByIdTest()
	{
		JobRepository repository = new(DbContext);

		int updated = repository.ExecuteUpdate(Guid.NewGuid(), s => s.SetProperty(p => p.Name, "Tester"));

		Assert.AreEqual(0, updated);
	}

	[TestMethod]
	public void ExecuteUpdateByIdsTest()
	{
		JobRepository repository = new(DbContext);

		int updated = repository.ExecuteUpdate([Guid.NewGuid(), Guid.NewGuid()], s => s.SetProperty(p => p.Name, "Tester"));

		Assert.AreEqual(0, updated);
	}

	[TestMethod]
	public async Task ExecuteUpdateByIdAsyncTest()
	{
		JobRepository repository = new(DbContext);

		int updated = await repository.ExecuteUpdateAsync(Guid.NewGuid(), s => s.SetProperty(p => p.Name, "Tester"), TestContext.CancellationToken)
			.ConfigureAwait(false);

		Assert.AreEqual(0, updated);
	}

	[TestMethod]
	public async Task ExecuteUpdateByIdsAsyncTest()
	{
		JobRepository repository = new(DbContext);

		int updated = await repository.ExecuteUpdateAsync([Guid.NewGuid(), Guid.NewGuid()], s => s.SetProperty(p => p.Name, "Tester"), TestContext.CancellationToken)
			.ConfigureAwait(false);

		Assert.AreEqual(0, updated);
	}

	public TestContext TestContext { get; set; }
}
