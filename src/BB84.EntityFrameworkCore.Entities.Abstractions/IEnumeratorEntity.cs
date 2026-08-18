// Copyright: 2024 Robert Peter Meyer
// License: MIT
//
// This source code is licensed under the MIT license found in the
// LICENSE file in the root directory of this source tree.
using BB84.EntityFrameworkCore.Entities.Abstractions.Components;

namespace BB84.EntityFrameworkCore.Entities.Abstractions;

/// <summary>
/// Represents an entity contract that provides properties for the name and description,
/// with a unique identifier of type <typeparamref name="TKey"/> and the support for
/// soft deletion functionality.
/// </summary>
/// <typeparam name="TKey">The type of the unique identifier for the entity.</typeparam>
/// <typeparam name="TToken">The type of the concurrency token.</typeparam>
public interface IEnumeratorEntity<TKey, TToken> : IIdentityEntity<TKey, TToken>, IEnumeration, ISoftDeletable
	where TKey : IEquatable<TKey>;

/// <inheritdoc cref="IEnumeratorEntity{TKey, TToken}"/>
/// <remarks>
/// <typeparamref name="TKey"/> is supplied; <c>TToken</c> defaults to a <see cref="byte"/> array.
/// For a custom token type use <see cref="IEnumeratorEntity{TKey, TToken}"/> and name both.
/// </remarks>
public interface IEnumeratorEntity<TKey> : IEnumeratorEntity<TKey, byte[]>, IIdentityEntity<TKey>
	where TKey : IEquatable<TKey>;

/// <inheritdoc cref="IEnumeratorEntity{TKey, TToken}"/>
/// <remarks>
/// The unique identifier is of type <see cref="int"/> and the token a <see cref="byte"/> array.
/// </remarks>
public interface IEnumeratorEntity : IEnumeratorEntity<int>;
