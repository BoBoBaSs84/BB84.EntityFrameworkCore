// Copyright: 2024 Robert Peter Meyer
// License: MIT
//
// This source code is licensed under the MIT license found in the
// LICENSE file in the root directory of this source tree.
using BB84.EntityFrameworkCore.Entities.Abstractions;

namespace BB84.EntityFrameworkCore.Entities;

/// <summary>
/// This abstract class provides a base implementation for entities that provides properties
/// to track the creator, creation time, editor, and last modification time of the entity.
/// </summary>
/// <typeparam name="TKey">The type of the unique identifier for the entity.</typeparam>
/// <typeparam name="TCreator">The type representing the creator of the entity.</typeparam>
/// <typeparam name="TEdited">The type representing the editor of the entity.</typeparam>
public abstract class FullAuditedEntity<TKey, TCreator, TEdited> : IdentityEntity<TKey>, IFullAuditedEntity<TKey, TCreator, TEdited>
	where TKey : IEquatable<TKey>
	where TCreator : notnull
{
	/// <inheritdoc/>
	public TCreator CreatedBy { get; set; } = default!;
	/// <inheritdoc/>
	public DateTime CreatedAt { get; set; } = default!;
	/// <inheritdoc/>
	public TEdited EditedBy { get; set; } = default!;
	/// <inheritdoc/>
	public DateTime? EditedAt { get; set; } = default!;
}

/// <inheritdoc cref="FullAuditedEntity{TKey, TCreator, TEdited}"/>
/// <remarks>
/// The creator and editor columns are of type <see cref="string"/>.
/// </remarks>
public abstract class FullAuditedEntity<TKey> : FullAuditedEntity<TKey, string, string?>, IFullAuditedEntity<TKey>
	where TKey : IEquatable<TKey>
{ }


/// <inheritdoc cref="FullAuditedEntity{TKey, TCreator, TEdited}"/>
/// <remarks>
/// The identity column is of type <see cref="Guid"/>.
/// </remarks>
public abstract class FullAuditedEntity<TCreator, TEdited> : FullAuditedEntity<Guid, TCreator, TEdited>, IFullAuditedEntity<TCreator, TEdited>
	where TCreator : notnull
{ }

/// <inheritdoc cref="FullAuditedEntity{TKey, TCreator, TEdited}"/>
public abstract class FullAuditedEntity : FullAuditedEntity<Guid, string, string?>, IFullAuditedEntity
{ }
