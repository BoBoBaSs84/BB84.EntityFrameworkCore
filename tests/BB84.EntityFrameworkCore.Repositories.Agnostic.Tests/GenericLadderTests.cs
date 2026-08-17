// Copyright: 2024 Robert Peter Meyer
// License: MIT
//
// This source code is licensed under the MIT license found in the
// LICENSE file in the root directory of this source tree.
using System.Reflection;

using BB84.EntityFrameworkCore.Entities;
using BB84.EntityFrameworkCore.Repositories.Abstractions;
using BB84.EntityFrameworkCore.Repositories.Configurations;

namespace BB84.EntityFrameworkCore.Repositories.Agnostic.Tests;

/// <summary>
/// Binds every rung of every generic ladder, so that removing one breaks the build.
/// </summary>
/// <remarks>
/// <para>
/// The entity and configuration abstractions are ladders of progressively defaulted aliases. Most
/// rungs are referenced by nothing: when the ambiguous two-argument audit rungs were removed, the
/// solution still built and all 112 tests still passed, because no test named them. Public API
/// disappeared and CI had nothing to say about it.
/// </para>
/// <para>
/// Every probe below exists purely to name a rung. They assert nothing individually — the
/// compiler is the assertion, and removing a rung fails the build here. The reflection tests
/// catch the opposite direction: a rung added as a <i>sibling</i> of the existing ones, with no
/// probe naming it. A rung inserted into an existing chain is not caught, since the probes
/// already derive from it transitively.
/// </para>
/// </remarks>
[TestClass]
public sealed class GenericLadderTests
{
	[TestMethod]
	public void EveryEntityRungShouldBeBound()
		=> AssertEveryRungIsBound(typeof(AuditedEntity).Assembly, "BB84.EntityFrameworkCore.Entities");

	[TestMethod]
	public void EveryConfigurationRungShouldBeBound()
		=> AssertEveryRungIsBound(typeof(AuditedConfiguration<>).Assembly, "BB84.EntityFrameworkCore.Repositories.Configurations");

	[TestMethod]
	public void EveryRepositoryRungShouldBeBound()
		=> AssertEveryRungIsBound(typeof(GenericRepository<>).Assembly, "BB84.EntityFrameworkCore.Repositories");

	/// <summary>
	/// Asserts that every public abstract type declared directly in <paramref name="namespace"/>
	/// has at least one type in this assembly deriving from it.
	/// </summary>
	private static void AssertEveryRungIsBound(Assembly assembly, string @namespace)
	{
		Type[] rungs = [.. assembly.GetExportedTypes()
			.Where(x => x.IsAbstract && x.IsClass && x.Namespace == @namespace)];

		Assert.IsNotEmpty(rungs, "Nothing was reflected over, so the check below proves nothing.");

		Type[] probes = [.. typeof(GenericLadderTests).Assembly.GetTypes()];

		string[] unbound = [.. rungs
			.Where(rung => !probes.Any(probe => DerivesFrom(probe, rung)))
			.Select(x => x.Name)
			.Order()];

		Assert.IsEmpty(unbound, $"Unbound rungs, add a probe for each: {string.Join(", ", unbound)}");
	}

	private static bool DerivesFrom(Type probe, Type rung)
	{
		for (Type? current = probe.BaseType; current is not null; current = current.BaseType)
		{
			if (current == rung || (current.IsGenericType && current.GetGenericTypeDefinition() == rung))
				return true;
		}

		return false;
	}

	// ---- entity ladder -------------------------------------------------------------------

	private sealed class CompositeProbe : CompositeEntity;
	private sealed class IdentityKeyedProbe : IdentityEntity<int>;
	private sealed class IdentityProbe : IdentityEntity;
	private sealed class EnumeratorKeyedProbe : EnumeratorEntity<long>;
	private sealed class EnumeratorProbe : EnumeratorEntity;
	private sealed class AuditedFullyNamedProbe : AuditedEntity<int, int, int?>;
	private sealed class AuditedKeyedProbe : AuditedEntity<int>;
	private sealed class AuditedProbe : AuditedEntity;
	private sealed class FullAuditedFullyNamedProbe : FullAuditedEntity<int, int, int?>;
	private sealed class FullAuditedKeyedProbe : FullAuditedEntity<int>;
	private sealed class FullAuditedProbe : FullAuditedEntity;
	private sealed class AuditedCompositeNamedProbe : AuditedCompositeEntity<int, int?>;
	private sealed class AuditedCompositeProbe : AuditedCompositeEntity;

	// ---- configuration ladder ------------------------------------------------------------

	private abstract class IdentityKeyedConfigurationProbe : IdentityConfiguration<IdentityKeyedProbe, int>;
	private abstract class IdentityConfigurationProbe : IdentityConfiguration<IdentityProbe>;
	private abstract class CompositeConfigurationProbe : CompositeConfiguration<CompositeProbe>;
	private abstract class EnumeratorKeyedConfigurationProbe : EnumeratorConfiguration<EnumeratorKeyedProbe, long>;
	private abstract class EnumeratorConfigurationProbe : EnumeratorConfiguration<EnumeratorProbe>;
	private abstract class AuditedFullyNamedConfigurationProbe : AuditedConfiguration<AuditedFullyNamedProbe, int, int, int?>;
	private abstract class AuditedKeyedConfigurationProbe : AuditedConfiguration<AuditedKeyedProbe, int>;
	private abstract class AuditedConfigurationProbe : AuditedConfiguration<AuditedProbe>;
	private abstract class FullAuditedFullyNamedConfigurationProbe : FullAuditedConfiguration<FullAuditedFullyNamedProbe, int, int, int?>;
	private abstract class FullAuditedKeyedConfigurationProbe : FullAuditedConfiguration<FullAuditedKeyedProbe, int>;
	private abstract class FullAuditedConfigurationProbe : FullAuditedConfiguration<FullAuditedProbe>;
	private abstract class AuditedCompositeNamedConfigurationProbe : AuditedCompositeConfiguration<AuditedCompositeNamedProbe, int, int?>;
	private abstract class AuditedCompositeConfigurationProbe : AuditedCompositeConfiguration<AuditedCompositeProbe>;

	// ---- repository ladder ---------------------------------------------------------------

	private sealed class GenericRepositoryProbe(IDbContext context) : GenericRepository<CompositeProbe>(context);
	private sealed class IdentityKeyedRepositoryProbe(IDbContext context) : IdentityRepository<IdentityKeyedProbe, int>(context);
	private sealed class IdentityRepositoryProbe(IDbContext context) : IdentityRepository<IdentityProbe>(context);
	private sealed class EnumeratorKeyedRepositoryProbe(IDbContext context) : EnumeratorRepository<EnumeratorKeyedProbe, long>(context);
	private sealed class EnumeratorRepositoryProbe(IDbContext context) : EnumeratorRepository<EnumeratorProbe>(context);
}
