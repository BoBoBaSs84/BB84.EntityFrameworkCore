// Copyright: 2024 Robert Peter Meyer
// License: MIT
//
// This source code is licensed under the MIT license found in the
// LICENSE file in the root directory of this source tree.
using BB84.EntityFrameworkCore.Entities.Abstractions;

namespace BB84.EntityFrameworkCore.Entities;

/// <summary>
/// This abstract class provides a base implementation for entities entities that are
/// composed of multiple related components.
/// </summary>
public abstract class CompositeEntity : ICompositeEntity
{
	/// <inheritdoc/>
	public byte[] Timestamp { get; } = default!;
}
