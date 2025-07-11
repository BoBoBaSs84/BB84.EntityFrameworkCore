// Copyright: 2024 Robert Peter Meyer
// License: MIT
//
// This source code is licensed under the MIT license found in the
// LICENSE file in the root directory of this source tree.
using BB84.EntityFrameworkCore.Entities.Abstractions;

namespace BB84.EntityFrameworkCore.Entities;

/// <summary>
/// This abstract class provides a base implementation for entities that require a unique
/// identifier and a timestamp for concurrency control.
/// </summary>
/// <typeparam name="TKey">The type of the unique identifier for the entity.</typeparam>
public abstract class IdentityEntity<TKey> : IIdentityEntity<TKey>
	where TKey : IEquatable<TKey>
{
	/// <inheritdoc/>
	public TKey Id { get; set; } = default!;

	/// <inheritdoc/>
	public byte[] Timestamp { get; } = default!;
}

/// <inheritdoc cref="IdentityEntity{TKey}"/>
/// <remarks>
/// The unique identifier is of type <see cref="Guid"/>.
/// </remarks>
public abstract class IdentityEntity : IdentityEntity<Guid>, IIdentityEntity
{ }
