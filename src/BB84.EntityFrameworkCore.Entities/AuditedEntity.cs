// Copyright: 2024 Robert Peter Meyer
// License: MIT
//
// This source code is licensed under the MIT license found in the
// LICENSE file in the root directory of this source tree.
using BB84.EntityFrameworkCore.Entities.Abstractions;

namespace BB84.EntityFrameworkCore.Entities;

/// <summary>
/// This abstract class provides a base implementation for entities that track auditing information,
/// including the creator and the last editor, a unique identifier and a timestamp for concurrency
/// control.
/// </summary>
/// <typeparam name="TKey">The type of the unique identifier for the entity.</typeparam>
/// <typeparam name="TCreator">The type representing the creator of the entity.</typeparam>
/// <typeparam name="TEdited">The type representing the last editor of the entity.</typeparam>
public abstract class AuditedEntity<TKey, TCreator, TEdited> : IdentityEntity<TKey>, IAuditedEntity<TKey, TCreator, TEdited>
	where TKey : IEquatable<TKey>
	where TCreator : notnull
{
	/// <inheritdoc/>
	public TCreator CreatedBy { get; set; } = default!;

	/// <inheritdoc/>
	public TEdited EditedBy { get; set; } = default!;
}

/// <inheritdoc cref="AuditedEntity{TKey, TCreator, TEdited}"/>
/// <remarks>
/// The creator and editor types default to <see cref="string"/>.
/// </remarks>
public abstract class AuditedEntity<TKey> : AuditedEntity<TKey, string, string?>, IAuditedEntity<TKey>
	where TKey : IEquatable<TKey>
{ }

/// <inheritdoc cref="AuditedEntity{TKey, TCreator, TEdited}"/>
/// <remarks>
/// The unique identifier is of type <see cref="Guid"/>.
/// </remarks>
public abstract class AuditedEntity<TCreator, TEdited> : AuditedEntity<Guid, TCreator, TEdited>, IAuditedEntity<TCreator, TEdited>
	where TCreator : notnull
{ }

/// <inheritdoc cref="AuditedEntity{TKey, TCreator, TEdited}"/>
/// <remarks>
/// The unique identifier type defaults to <see cref="Guid"/> and the creator and editor
/// types default to <see cref="string"/>.
/// </remarks>
public abstract class AuditedEntity : AuditedEntity<Guid, string, string?>, IAuditedEntity
{ }
