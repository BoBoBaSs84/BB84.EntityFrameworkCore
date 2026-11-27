// Copyright: 2024 Robert Peter Meyer
// License: MIT
//
// This source code is licensed under the MIT license found in the
// LICENSE file in the root directory of this source tree.
using BB84.EntityFrameworkCore.Entities;
using BB84.EntityFrameworkCore.Entities.Abstractions.Components;
using BB84.EntityFrameworkCore.Repositories.Abstractions;
using BB84.EntityFrameworkCore.Repositories.Agnostic.Tests.Persistence;
using BB84.EntityFrameworkCore.Repositories.Interceptors;
using BB84.EntityFrameworkCore.TestSupport.Entities;

using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Time.Testing;

namespace BB84.EntityFrameworkCore.Repositories.Agnostic.Tests;

/// <summary>
/// Covers the interceptor branches the happy-path tests never reach.
/// </summary>
/// <remarks>
/// <see cref="InterceptorTests"/> covers the states the interceptors act on. These cover the
/// states they must leave alone, plus the asynchronous save path and the default clock.
/// </remarks>
[TestClass]
[SuppressMessage("Style", "IDE0058", Justification = "Not relevant here, unit testing.")]
public sealed class InterceptorEdgeCaseTests : UnitTestBase
{
	private readonly CancellationToken _testToken = CancellationToken.None;
	private static readonly DateTimeOffset Now = new(2026, 8, 17, 9, 0, 0, TimeSpan.Zero);

	[TestMethod]
	public void SoftDeletableInterceptorShouldLeaveAnAddedEntityAlone()
	{
		PersonTypeEntity entity = new() { Name = "AddedNotDeleted", Description = "Inserted, not removed." };

		DbContext.Set<PersonTypeEntity>().Add(entity);
		DbContext.SaveChanges();

		Assert.IsFalse(entity.IsDeleted, "an insert must not be treated as a soft delete");
	}

	[TestMethod]
	public void SoftDeletableInterceptorShouldTurnADeleteIntoAnUpdate()
	{
		PersonTypeEntity entity = DbContext.Set<PersonTypeEntity>().Single(x => x.Name == "Male");

		DbContext.Set<PersonTypeEntity>().Remove(entity);
		DbContext.SaveChanges();

		Assert.IsTrue(entity.IsDeleted);
		Assert.AreEqual(EntityState.Unchanged, DbContext.Entry(entity).State, "the row should have been updated, not removed");
		Assert.IsTrue(DbContext.Set<PersonTypeEntity>().IgnoreQueryFilters().Any(x => x.Name == "Male"), "the row should still be there");
	}

	/// <summary>
	/// Soft-deleting a stub that carries only the key and token writes <c>IsDeleted</c> alone.
	/// </summary>
	/// <remarks>
	/// The interceptor once set the entry to <see cref="EntityState.Modified"/>, marking every
	/// column, so the stub's empty values overwrote the stored row.
	/// </remarks>
	[TestMethod]
	public void SoftDeletableInterceptorShouldOnlyWriteIsDeletedForAStub()
	{
		PersonTypeEntity stored = DbContext.Set<PersonTypeEntity>().AsNoTracking().Single(x => x.Name == "Male");
		PersonTypeEntity stub = new() { Id = stored.Id, Name = string.Empty, Timestamp = stored.Timestamp };

		DbContext.Set<PersonTypeEntity>().Remove(stub);
		DbContext.SaveChanges();
		DbContext.ChangeTracker.Clear();

		PersonTypeEntity reloaded = DbContext.Set<PersonTypeEntity>().IgnoreQueryFilters().Single(x => x.Id == stored.Id);
		Assert.IsTrue(reloaded.IsDeleted);
		Assert.AreEqual(stored.Name, reloaded.Name);
		Assert.AreEqual(stored.Description, reloaded.Description);
	}

	[TestMethod]
	public void TimeAuditedInterceptorShouldDefaultToTheSystemClock()
	{
		DateTimeOffset before = DateTimeOffset.UtcNow;

		// No TimeProvider supplied, so the interceptor falls back to TimeProvider.System.
		using TestDbContext context = CreateContext();
		SkillEntity entity = new() { Name = "SystemClock", Description = "Timed by the default clock." };

		context.Set<SkillEntity>().Add(entity);
		context.SaveChanges();

		Assert.IsGreaterThanOrEqualTo(before, entity.CreatedAt);
		Assert.IsLessThanOrEqualTo(DateTimeOffset.UtcNow, entity.CreatedAt);
	}

