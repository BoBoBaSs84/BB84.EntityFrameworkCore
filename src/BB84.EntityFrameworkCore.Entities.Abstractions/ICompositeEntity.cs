// Copyright: 2024 Robert Peter Meyer
// License: MIT
//
// This source code is licensed under the MIT license found in the
// LICENSE file in the root directory of this source tree.
using BB84.EntityFrameworkCore.Entities.Abstractions.Components;

namespace BB84.EntityFrameworkCore.Entities.Abstractions;

/// <summary>
/// Represents an entity contract that is composed of multiple related components or sub-entities.
/// </summary>
/// <remarks>
/// This interface is typically implemented by entities that aggregate other entities or components
/// into a single cohesive unit. It extends the <see cref="IConcurrency"/> interface, indicating that
/// implementations may also support concurrency-related operations.
/// </remarks>
public interface ICompositeEntity : IConcurrency
{ }
