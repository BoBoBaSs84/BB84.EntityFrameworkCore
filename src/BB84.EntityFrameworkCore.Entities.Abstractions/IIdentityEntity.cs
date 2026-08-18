// Copyright: 2024 Robert Peter Meyer
// License: MIT
//
// This source code is licensed under the MIT license found in the
// LICENSE file in the root directory of this source tree.
using BB84.EntityFrameworkCore.Entities.Abstractions.Components;

namespace BB84.EntityFrameworkCore.Entities.Abstractions;

/// <summary>
/// Represents an entity contract with a unique identifier and concurrency control.
/// </summary>
/// <typeparam name="TKey">The type of the unique identifier for the entity.</typeparam>
/// <typeparam name="TToken">The type of the concurrency token.</typeparam>
public interface IIdentityEntity<TKey, TToken> : IIdentity<TKey>, IConcurrency<TToken>
	where TKey : IEquatable<TKey>;

/// <inheritdoc cref="IIdentityEntity{TKey, TToken}"/>
/// <remarks>
/// <typeparamref name="TKey"/> is supplied; <c>TToken</c> defaults to a <see cref="byte"/> array.
/// For a custom token type use <see cref="IIdentityEntity{TKey, TToken}"/> and name both.
/// </remarks>
public interface IIdentityEntity<TKey> : IIdentityEntity<TKey, byte[]>, IConcurrency
	where TKey : IEquatable<TKey>;

/// <inheritdoc cref="IIdentityEntity{TKey, TToken}"/>
/// <remarks>
/// The unique identifier is of type <see cref="Guid"/> and the token a <see cref="byte"/> array.
/// </remarks>
public interface IIdentityEntity : IIdentityEntity<Guid>;
