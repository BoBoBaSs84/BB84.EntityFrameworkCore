// Copyright: 2024 Robert Peter Meyer
// License: MIT
//
// This source code is licensed under the MIT license found in the
// LICENSE file in the root directory of this source tree.
namespace BB84.EntityFrameworkCore.Entities.Abstractions.Components;

/// <summary>
/// Represents an entity that tracks audit information related to creation and modification times.
/// </summary>
/// <remarks>
/// This interface is typically implemented by data models that require auditing of their creation
/// and last modification timestamps. The <see cref="Created"/> property records the initial
/// creation time, while the <see cref="Edited"/> property tracks the most recent modification time.
/// </remarks>
public interface ITimeAudited
{
	/// <summary>
	/// Gets or sets the date and time when the entity was created.
	/// </summary>
	DateTime Created { get; set; }

	/// <summary>
	/// Gets or sets the date and time when the entity was last edited.
	/// </summary>
	DateTime? Edited { get; set; }
}
