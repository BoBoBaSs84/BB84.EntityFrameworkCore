#pragma warning disable CA1866 // Use char overload
#pragma warning disable CA1847 // Use char literal for a single character lookup
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
public sealed class RepositoryOverloadTests : UnitTestBase
{
	private readonly CancellationToken _cancellationToken = CancellationToken.None;

	[TestMethod]
	public void QueryFilterOverloadsSyncTest()
	{
		using TestDbContext dbContext = GetTestContext();
		PersonTypeRepository repository = new(dbContext);

		int count = repository
			.CountByCondition(
				queryFilter: query => query.Where(x => x.Name.Contains("ale")),
				ignoreQueryFilters: true);

		PersonTypeEntity? single = repository
			.GetByCondition(
				queryFilter: query => query.Where(x => x.Name == "Male"),
				ignoreQueryFilters: true);

		IReadOnlyList<PersonTypeEntity> many = repository
			.GetManyByCondition(
				queryFilter: query => query.Where(x => x.Name.Contains("e")),
				ignoreQueryFilters: true,
				orderBy: query => query.OrderBy(x => x.Name),
				skip: 1,
				take: 1);

		Assert.AreEqual(2, count);
		Assert.IsNotNull(single);
		Assert.AreEqual(2, single.Id);
		Assert.HasCount(1, many);
		Assert.AreEqual("Female", many.Single().Name);
	}

