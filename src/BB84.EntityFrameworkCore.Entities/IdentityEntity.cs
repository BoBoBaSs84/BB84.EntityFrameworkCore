// Copyright: 2024 Robert Peter Meyer
// License: MIT
//
// This source code is licensed under the MIT license found in the
// LICENSE file in the root directory of this source tree.
using BB84.EntityFrameworkCore.Entities.Abstractions;

namespace BB84.EntityFrameworkCore.Entities;

/// <summary>
/// The base implementation for the identity models.
/// </summary>
/// <inheritdoc cref="IIdentityEntity{TKey}"/>
public abstract class IdentityEntity<TKey> : IIdentityEntity<TKey> where TKey : IEquatable<TKey>
{
	/// <inheritdoc/>
	public TKey Id { get; set; } = default!;

	/// <inheritdoc/>
	public byte[] Timestamp { get; } = default!;
}

/// <inheritdoc cref="IdentityEntity{TKey}"/>
/// <inheritdoc cref="IIdentityEntity"/>
public abstract class IdentityEntity : IdentityEntity<Guid>, IIdentityEntity
{ }
