// Copyright: 2024 Robert Peter Meyer
// License: MIT
//
// This source code is licensed under the MIT license found in the
// LICENSE file in the root directory of this source tree.
using BB84.EntityFrameworkCore.Repositories.Abstractions;
using BB84.EntityFrameworkCore.Repositories.Agnostic.Tests.Persistence;
using BB84.EntityFrameworkCore.Repositories.Interceptors;
using BB84.EntityFrameworkCore.TestSupport.Entities;

using Microsoft.Extensions.Time.Testing;

namespace BB84.EntityFrameworkCore.Repositories.Agnostic.Tests;

/// <summary>
/// Covers the audit interceptors against a controlled clock and a controlled identity.
/// </summary>
/// <remarks>
/// These build their own context rather than using the inherited one, because the point is to
/// supply interceptors other than the defaults.
/// </remarks>
[TestClass]
[SuppressMessage("Style", "IDE0058", Justification = "Not relevant here, unit testing.")]
public sealed class InterceptorTests : UnitTestBase
{
	private static readonly DateTimeOffset CreatedAt = new(2026, 8, 16, 10, 30, 0, TimeSpan.Zero);
	private static readonly DateTimeOffset EditedAt = new(2026, 8, 16, 14, 45, 0, TimeSpan.Zero);

	[TestMethod]
	public void TimeAuditedInterceptorUsesTheSuppliedClockTest()
	{
		FakeTimeProvider timeProvider = new(CreatedAt);
		using TestDbContext dbContext = GetTestContext(timeProvider, "Creator");
		SkillEntity entity = new() { Name = "TimeAudited", Description = "Created under a fake clock." };

		dbContext.Set<SkillEntity>().Add(entity);
		dbContext.SaveChanges();

		Assert.AreEqual(CreatedAt, entity.CreatedAt);
		Assert.IsNull(entity.EditedAt);

		timeProvider.SetUtcNow(EditedAt);
		entity.Description = "Edited under a fake clock.";
		dbContext.SaveChanges();

		Assert.AreEqual(CreatedAt, entity.CreatedAt);
		Assert.AreEqual(EditedAt, entity.EditedAt);

	}

	[TestMethod]
	public void TimeAuditedInterceptorReadsTheClockOncePerSaveTest()
	{
		// An advancing clock: every read returns a later value, so entities sharing a save
		// would drift apart if the interceptor read it more than once.
		FakeTimeProvider timeProvider = new(CreatedAt)
		{
			AutoAdvanceAmount = TimeSpan.FromMinutes(1)
		};

		using TestDbContext dbContext = GetTestContext(timeProvider, "Creator");
		SkillEntity first = new() { Name = "SharedSave1", Description = "First." };
		SkillEntity second = new() { Name = "SharedSave2", Description = "Second." };

		dbContext.Set<SkillEntity>().AddRange(first, second);
		dbContext.SaveChanges();

		Assert.AreEqual(first.CreatedAt, second.CreatedAt);

	}

	[TestMethod]
	public void UserAuditedInterceptorUsesTheSuppliedProviderTest()
	{
		using TestDbContext dbContext = GetTestContext(new FakeTimeProvider(CreatedAt), "DOMAIN\\Creator");
		SkillEntity entity = new() { Name = "UserAudited", Description = "Created by a fake user." };

		dbContext.Set<SkillEntity>().Add(entity);
		dbContext.SaveChanges();

		Assert.AreEqual("DOMAIN\\Creator", entity.CreatedBy);
		Assert.IsNull(entity.EditedBy);

		entity.Description = "Edited by a fake user.";
		dbContext.SaveChanges();

		Assert.AreEqual("DOMAIN\\Creator", entity.CreatedBy);
		Assert.AreEqual("DOMAIN\\Creator", entity.EditedBy);

	}

	[TestMethod]
	public void EnvironmentUserProviderReportsTheProcessUserTest()
	{
		EnvironmentUserProvider provider = new();

		Assert.AreEqual($"{Environment.MachineName}\\{Environment.UserName}", provider.GetCurrentUser());
	}

	private TestDbContext GetTestContext(TimeProvider timeProvider, string currentUser)
		=> CreateContext(timeProvider, new StubUserProvider(currentUser));

	private sealed class StubUserProvider(string currentUser) : ICurrentUserProvider
	{
		public string GetCurrentUser()
			=> currentUser;
	}
}
