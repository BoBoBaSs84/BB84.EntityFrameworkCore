// Copyright: 2024 Robert Peter Meyer
// License: MIT
//
// This source code is licensed under the MIT license found in the
// LICENSE file in the root directory of this source tree.
namespace BB84.EntityFrameworkCore.Entities.Abstractions.Components;

/// <summary>
/// Defines a contract for entities that support enumerator functionality.
/// </summary>
/// <remarks>
/// The <see cref="IEnumerator"/> interface is designed to provide a standardized way to
/// efine enumerators within entities. It includes properties for the name and description
/// so that each enumerator can be clearly identified and documented.
/// </remarks>
public interface IEnumerator
{
	/// <summary>
	/// Gets or sets the name of the enumerator.
	/// </summary>
	string Name { get; set; }

	/// <summary>
	/// Gets or sets the description of the enumerator.
	/// </summary>
	string? Description { get; set; }
}
