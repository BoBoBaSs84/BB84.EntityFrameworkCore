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
/// into a single cohesive unit. It extends the <see cref="IConcurrency{TToken}"/> interface, indicating
/// that implementations may also support concurrency-related operations.
/// </remarks>
/// <typeparam name="TToken">The type of the concurrency token.</typeparam>
public interface ICompositeEntity<TToken> : IConcurrency<TToken>;

/// <inheritdoc cref="ICompositeEntity{TToken}"/>
/// <remarks>
/// The token is a <see cref="byte"/> array. For a custom token type use
/// <see cref="ICompositeEntity{TToken}"/> and name it.
/// </remarks>
public interface ICompositeEntity : ICompositeEntity<byte[]>, IConcurrency;
