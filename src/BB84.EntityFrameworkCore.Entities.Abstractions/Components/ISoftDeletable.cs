// Copyright: 2024 Robert Peter Meyer
// License: MIT
//
// This source code is licensed under the MIT license found in the
// LICENSE file in the root directory of this source tree.
namespace BB84.EntityFrameworkCore.Entities.Abstractions.Components;

/// <summary>
/// Defines a contract for entities that support soft deletion functionality.
/// </summary>
/// <remarks>
/// Soft deletion allows an entity to be marked as deleted without being permanently
/// removed from the data store. This is typically used to enable logical deletion
/// while preserving the entity for auditing or recovery purposes.
/// </remarks>
public interface ISoftDeletable
{
	/// <summary>
	/// Gets or sets a value indicating whether the entity is marked as deleted.
	/// </summary>
	bool IsDeleted { get; set; }
}
