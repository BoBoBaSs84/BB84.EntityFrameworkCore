// Copyright: 2024 Robert Peter Meyer
// License: MIT
//
// This source code is licensed under the MIT license found in the
// LICENSE file in the root directory of this source tree.
namespace BB84.EntityFrameworkCore.Entities.Abstractions.Components;

/// <summary>
/// Represents an interface for managing concurrency in data operations.
/// </summary>
/// <remarks>
/// This interface is typically used to handle concurrency control in scenarios where data rows
/// are updated or modified. The <see cref="Timestamp"/> property provides a mechanism for
/// detecting changes to a data row, ensuring that updates are applied only to the intended version.
/// </remarks>
public interface IConcurrency
{
	/// <summary>
	/// Gets the timestamp value that represents the version of the entity.
	/// </summary>
	byte[] Timestamp { get; }
}
