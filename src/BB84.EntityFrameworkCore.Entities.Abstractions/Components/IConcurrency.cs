// Copyright: 2024 Robert Peter Meyer
// License: MIT
//
// This source code is licensed under the MIT license found in the
// LICENSE file in the root directory of this source tree.
namespace BB84.EntityFrameworkCore.Entities.Abstractions.Components;

/// <summary>
/// Defines a contract for entities that support concurrency control through a timestamp.
/// </summary>
/// <remarks>
/// Implementations of this interface typically use the <see cref="Timestamp"/> property to manage
/// concurrency by ensuring that updates to an entity are based on the most recent version.
/// This is commonly used in scenarios such as optimistic concurrency control in databases.
/// </remarks>
public interface IConcurrency
{
	/// <summary>
	/// Gets the timestamp associated with the current entity.
	/// </summary>
	byte[] Timestamp { get; }
}
