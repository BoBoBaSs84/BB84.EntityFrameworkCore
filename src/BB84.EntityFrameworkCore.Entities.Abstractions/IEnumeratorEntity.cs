// Copyright: 2024 Robert Peter Meyer
// License: MIT
//
// This source code is licensed under the MIT license found in the
// LICENSE file in the root directory of this source tree.
using BB84.EntityFrameworkCore.Entities.Abstractions.Components;

namespace BB84.EntityFrameworkCore.Entities.Abstractions;

/// <summary>
/// The interface for the enumerator models.
/// </summary>
/// <inheritdoc cref="IIdentityEntity{TKey}"/>
/// <inheritdoc cref="IEnumerator"/>
/// <inheritdoc cref="ISoftDeletable"/>
public interface IEnumeratorEntity<TKey> : IIdentityEntity<TKey>, IEnumerator, ISoftDeletable where TKey : IEquatable<TKey>
{ }

/// <inheritdoc cref="IEnumeratorEntity{TKey}"/>
/// <remarks>
/// The identity column is of type <see cref="int"/>.
/// </remarks>
public interface IEnumeratorEntity : IEnumeratorEntity<int>
{ }
