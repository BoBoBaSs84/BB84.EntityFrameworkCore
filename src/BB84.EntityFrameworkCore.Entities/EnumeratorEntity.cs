// Copyright: 2024 Robert Peter Meyer
// License: MIT
//
// This source code is licensed under the MIT license found in the
// LICENSE file in the root directory of this source tree.
using BB84.EntityFrameworkCore.Entities.Abstractions;

namespace BB84.EntityFrameworkCore.Entities;

/// <summary>
/// The base implementation for the enumerator models.
/// </summary>
/// <inheritdoc cref="IEnumeratorEntity{TKey}"/>
public abstract class EnumeratorEntity<TKey> : IEnumeratorEntity<TKey> where TKey : IEquatable<TKey>
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
/// <inheritdoc cref="IEnumeratorEntity"/>
public abstract class EnumeratorEntity : EnumeratorEntity<int>, IEnumeratorEntity
{ }
