// Copyright: 2024 Robert Peter Meyer
// License: MIT
//
// This source code is licensed under the MIT license found in the
// LICENSE file in the root directory of this source tree.
#pragma warning disable CA1866 // Use char overload
#pragma warning disable CA1847 // Use char literal for a single character lookup
using BB84.EntityFrameworkCore.Repositories.Abstractions;
using BB84.EntityFrameworkCore.TestSupport.Entities;
using BB84.EntityFrameworkCore.TestSupport.Repositories;

using Microsoft.EntityFrameworkCore;

namespace BB84.EntityFrameworkCore.Repositories.Agnostic.Tests;

[TestClass]
public sealed class RepositoryOverloadTests : UnitTestBase
{
	private readonly CancellationToken _cancellationToken = CancellationToken.None;

	[TestMethod]
	public void QueryFilterOverloadsSyncTest()
	{
		PersonTypeRepository repository = new(DbContext);

		int count = repository
			.Count(new()
			{
				QueryFilter = query => query.Where(x => x.Name.Contains("ale")),
				IgnoreQueryFilters = true
			});

		PersonTypeEntity? single = repository
			.GetSingle(new()
			{
				QueryFilter = query => query.Where(x => x.Name == "Male"),
				IgnoreQueryFilters = true
			});

		IReadOnlyList<PersonTypeEntity> many = repository
			.GetList(new()
			{
				QueryFilter = query => query.Where(x => x.Name.Contains("e")),
				IgnoreQueryFilters = true,
				OrderBy = query => query.OrderBy(x => x.Name),
				Skip = 1,
				Take = 1
			});

		Assert.AreEqual(2, count);
		Assert.IsNotNull(single);
		Assert.AreEqual(2, single.Id);
		Assert.HasCount(1, many);
		Assert.AreEqual("Female", many.Single().Name);
	}

	[TestMethod]
	public async Task QueryFilterOverloadsAsyncTest()
	{
		PersonTypeRepository repository = new(DbContext);

		int count = await repository
			.CountAsync(
				new()
				{
					QueryFilter = query => query.Where(x => x.Name.StartsWith("D")),
					IgnoreQueryFilters = true
				},
				_cancellationToken)
			.ConfigureAwait(false);

		PersonTypeEntity? single = await repository
			.GetSingleAsync(
				new()
				{
					QueryFilter = query => query.Where(x => x.Name == "Female"),
					IgnoreQueryFilters = true
				},
				_cancellationToken)
			.ConfigureAwait(false);

		IReadOnlyList<PersonTypeEntity> many = await repository
			.GetListAsync(
				new()
				{
					QueryFilter = query => query.Where(x => x.Name.Contains("e")),
					IgnoreQueryFilters = true,
					OrderBy = query => query.OrderByDescending(x => x.Name),
					Skip = 1,
					Take = 1
				},
				_cancellationToken)
			.ConfigureAwait(false);

		Assert.AreEqual(1, count);
		Assert.IsNotNull(single);
		Assert.AreEqual(1, single.Id);
		Assert.HasCount(1, many);
		Assert.AreEqual("Female", many.Single().Name);
	}

	[TestMethod]
	public void ProjectionOverloadsSyncTest()
	{
		PersonTypeRepository repository = new(DbContext);
		Query<PersonTypeEntity> unfiltered = new() { IgnoreQueryFilters = true };

		IReadOnlyList<PersonTypeProjection> all = repository
			.GetList(
				selector: x => new PersonTypeProjection
				{
					Id = x.Id,
					Name = x.Name.ToUpperInvariant(),
					Description = null
				},
				query: unfiltered);

		PersonTypeProjection? byId = repository
			.GetById(
				id: 2,
				selector: x => new PersonTypeProjection
				{
					Id = x.Id,
					Name = x.Name.ToLowerInvariant(),
					Description = x.Description
				},
				query: unfiltered);

		IReadOnlyList<PersonTypeProjection> byIds = repository
			.GetByIds(
				ids: [1, 3],
				selector: x => new PersonTypeProjection
				{
					Id = x.Id,
					Name = x.Name,
					Description = null
				},
				query: unfiltered);

		PersonTypeProjection? byCondition = repository
			.GetSingle(
				selector: x => new PersonTypeProjection
				{
					Id = x.Id,
					Name = $"Type:{x.Name}",
					Description = null
				},
				query: unfiltered with { Where = x => x.Id == 1 });

		IReadOnlyList<PersonTypeProjection> manyByCondition = repository
			.GetList(
				selector: x => new PersonTypeProjection
				{
					Id = x.Id,
					Name = x.Name,
					Description = null
				},
				query: unfiltered with
				{
					Where = x => x.Id > 1,
					OrderBy = query => query.OrderBy(x => x.Id)
				});

		Assert.HasCount(3, all);
		Assert.IsTrue(all.All(x => x.Description is null));
		Assert.Contains(x => x.Name == "MALE", all);

		Assert.IsNotNull(byId);
		Assert.AreEqual("male", byId.Name);

		Assert.HasCount(2, byIds);
		Assert.IsTrue(byIds.All(x => x.Description is null));

		Assert.IsNotNull(byCondition);
		Assert.AreEqual("Type:Female", byCondition.Name);
		Assert.IsNull(byCondition.Description);

		Assert.HasCount(2, manyByCondition);
		Assert.IsTrue(manyByCondition.All(x => x.Description is null));
	}

