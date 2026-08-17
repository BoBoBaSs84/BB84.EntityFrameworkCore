// Copyright: 2024 Robert Peter Meyer
// License: MIT
//
// This source code is licensed under the MIT license found in the
// LICENSE file in the root directory of this source tree.
namespace BB84.EntityFrameworkCore.Entities.Abstractions.Components;

/// <summary>
/// Defines a contract for entities that support concurrency control through a timestamp.
/// </summary>
/// <remarks>
/// Implementations of this interface typically use the <see cref="Timestamp"/> property to manage
/// concurrency by ensuring that updates to an entity are based on the most recent version.
/// This is commonly used in scenarios such as optimistic concurrency control in databases.
/// </remarks>
public interface IConcurrency
{
	/// <summary>
	/// Gets or sets the timestamp associated with the current entity.
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
	/// The token is shaped for SQL Server <c>rowversion</c>. Providers without an eight byte row
	/// version need their own mapping for this property.
	/// </para>
	/// </remarks>
	byte[] Timestamp { get; set; }
}
