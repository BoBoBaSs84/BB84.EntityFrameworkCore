// Copyright: 2024 Robert Peter Meyer
// License: MIT
//
// This source code is licensed under the MIT license found in the
// LICENSE file in the root directory of this source tree.
namespace BB84.EntityFrameworkCore.Entities.Abstractions.Components;

/// <summary>
/// Represents an entity with a unique identifier.
/// </summary>
/// <typeparam name="TKey">The type of the unique identifier.</typeparam>
public interface IIdentity<TKey> where TKey : IEquatable<TKey>
{
	/// <summary>
	/// Gets or sets the unique identifier for the entity.
	/// </summary>
	TKey Id { get; set; }
}

/// <summary>
/// Gets or sets the unique identifier for the entity.
/// The identifier is of type <see cref="Guid"/>.
/// </summary>
public interface IIdentity : IIdentity<Guid>
{ }
