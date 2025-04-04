// Copyright: 2024 Robert Peter Meyer
// License: MIT
//
// This source code is licensed under the MIT license found in the
// LICENSE file in the root directory of this source tree.
namespace BB84.EntityFrameworkCore.Entities.Abstractions.Components;

/// <summary>
/// The interface for concurrency based components.
/// </summary>
public interface IConcurrency
{
	/// <summary>
	/// The current timestamp or row version of the data row.
	/// </summary>
	byte[] Timestamp { get; }
}
