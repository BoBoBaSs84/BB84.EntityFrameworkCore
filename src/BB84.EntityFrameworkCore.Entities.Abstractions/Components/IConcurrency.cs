// Copyright: 2024 Robert Peter Meyer
// License: MIT
//
// This source code is licensed under the MIT license found in the
// LICENSE file in the root directory of this source tree.
namespace BB84.EntityFrameworkCore.Entities.Abstractions.Components;

/// <summary>
/// Defines a contract for entities that support concurrency control through a store generated token.
/// </summary>
/// <remarks>
/// Implementations of this interface typically use the <see cref="Timestamp"/> property to manage
/// concurrency by ensuring that updates to an entity are based on the most recent version.
/// This is commonly used in scenarios such as optimistic concurrency control in databases.
/// </remarks>
/// <typeparam name="TToken">The type of the concurrency token.</typeparam>
public interface IConcurrency<TToken>
{
	/// <summary>
	/// Gets or sets the concurrency token associated with the current entity.
	/// </summary>
	/// <remarks>
	/// <para>
	/// The setter exists for the disconnected update scenario the token is there for: an entity
	/// is read, mapped to a data transfer object, sent over the wire and posted back. Assign the
	/// original value onto the reconstructed entity before updating it, so that it reaches the
	/// <c>WHERE</c> predicate of the update statement and a concurrent modification raises a
	/// <see href="https://learn.microsoft.com/dotnet/api/microsoft.entityframeworkcore.dbupdateconcurrencyexception">
	/// DbUpdateConcurrencyException</see>. Leaving it unset means the update is not guarded.
	/// </para>
	/// <para>
	/// The value is store generated, so there is no reason to assign it on a newly created
	/// entity, and assigning anything other than a previously read value only makes the update
	/// fail to match.
	/// </para>
	/// <para>
	/// The token type is the provider's business: <see cref="byte"/> array for a SQL Server
	/// <c>rowversion</c>, <see cref="uint"/> for a PostgreSQL <c>xmin</c>, or a <see cref="Guid"/>
	/// or incrementing <see cref="int"/> rotated by the application. Whatever is chosen, the store
	/// has to be told to generate it — see the entity type configuration base classes of the
	/// <c>BB84.EntityFrameworkCore.Repositories</c> package.
	/// </para>
	/// </remarks>
	TToken Timestamp { get; set; }
}

/// <summary>
/// Defines a contract for entities that support concurrency control through a store generated token.
/// </summary>
/// <remarks>
/// The token is a <see cref="byte"/> array, shaped for SQL Server <c>rowversion</c>. Providers
/// without an eight byte row version should name their own token type through
/// <see cref="IConcurrency{TToken}"/> rather than mapping something else onto this one.
/// </remarks>
public interface IConcurrency : IConcurrency<byte[]>;
