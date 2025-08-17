// Copyright: 2024 Robert Peter Meyer
// License: MIT
//
// This source code is licensed under the MIT license found in the
// LICENSE file in the root directory of this source tree.
using BB84.EntityFrameworkCore.Entities.Abstractions;

namespace BB84.EntityFrameworkCore.Entities;

/// <summary>
/// This abstract class provides a base implementation for entities that are composed
/// of multiple related components and require auditing information, including the creator,
/// editor, and a concurrency timestamp.
/// </summary>
/// <typeparam name="TCreator">The type of the entity or user responsible for creating the entity.</typeparam>
/// <typeparam name="TEdited">The type of the entity or user responsible for editing the entity.</typeparam>
public abstract class AuditedCompositeEntity<TCreator, TEdited> : IAuditedCompositeEntity<TCreator, TEdited>
	where TCreator : notnull
{
	/// <inheritdoc/>
	public byte[] Timestamp { get; } = default!;

	/// <inheritdoc/>
	public TCreator CreatedBy { get; set; } = default!;

	/// <inheritdoc/>
	public TEdited EditedBy { get; set; } = default!;
}

/// <inheritdoc cref="AuditedCompositeEntity{TCreator, TEdited}"/>
/// <remarks>
/// The creator and editor types default to <see cref="string"/>.
/// </remarks>
public abstract class AuditedCompositeEntity : AuditedCompositeEntity<string, string?>, IAuditedCompositeEntity
{ }
