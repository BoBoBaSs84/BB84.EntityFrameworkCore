// Copyright: 2024 Robert Peter Meyer
// License: MIT
//
// This source code is licensed under the MIT license found in the
// LICENSE file in the root directory of this source tree.
using BB84.EntityFrameworkCore.Entities;

namespace BB84.EntityFrameworkCore.Repositories.Agnostic.Tests.Persistence.Entities;

/// <summary>
/// An entity whose concurrency token is a <see cref="Guid"/> instead of a row version.
/// </summary>
/// <remarks>
/// <para>
/// This one lives here rather than in the shared test model on purpose: the shared model is used
/// by the SQL Server suite as well, where the token is a <c>rowversion</c> and has to stay one.
/// </para>
/// <para>
/// It is the only entity in either suite that does not take the token type the base classes
/// default to, which is the whole reason it exists — without it nothing would fail if the token
/// type quietly went back to being fixed.
/// </para>
/// </remarks>
internal sealed class TokenTypeEntity : IdentityEntity<Guid, Guid>
{
	/// <summary>
	/// The name of the entity.
	/// </summary>
	public required string Name { get; set; }
}
