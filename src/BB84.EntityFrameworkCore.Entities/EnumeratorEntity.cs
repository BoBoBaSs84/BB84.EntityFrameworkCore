// Copyright: 2024 Robert Peter Meyer
// License: MIT
//
// This source code is licensed under the MIT license found in the
// LICENSE file in the root directory of this source tree.
using BB84.EntityFrameworkCore.Entities.Abstractions;

namespace BB84.EntityFrameworkCore.Entities;

/// <summary>
/// Represents an abstract base class for entities that provides properties for the name
/// and description, with a unique identifier of type <typeparamref name="TKey"/> and the
/// support for soft deletion functionality.
/// </summary>
/// <typeparam name="TKey">The type of the unique identifier for the entity.</typeparam>
public abstract class EnumeratorEntity<TKey> : IEnumeratorEntity<TKey>
	where TKey : IEquatable<TKey>
{
	/// <inheritdoc/>
	public TKey Id { get; set; } = default!;

	/// <inheritdoc/>
	public byte[] Timestamp { get; } = default!;

	/// <inheritdoc/>
	public required string Name { get; set; }

	/// <inheritdoc/>
	public string? Description { get; set; }

	/// <inheritdoc/>
	public bool IsDeleted { get; set; }
}

/// <inheritdoc cref="EnumeratorEntity{TKey}"/>
/// <remarks>
/// The unique identifier type defaults to <see cref="int"/>.
/// </remarks>
public abstract class EnumeratorEntity : EnumeratorEntity<int>, IEnumeratorEntity
{ }
