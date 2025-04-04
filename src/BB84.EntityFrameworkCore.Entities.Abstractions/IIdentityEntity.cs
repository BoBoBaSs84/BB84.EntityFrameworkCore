// Copyright: 2024 Robert Peter Meyer
// License: MIT
//
// This source code is licensed under the MIT license found in the
// LICENSE file in the root directory of this source tree.
using BB84.EntityFrameworkCore.Entities.Abstractions.Components;

namespace BB84.EntityFrameworkCore.Entities.Abstractions;

/// <summary>
/// The interface for the identity models.
/// </summary>
/// <inheritdoc cref="IIdentity{TKey}"/>
/// <inheritdoc cref="IConcurrency"/>
public interface IIdentityEntity<TKey> : IIdentity<TKey>, IConcurrency where TKey : IEquatable<TKey>
{ }

/// <inheritdoc cref="IIdentityEntity{TKey}"/>
/// <remarks>
/// The identity column is of type <see cref="Guid"/>.
/// </remarks>
public interface IIdentityEntity : IIdentityEntity<Guid>
{ }