	[TestMethod]
	public async Task QueryFilterOverloadsAsyncTest()
	{
		using TestDbContext dbContext = GetTestContext();
		PersonTypeRepository repository = new(dbContext);

		int count = await repository
			.CountByConditionAsync(
				queryFilter: query => query.Where(x => x.Name.StartsWith("D")),
				ignoreQueryFilters: true,
				token: _cancellationToken)
			.ConfigureAwait(false);
		PersonTypeEntity? single = await repository
			.GetByConditionAsync(
				queryFilter: query => query.Where(x => x.Name == "Female"),
				ignoreQueryFilters: true,
				token: _cancellationToken)
			.ConfigureAwait(false);

		IReadOnlyList<PersonTypeEntity> many = await repository
			.GetManyByConditionAsync(
				queryFilter: query => query.Where(x => x.Name.Contains("e")),
				ignoreQueryFilters: true,
				orderBy: query => query.OrderByDescending(x => x.Name),
				skip: 1,
				take: 1,
				token: _cancellationToken)
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
		using TestDbContext dbContext = GetTestContext();
		PersonTypeRepository repository = new(dbContext);

		IReadOnlyList<PersonTypeProjection> all = repository
			.GetAll(
				selector: x => new PersonTypeProjection
				{
					Id = x.Id,
					Name = x.Name,
					Description = x.Description
				},
				fieldSelector: x => new PersonTypeProjection
				{
					Id = x.Id,
					Name = x.Name.ToUpperInvariant(),
					Description = null
				},
				ignoreQueryFilters: true);

		PersonTypeProjection? byId = repository
			.GetById(
				id: 2,
				selector: x => new PersonTypeProjection
				{
					Id = x.Id,
					Name = x.Name,
					Description = x.Description
				},
				fieldSelector: x => new PersonTypeProjection
				{
					Id = x.Id,
					Name = x.Name.ToLowerInvariant(),
					Description = x.Description
				},
				ignoreQueryFilters: true);

		IReadOnlyList<PersonTypeProjection> byIds = repository
			.GetByIds(
				ids: [1, 3],
				selector: x => new PersonTypeProjection
				{
					Id = x.Id,
					Name = x.Name,
					Description = x.Description
				},
				fieldSelector: x => new PersonTypeProjection
				{
					Id = x.Id,
					Name = x.Name,
					Description = null
				},
				ignoreQueryFilters: true);

		PersonTypeProjection? byCondition = repository
				.GetByCondition(
				expression: x => x.Id == 1,
				selector: x => new PersonTypeProjection
				{
					Id = x.Id,
					Name = x.Name,
					Description = x.Description
				},
				fieldSelector: x => new PersonTypeProjection
				{
					Id = x.Id,
					Name = $"Type:{x.Name}",
					Description = null
				},
				ignoreQueryFilters: true);

		IReadOnlyList<PersonTypeProjection> manyByCondition = repository
			.GetManyByCondition(
				expression: x => x.Id > 1,
				selector: x => new PersonTypeProjection
				{
					Id = x.Id,
					Name = x.Name,
					Description = x.Description
				},
				fieldSelector: x => new PersonTypeProjection
				{
					Id = x.Id,
					Name = x.Name,
					Description = null
				},
				ignoreQueryFilters: true,
				orderBy: query => query.OrderBy(x => x.Id));

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
		using TestDbContext dbContext = GetTestContext();
		PersonTypeRepository repository = new(dbContext);

		IReadOnlyList<PersonTypeProjection> all = await repository
			.GetAllAsync(
				selector: x => new PersonTypeProjection
				{
					Id = x.Id,
					Name = x.Name,
					Description = x.Description
				},
				fieldSelector: x => new PersonTypeProjection
				{
					Id = x.Id,
					Name = x.Name.ToUpperInvariant(),
					Description = null
				},
				ignoreQueryFilters: true,
				token: _cancellationToken)
			.ConfigureAwait(false);

		PersonTypeProjection? byId = await repository
			.GetByIdAsync(
				id: 2,
				selector: x => new PersonTypeProjection
				{
					Id = x.Id,
					Name = x.Name,
					Description = x.Description
				},
				fieldSelector: x => new PersonTypeProjection
				{
					Id = x.Id,
					Name = x.Name.ToLowerInvariant(),
					Description = x.Description
				},
				ignoreQueryFilters: true,
				token: _cancellationToken)
			.ConfigureAwait(false);

		IReadOnlyList<PersonTypeProjection> byIds = await repository
			.GetByIdsAsync(
				ids: [1, 3],
				selector: x => new PersonTypeProjection
				{
					Id = x.Id,
					Name = x.Name,
					Description = x.Description
				},
				fieldSelector: x => new PersonTypeProjection
				{
					Id = x.Id,
					Name = x.Name,
					Description = null
				},
				ignoreQueryFilters: true,
				token: _cancellationToken)
			.ConfigureAwait(false);

		PersonTypeProjection? byCondition = await repository
			.GetByConditionAsync(
				expression: x => x.Id == 1,
				selector: x => new PersonTypeProjection
				{
					Id = x.Id,
					Name = x.Name,
					Description = x.Description
				},
				fieldSelector: x => new PersonTypeProjection
				{
					Id = x.Id,
					Name = $"Type:{x.Name}",
					Description = null
				},
				ignoreQueryFilters: true,
				token: _cancellationToken)
			.ConfigureAwait(false);

		IReadOnlyList<PersonTypeProjection> manyByCondition = await repository
			.GetManyByConditionAsync(
				expression: x => x.Id > 1,
				selector: x => new PersonTypeProjection
				{
					Id = x.Id,
					Name = x.Name,
					Description = x.Description
				},
				fieldSelector: x => new PersonTypeProjection
				{
					Id = x.Id,
					Name = x.Name,
					Description = null
				},
				ignoreQueryFilters: true,
				orderBy: query => query.OrderBy(x => x.Id),
				token: _cancellationToken
				)
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
	public void DeleteByConditionRemovesMatchingEntitiesTest()
	{
		using TestDbContext dbContext = GetTestContext();
		SkillRepository repository = new(dbContext);
		string uniqueName = $"Skill-{Guid.NewGuid():N}";

		SkillEntity entity = new()
		{
			Id = Guid.NewGuid(),
			Name = uniqueName,
			Description = "Created for delete test.",
			IsCritical = true
		};

		repository.Create(entity);
		_ = dbContext.SaveChanges();

		try
		{
			int deleted = repository.Delete(x => x.Name == uniqueName);

			SkillEntity? result = repository.GetByCondition(
				expression: x => x.Name == uniqueName,
				trackChanges: false);

			Assert.AreEqual(1, deleted);
			Assert.IsNull(result);
		}
		finally
		{
			_ = dbContext.Set<SkillEntity>()
				.Where(x => x.Name == uniqueName)
				.ExecuteDelete();
		}
	}

	[TestMethod]
	public void UpdateByConditionAndIdsModifyMatchingEntitiesTest()
	{
		using TestDbContext dbContext = GetTestContext();
		JobRepository repository = new(dbContext);
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
		_ = dbContext.SaveChanges();

		try
		{
			int updatedByCondition = repository.Update(
				expression: x => x.Name.StartsWith(uniquePrefix),
				setPropertyCalls: s => s.SetProperty(p => p.Description, "AfterCondition")
				);

			int updatedByIds = repository.Update(
				ids: [first.Id, second.Id],
				setPropertyCalls: s => s.SetProperty(p => p.Name, "Updated")
				);

			IReadOnlyList<JobEntity> jobs = repository.GetByIds(
				ids: [first.Id, second.Id],
				trackChanges: false
				);

			Assert.AreEqual(2, updatedByCondition);
			Assert.AreEqual(2, updatedByIds);
			Assert.HasCount(2, jobs);
			Assert.IsTrue(jobs.All(x => x.Name == "Updated"));
			Assert.IsTrue(jobs.All(x => x.Description == "AfterCondition"));
		}
		finally
		{
			_ = dbContext.Set<JobEntity>()
				.Where(x => x.Id == first.Id || x.Id == second.Id)
				.ExecuteDelete();
		}
	}

	private sealed class PersonTypeProjection
	{
		public int Id { get; init; }
		public string Name { get; init; } = string.Empty;
		public string? Description { get; init; }
	}
}
