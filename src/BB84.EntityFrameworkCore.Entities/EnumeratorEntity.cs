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
/// <typeparam name="TToken">The type of the concurrency token.</typeparam>
public abstract class EnumeratorEntity<TKey, TToken> : IEnumeratorEntity<TKey, TToken>
	where TKey : IEquatable<TKey>
{
	/// <inheritdoc/>
	public TKey Id { get; set; } = default!;

	/// <inheritdoc/>
	public TToken Timestamp { get; set; } = default!;

	/// <inheritdoc/>
	public required string Name { get; set; }

	/// <inheritdoc/>
	public string? Description { get; set; }

	/// <inheritdoc/>
	public bool IsDeleted { get; set; }
}

/// <inheritdoc cref="EnumeratorEntity{TKey, TToken}"/>
/// <remarks>
/// <typeparamref name="TKey"/> is supplied; <c>TToken</c> defaults to a <see cref="byte"/> array.
/// For a custom token type use <see cref="EnumeratorEntity{TKey, TToken}"/> and name both.
/// </remarks>
public abstract class EnumeratorEntity<TKey> : EnumeratorEntity<TKey, byte[]>, IEnumeratorEntity<TKey>
	where TKey : IEquatable<TKey>
{
	/// <summary>
	/// Initializes a new instance of the <see cref="EnumeratorEntity{TKey}"/> class.
	/// </summary>
	/// <inheritdoc cref="IdentityEntity{TKey}()" path="/remarks"/>
	protected EnumeratorEntity()
		=> Timestamp = [];
}

/// <inheritdoc cref="EnumeratorEntity{TKey, TToken}"/>
/// <remarks>
/// The unique identifier type defaults to <see cref="int"/> and the token to a
/// <see cref="byte"/> array.
/// </remarks>
public abstract class EnumeratorEntity : EnumeratorEntity<int>, IEnumeratorEntity;
