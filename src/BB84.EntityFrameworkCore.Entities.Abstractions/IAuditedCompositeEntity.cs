// Copyright: 2024 Robert Peter Meyer
// License: MIT
//
// This source code is licensed under the MIT license found in the
// LICENSE file in the root directory of this source tree.
using BB84.EntityFrameworkCore.Entities.Abstractions.Components;

namespace BB84.EntityFrameworkCore.Entities.Abstractions;

/// <summary>
/// Represents an entity contract that supports auditing for creation and modification,
/// with additional concurrency control mechanisms.
/// </summary>
/// <typeparam name="TCreator">The type of the user or entity responsible for creating the entity.</typeparam>
/// <typeparam name="TEditor">The type of the user or entity responsible for editing or modifying the entity.</typeparam>
public interface IAuditedCompositeEntity<TCreator, TEditor> : IConcurrency, IUserAudited<TCreator, TEditor>
	where TCreator : notnull
{ }

/// <inheritdoc cref="IAuditedCompositeEntity{TCreator, TEditor}"/>
/// <remarks>
/// The creator and editor types default to <see cref="string"/>.
/// </remarks>
public interface IAuditedCompositeEntity : IAuditedCompositeEntity<string, string?>
{ }
