// Copyright: 2024 Robert Peter Meyer
// License: MIT
//
// This source code is licensed under the MIT license found in the
// LICENSE file in the root directory of this source tree.
using System.Reflection;

using BB84.EntityFrameworkCore.Repositories.Abstractions;
using BB84.EntityFrameworkCore.TestSupport.Entities;
using BB84.EntityFrameworkCore.TestSupport.Repositories;

using Microsoft.EntityFrameworkCore;

namespace BB84.EntityFrameworkCore.Repositories.Agnostic.Tests;

/// <summary>
/// Covers the read and write split of the repository abstractions.
/// </summary>
[TestClass]
public sealed class RepositorySegregationTests : UnitTestBase
{
	private static readonly string[] WriteVerbs = ["Create", "Update", "Delete"];
	private static readonly string[] ReadVerbs = ["Count", "Get", "Stream"];

	[TestMethod]
	public void ReadOnlyConsumerCanQueryTest()
	{
		// The consumer takes the read half only, so the compiler is what proves it cannot write.
		PersonTypeReader reader = new(new PersonTypeRepository(DbContext));

		PersonTypeEntity? result = reader.Find("Male");

		Assert.IsNotNull(result);
	}

	[TestMethod]
	public void WriteOnlyConsumerCanCreateTest()
	{
		PersonTypeWriter writer = new(new PersonTypeRepository(DbContext));
		PersonTypeEntity entity = new() { Name = "WriteOnlyConsumer", Description = "Created through the write half." };

		writer.Add(entity);
		_ = DbContext.SaveChanges();

		Assert.AreNotEqual(0, entity.Id);

		_ = DbContext.Set<PersonTypeEntity>()
			.Where(x => x.Id == entity.Id)
			.ExecuteDelete();
	}

	[TestMethod]
	public void ReadRepositoryDeclaresNoWriteMembersTest()
	{
		string[] declared = DeclaredMethodNames(typeof(IReadRepository<>))
			.Concat(DeclaredMethodNames(typeof(IReadIdentityRepository<,>)))
			.Concat(DeclaredMethodNames(typeof(IReadEnumeratorRepository<,>)))
			.ToArray();

		Assert.IsNotEmpty(declared, "Nothing was reflected over, so the check below proves nothing.");
		Assert.IsFalse(
			declared.Any(name => WriteVerbs.Any(verb => name.StartsWith(verb, StringComparison.Ordinal))),
			"A write member leaked into the read half of the abstractions.");
	}

	[TestMethod]
	public void WriteRepositoryDeclaresNoReadMembersTest()
	{
		string[] declared = DeclaredMethodNames(typeof(IWriteRepository<>))
			.Concat(DeclaredMethodNames(typeof(IWriteIdentityRepository<,>)))
			.ToArray();

		Assert.IsNotEmpty(declared, "Nothing was reflected over, so the check below proves nothing.");
		Assert.IsFalse(
			declared.Any(name => ReadVerbs.Any(verb => name.StartsWith(verb, StringComparison.Ordinal))),
			"A read member leaked into the write half of the abstractions.");
	}

	[TestMethod]
	public void ComposedInterfacesDeclareNoMembersOfTheirOwnTest()
	{
		// They exist to compose the halves. A member declared here would be invisible to a
		// consumer that depends on one half only.
		Assert.IsEmpty(DeclaredMethodNames(typeof(IGenericRepository<>)));
		Assert.IsEmpty(DeclaredMethodNames(typeof(IIdentityRepository<,>)));
		Assert.IsEmpty(DeclaredMethodNames(typeof(IEnumeratorRepository<,>)));
	}

	private static string[] DeclaredMethodNames(Type type)
		=> [.. type.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly).Select(x => x.Name)];

	private sealed class PersonTypeReader(IReadEnumeratorRepository<PersonTypeEntity> repository)
	{
		public PersonTypeEntity? Find(string name)
			=> repository.GetByName(name);
	}

	private sealed class PersonTypeWriter(IWriteRepository<PersonTypeEntity> repository)
	{
		public void Add(PersonTypeEntity entity)
			=> repository.Create(entity);
	}
}
