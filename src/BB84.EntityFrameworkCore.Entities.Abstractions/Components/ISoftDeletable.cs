// Copyright: 2024 Robert Peter Meyer
// License: MIT
//
// This source code is licensed under the MIT license found in the
// LICENSE file in the root directory of this source tree.
namespace BB84.EntityFrameworkCore.Entities.Abstractions.Components;

/// <summary>
/// Represents an entity that supports soft deletion functionality.
/// </summary>
/// <remarks>
/// Soft deletion is a mechanism where an entity is marked as deleted without being permanently
/// removed from the data store. This interface provides a property to track the deletion state.
/// </remarks>
public interface ISoftDeletable
{
	/// <summary>
	/// Gets or sets a value indicating whether the entity is marked as deleted.
	/// </summary>
	bool IsDeleted { get; set; }
}
