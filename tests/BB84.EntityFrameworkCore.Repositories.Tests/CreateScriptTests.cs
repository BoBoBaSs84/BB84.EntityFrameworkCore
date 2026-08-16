// Copyright: 2024 Robert Peter Meyer
// License: MIT
//
// This source code is licensed under the MIT license found in the
// LICENSE file in the root directory of this source tree.
using BB84.EntityFrameworkCore.Repositories.Tests.Persistence;

using Microsoft.EntityFrameworkCore;

namespace BB84.EntityFrameworkCore.Repositories.Tests;

/// <summary>
/// Generates the create script for the test model.
/// </summary>
/// <remarks>
/// This deliberately does not derive from <see cref="UnitTestBase"/>. MSTest discovers
/// inherited test methods, so a test declared there would run once per derived class and
/// every run would write the same file.
/// </remarks>
[TestClass]
public sealed class CreateScriptTests
{
	public TestContext TestContext { get; set; } = default!;

	[TestMethod]
	public void GenerateCreateScriptTest()
	{
		using TestDbContext dbContext = UnitTestBase.GetTestContext();

		string sqlScript = dbContext.Database.GenerateCreateScript();

		Assert.IsFalse(string.IsNullOrWhiteSpace(sqlScript));

		foreach (string tableName in new[] { "Jobs", "JobTypes", "Persons", "PersonJobs", "PersonTypes", "Skills" })
			Assert.Contains(tableName, sqlScript);

		string filePath = Path.Combine(TestContext.TestRunResultsDirectory ?? AppContext.BaseDirectory, "CreateScript.sql");
		File.WriteAllText(filePath, sqlScript);
	}
}
