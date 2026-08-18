// Copyright: 2024 Robert Peter Meyer
// License: MIT
//
// This source code is licensed under the MIT license found in the
// LICENSE file in the root directory of this source tree.
using BB84.EntityFrameworkCore.Entities.Abstractions;

namespace BB84.EntityFrameworkCore.Entities;

/// <summary>
/// This abstract class provides a base implementation for entities that require a unique
/// identifier and a concurrency token.
/// </summary>
/// <typeparam name="TKey">The type of the unique identifier for the entity.</typeparam>
/// <typeparam name="TToken">The type of the concurrency token.</typeparam>
public abstract class IdentityEntity<TKey, TToken> : IIdentityEntity<TKey, TToken>
	where TKey : IEquatable<TKey>
{
	/// <inheritdoc/>
	public TKey Id { get; set; } = default!;

	/// <inheritdoc/>
	public TToken Timestamp { get; set; } = default!;
}

/// <inheritdoc cref="IdentityEntity{TKey, TToken}"/>
/// <remarks>
/// <typeparamref name="TKey"/> is supplied; <c>TToken</c> defaults to a <see cref="byte"/> array.
/// For a custom token type use <see cref="IdentityEntity{TKey, TToken}"/> and name both.
/// </remarks>
public abstract class IdentityEntity<TKey> : IdentityEntity<TKey, byte[]>, IIdentityEntity<TKey>
	where TKey : IEquatable<TKey>
{
	/// <summary>
	/// Initializes a new instance of the <see cref="IdentityEntity{TKey}"/> class.
	/// </summary>
	/// <remarks>
	/// The token starts out as an empty array rather than <see langword="null"/>. The generic base
	/// cannot express that for an arbitrary token type, so the <see cref="byte"/> array rungs
	/// restore it here — the property was never null before the token type became a parameter.
	/// </remarks>
	protected IdentityEntity()
		=> Timestamp = [];
}

/// <inheritdoc cref="IdentityEntity{TKey, TToken}"/>
/// <remarks>
/// The unique identifier is of type <see cref="Guid"/> and the token a <see cref="byte"/> array.
/// </remarks>
public abstract class IdentityEntity : IdentityEntity<Guid>, IIdentityEntity;
