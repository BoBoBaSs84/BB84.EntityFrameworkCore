// Copyright: 2024 Robert Peter Meyer
// License: MIT
//
// This source code is licensed under the MIT license found in the
// LICENSE file in the root directory of this source tree.
using BB84.EntityFrameworkCore.Repositories.Interceptors;
using BB84.EntityFrameworkCore.TestSupport.Repositories;

namespace BB84.EntityFrameworkCore.Repositories.Agnostic.Tests;

/// <summary>
/// Covers the constructor guards of the repository ladder and the audit interceptors.
/// </summary>
[TestClass]
public sealed class ConstructorGuardTests
{
	/// <summary>
	/// Every rung of the repository ladder, since each forwards the context through a primary
	/// constructor and none of them declares a field initializer of its own.
	/// </summary>
	[TestMethod]
	public void RepositoriesShouldRejectANullContext()
	{
		AssertRejects("dbContext", () => new PersonJobRepository(null!));
		AssertRejects("dbContext", () => new SkillRepository(null!));
		AssertRejects("dbContext", () => new JobRepository(null!));
		AssertRejects("dbContext", () => new PersonRepository(null!));
		AssertRejects("dbContext", () => new PersonTypeRepository(null!));
	}

	[TestMethod]
	public void UserAuditedInterceptorShouldRejectANullProvider()
	{
		AssertRejects("currentUserProvider", () => new UserAuditedInterceptor(null!));
		AssertRejects("currentUserProvider", () => new UserAuditedInterceptor<string>(null!));
	}

	/// <summary>
	/// The deliberate exception to the rule, asserted so that nobody "fixes" it into a guard.
	/// </summary>
	/// <remarks>
	/// The clock is an optional parameter that falls back to <see cref="TimeProvider.System"/>;
	/// passing nothing is the supported way to construct it.
	/// </remarks>
	[TestMethod]
	public void TimeAuditedInterceptorShouldAcceptANullClock()
	{
		TimeAuditedInterceptor interceptor = new(null);

		Assert.IsNotNull(interceptor);
	}

	private static void AssertRejects(string parameterName, Func<object> construct)
	{
		ArgumentNullException exception = Assert.ThrowsExactly<ArgumentNullException>(() => construct());

		Assert.AreEqual(parameterName, exception.ParamName);
	}
}