	[TestMethod]
	public void AuditInterceptorsShouldLeaveADeletedEntityAlone()
	{
		FakeTimeProvider timeProvider = new(Now);
		using TestDbContext context = CreateContext(timeProvider);
		SkillEntity entity = new() { Name = "HardDeleted", Description = "Removed outright." };

		context.Set<SkillEntity>().Add(entity);
		context.SaveChanges();

		// SkillEntity is not soft-deletable, so this is a real delete.
		context.Set<SkillEntity>().Remove(entity);
		context.SaveChanges();

		Assert.IsNull(entity.EditedAt, "a delete is not an edit");
		Assert.IsNull(entity.EditedBy, "a delete is not an edit");
	}

	[TestMethod]
	public async Task AuditInterceptorsShouldRunOnTheAsynchronousSavePath()
	{
		FakeTimeProvider timeProvider = new(Now);
		using TestDbContext context = CreateContext(timeProvider);
		SkillEntity entity = new() { Name = "AsyncSave", Description = "Saved asynchronously." };

		context.Set<SkillEntity>().Add(entity);
		await context.SaveChangesAsync(_testToken);

		Assert.AreEqual(Now, entity.CreatedAt);
		Assert.IsNotNull(entity.CreatedBy);

		entity.Description = "Edited asynchronously.";
		await context.SaveChangesAsync(_testToken);

		Assert.AreEqual(Now, entity.EditedAt);
		Assert.IsNotNull(entity.EditedBy);
	}

	/// <summary>
	/// The editor's nullability does not decide whether the user audit interceptor sees an entity.
	/// </summary>
	/// <remarks>
	/// For a reference type user, an entity declaring a non-nullable editor is not a separate case:
	/// nullable reference annotations are erased at runtime, so both spellings are the same
	/// constructed type and both are intercepted. Pinned here so the apparent distinction is not
	/// "fixed" into a real one. Value types do differ, see
	/// <see cref="UserAuditedInterceptorShouldRecordAValueTypeUser"/>.
	/// </remarks>
	[TestMethod]
	public void UserAuditedInterceptorShouldNotDistinguishTheEditorsNullability()
	{
		Assert.AreSame(
			typeof(IUserAudited<string, string?>),
			typeof(IUserAudited<string, string>),
			"nullable annotations are erased, so these are one type");

		Assert.Contains(typeof(IUserAudited<string, string?>), typeof(NonNullableEditorProbe).GetInterfaces());
	}

	/// <summary>
	/// A value-type user is recorded, whether the editor is nullable or not.
	/// </summary>
	/// <remarks>
	/// Unlike for <see cref="string"/>, <c>IUserAudited&lt;int, int?&gt;</c> and
	/// <c>IUserAudited&lt;int, int&gt;</c> are two distinct types, and the interceptor once
	/// matched only the latter, so an <c>int?</c> editor silently got no audit at all.
	/// </remarks>
	[TestMethod]
	public void UserAuditedInterceptorShouldRecordAValueTypeUser()
	{
		using SqliteConnection connection = new("Data Source=:memory:");
		connection.Open();
		using IntUserDbContext context = new(connection);
		context.Database.EnsureCreated();
		NullableIntEditorEntity nullableEditor = new() { Name = "Nullable" };
		IntEditorEntity plainEditor = new() { Name = "Plain" };

		context.AddRange(nullableEditor, plainEditor);
		context.SaveChanges();

		Assert.AreEqual(42, nullableEditor.CreatedBy);
		Assert.IsNull(nullableEditor.EditedBy, "an insert is not an edit");
		Assert.AreEqual(42, plainEditor.CreatedBy);

		nullableEditor.Name = "Nullable, edited";
		plainEditor.Name = "Plain, edited";
		context.SaveChanges();

		Assert.AreEqual(42, nullableEditor.EditedBy);
		Assert.AreEqual(42, plainEditor.EditedBy);
	}

	private sealed class NonNullableEditorProbe : IUserAudited<string, string>
	{
		public string CreatedBy { get; set; } = string.Empty;
		public string EditedBy { get; set; } = string.Empty;
	}

	private sealed class NullableIntEditorEntity : FullAuditedEntity<Guid, int, int?>
	{
		public required string Name { get; set; }
	}

	private sealed class IntEditorEntity : FullAuditedEntity<Guid, int, int>
	{
		public required string Name { get; set; }
	}

	private sealed class IntUserProvider : ICurrentUserProvider<int>
	{
		public int GetCurrentUser() => 42;
	}

	private sealed class IntUserDbContext(SqliteConnection connection) : DbContext
	{
		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
			=> optionsBuilder
				.UseSqlite(connection)
				.AddInterceptors(new UserAuditedInterceptor<int>(new IntUserProvider()));

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<NullableIntEditorEntity>();
			modelBuilder.Entity<IntEditorEntity>();
		}
	}
}