	[TestMethod]
	public async Task ProjectionOverloadsAsyncTest()
	{
		PersonTypeRepository repository = new(DbContext);
		Query<PersonTypeEntity> unfiltered = new() { IgnoreQueryFilters = true };

		IReadOnlyList<PersonTypeProjection> all = await repository
			.GetListAsync(
				selector: x => new PersonTypeProjection
				{
					Id = x.Id,
					Name = x.Name.ToUpperInvariant(),
					Description = null
				},
				query: unfiltered,
				cancellationToken: _cancellationToken)
			.ConfigureAwait(false);

		PersonTypeProjection? byId = await repository
			.GetByIdAsync(
				id: 2,
				selector: x => new PersonTypeProjection
				{
					Id = x.Id,
					Name = x.Name.ToLowerInvariant(),
					Description = x.Description
				},
				query: unfiltered,
				cancellationToken: _cancellationToken)
			.ConfigureAwait(false);

		IReadOnlyList<PersonTypeProjection> byIds = await repository
			.GetByIdsAsync(
				ids: [1, 3],
				selector: x => new PersonTypeProjection
				{
					Id = x.Id,
					Name = x.Name,
					Description = null
				},
				query: unfiltered,
				cancellationToken: _cancellationToken)
			.ConfigureAwait(false);

		PersonTypeProjection? byCondition = await repository
			.GetSingleAsync(
				selector: x => new PersonTypeProjection
				{
					Id = x.Id,
					Name = $"Type:{x.Name}",
					Description = null
				},
				query: unfiltered with { Where = x => x.Id == 1 },
				cancellationToken: _cancellationToken)
			.ConfigureAwait(false);

		IReadOnlyList<PersonTypeProjection> manyByCondition = await repository
			.GetListAsync(
				selector: x => new PersonTypeProjection
				{
					Id = x.Id,
					Name = x.Name,
					Description = null
				},
				query: unfiltered with
				{
					Where = x => x.Id > 1,
					OrderBy = query => query.OrderBy(x => x.Id)
				},
				cancellationToken: _cancellationToken)
			.ConfigureAwait(false);

		Assert.HasCount(3, all);
		Assert.IsTrue(all.All(x => x.Description is null));
		Assert.IsNotNull(byId);
		Assert.AreEqual("male", byId.Name);
		Assert.HasCount(2, byIds);
		Assert.IsNotNull(byCondition);
		Assert.AreEqual("Type:Female", byCondition.Name);
		Assert.HasCount(2, manyByCondition);
	}

	[TestMethod]
	public void QueryConditionsCombineRatherThanReplaceTest()
	{
		PersonTypeRepository repository = new(DbContext);

		// The identifier and the caller supplied condition both have to hold, so a mismatched
		// pair matches nothing rather than the identifier silently winning.
		PersonTypeEntity? matching = repository.GetById(2, new() { Where = x => x.Name == "Male" });
		PersonTypeEntity? conflicting = repository.GetById(2, new() { Where = x => x.Name == "Female" });

		Assert.IsNotNull(matching);
		Assert.IsNull(conflicting);
	}

	[TestMethod]
	public void CountIgnoresOrderingAndPagingTest()
	{
		PersonTypeRepository repository = new(DbContext);

		// Ordering and paging cannot change how many rows match, so a count has to ignore them.
		int count = repository.Count(new()
		{
			IgnoreQueryFilters = true,
			OrderBy = query => query.OrderBy(x => x.Name),
			Skip = 1,
			Take = 1
		});

		Assert.AreEqual(3, count);
	}

	[TestMethod]
	public void ExecuteDeleteByConditionRemovesMatchingEntitiesTest()
	{
		SkillRepository repository = new(DbContext);
		string uniqueName = $"Skill-{Guid.NewGuid():N}";

		SkillEntity entity = new()
		{
			Id = Guid.NewGuid(),
			Name = uniqueName,
			Description = "Created for delete test.",
			IsCritical = true
		};

		repository.Create(entity);
		_ = DbContext.SaveChanges();

		try
		{
			int deleted = repository.ExecuteDelete(x => x.Name == uniqueName);

			SkillEntity? result = repository.GetSingle(new() { Where = x => x.Name == uniqueName });

			Assert.AreEqual(1, deleted);
			Assert.IsNull(result);
		}
		finally
		{
			_ = DbContext.Set<SkillEntity>()
				.Where(x => x.Name == uniqueName)
				.ExecuteDelete();
		}
	}

