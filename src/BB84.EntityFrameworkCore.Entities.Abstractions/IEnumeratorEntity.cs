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
public interface IEnumeratorEntity<TKey> : IIdentityEntity<TKey>, IEnumerator, ISoftDeletable
	where TKey : IEquatable<TKey>
{ }

/// <inheritdoc cref="IEnumeratorEntity{TKey}"/>
public interface IEnumeratorEntity : IEnumeratorEntity<int>
{ }
