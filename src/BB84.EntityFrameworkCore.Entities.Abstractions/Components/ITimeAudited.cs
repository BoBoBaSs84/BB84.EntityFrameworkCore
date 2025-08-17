// Copyright: 2024 Robert Peter Meyer
// License: MIT
//
// This source code is licensed under the MIT license found in the
// LICENSE file in the root directory of this source tree.
namespace BB84.EntityFrameworkCore.Entities.Abstractions.Components;

/// <summary>
/// Defines a contract for entities that track creation and modification timestamps.
/// </summary>
/// <remarks>
/// This interface is typically implemented by data models that require auditing of
/// their creation and last modification times. The <see cref="CreatedAt"/> property
/// records the initial creation timestamp, while the <see cref="EditedAt"/> property
/// tracks the most recent modification timestamp, if applicable.
/// </remarks>
public interface ITimeAudited
{
	/// <summary>
	/// Gets or sets the date and time when the entity was created.
	/// </summary>
	DateTime CreatedAt { get; set; }

	/// <summary>
	/// Gets or sets the date and time when the item was last edited.
	/// </summary>
	DateTime? EditedAt { get; set; }
}
