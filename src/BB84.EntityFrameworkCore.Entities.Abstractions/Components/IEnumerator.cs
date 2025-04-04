// Copyright: 2024 Robert Peter Meyer
// License: MIT
//
// This source code is licensed under the MIT license found in the
// LICENSE file in the root directory of this source tree.
namespace BB84.EntityFrameworkCore.Entities.Abstractions.Components;

/// <summary>
/// The interface for enumerator based components.
/// </summary>
public interface IEnumerator
{
	/// <summary>
	/// The name of the enumerator.
	/// </summary>
	string Name { get; set; }

	/// <summary>
	/// The description of the enumerator.
	/// </summary>
	string? Description { get; set; }
}