	[TestMethod]
	public void ExecuteUpdateByConditionAndIdsModifyMatchingEntitiesTest()
	{
		JobRepository repository = new(DbContext);
		string uniquePrefix = $"Job-{Guid.NewGuid():N}";

		JobEntity first = new()
		{
			Id = Guid.NewGuid(),
			Name = $"{uniquePrefix}-1",
			Description = "Before"
		};

		JobEntity second = new()
		{
			Id = Guid.NewGuid(),
			Name = $"{uniquePrefix}-2",
			Description = "Before"
		};

		repository.Create([first, second]);
		_ = DbContext.SaveChanges();

		try
		{
			int updatedByCondition = repository.ExecuteUpdate(
				expression: x => x.Name.StartsWith(uniquePrefix),
				setPropertyCalls: s => s.SetProperty(p => p.Description, "AfterCondition")
				);

			int updatedByIds = repository.ExecuteUpdate(
				ids: [first.Id, second.Id],
				setPropertyCalls: s => s.SetProperty(p => p.Name, "Updated")
				);

			IReadOnlyList<JobEntity> jobs = repository.GetByIds([first.Id, second.Id]);

			Assert.AreEqual(2, updatedByCondition);
			Assert.AreEqual(2, updatedByIds);
			Assert.HasCount(2, jobs);
			Assert.IsTrue(jobs.All(x => x.Name == "Updated"));
			Assert.IsTrue(jobs.All(x => x.Description == "AfterCondition"));
		}
		finally
		{
			_ = DbContext.Set<JobEntity>()
				.Where(x => x.Id == first.Id || x.Id == second.Id)
				.ExecuteDelete();
		}
	}

	[TestMethod]
	public async Task StreamOverloadsAsyncTest()
	{
		PersonTypeRepository repository = new(DbContext);
		Query<PersonTypeEntity> unfiltered = new() { IgnoreQueryFilters = true };

		List<PersonTypeEntity> all = [];
		await foreach (PersonTypeEntity entity in repository
			.Stream(unfiltered, _cancellationToken)
			.ConfigureAwait(false))
		{
			all.Add(entity);
		}

		List<PersonTypeEntity> byQueryFilter = [];
		await foreach (PersonTypeEntity entity in repository
			.Stream(
				unfiltered with
				{
					QueryFilter = query => query.Where(x => x.Name.Contains("e")),
					OrderBy = query => query.OrderBy(x => x.Name),
					Skip = 1,
					Take = 1
				},
				_cancellationToken)
			.ConfigureAwait(false))
		{
			byQueryFilter.Add(entity);
		}

		List<PersonTypeEntity> byExpression = [];
		await foreach (PersonTypeEntity entity in repository
			.Stream(
				unfiltered with
				{
					Where = x => x.Id > 1,
					OrderBy = query => query.OrderBy(x => x.Id)
				},
				_cancellationToken)
			.ConfigureAwait(false))
		{
			byExpression.Add(entity);
		}

		List<PersonTypeProjection> projected = [];
		await foreach (PersonTypeProjection projection in repository
			.Stream(
				selector: x => new PersonTypeProjection
				{
					Id = x.Id,
					Name = x.Name.ToUpperInvariant(),
					Description = null
				},
				query: unfiltered with
				{
					Where = x => x.Id > 1,
					OrderBy = query => query.OrderBy(x => x.Id)
				},
				cancellationToken: _cancellationToken)
			.ConfigureAwait(false))
		{
			projected.Add(projection);
		}

		Assert.HasCount(3, all);

		Assert.HasCount(1, byQueryFilter);
		Assert.AreEqual("Female", byQueryFilter.Single().Name);

		Assert.HasCount(2, byExpression);
		Assert.AreEqual(2, byExpression[0].Id);
		Assert.AreEqual(3, byExpression[1].Id);

		Assert.HasCount(2, projected);
		Assert.IsTrue(projected.All(x => x.Description is null));
		Assert.AreEqual("MALE", projected[0].Name);
	}

	[TestMethod]
	public async Task StreamHonorsCancellationTest()
	{
		PersonTypeRepository repository = new(DbContext);
		using CancellationTokenSource tokenSource = new();
		await tokenSource.CancelAsync().ConfigureAwait(false);

		_ = await Assert.ThrowsAsync<OperationCanceledException>(async () =>
		{
			await foreach (PersonTypeEntity entity in repository
				.Stream(new() { IgnoreQueryFilters = true }, tokenSource.Token)
				.ConfigureAwait(false))
			{
				Assert.IsNotNull(entity);
			}
		}).ConfigureAwait(false);
	}

	[TestMethod]
	public async Task StreamYieldsEntitiesIncrementallyTest()
	{
		PersonTypeRepository repository = new(DbContext);
		List<int> trackedAfterEachEntity = [];

		await foreach (PersonTypeEntity entity in repository
			.Stream(new() { IgnoreQueryFilters = true, TrackChanges = true }, _cancellationToken)
			.ConfigureAwait(false))
		{
			Assert.IsNotNull(entity);
			trackedAfterEachEntity.Add(DbContext.ChangeTracker.Entries<PersonTypeEntity>().Count());
		}

		List<int> expectedTrackedCounts = [1, 2, 3];

		Assert.HasCount(3, trackedAfterEachEntity);
		CollectionAssert.AreEqual(expectedTrackedCounts, trackedAfterEachEntity);
	}

	private sealed class PersonTypeProjection
	{
		public int Id { get; init; }
		public string Name { get; init; } = string.Empty;
		public string? Description { get; init; }
	}
}
