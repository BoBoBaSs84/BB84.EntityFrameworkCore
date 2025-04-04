// Copyright: 2024 Robert Peter Meyer
// License: MIT
//
// This source code is licensed under the MIT license found in the
// LICENSE file in the root directory of this source tree.
namespace BB84.EntityFrameworkCore.Entities.Abstractions.Components;

/// <summary>
/// The interface for soft deleteable components.
/// </summary>
public interface ISoftDeletable
{
	/// <summary>
	/// Indicates if the data row in a soft deleted state.
	/// </summary>
	bool IsDeleted { get; set; }
}
