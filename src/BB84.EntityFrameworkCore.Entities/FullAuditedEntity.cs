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
/// <typeparam name="TEditor">The type representing the editor of the entity.</typeparam>
public abstract class FullAuditedEntity<TKey, TCreator, TEditor> : IdentityEntity<TKey>, IFullAuditedEntity<TKey, TCreator, TEditor>
	where TKey : IEquatable<TKey>
	where TCreator : notnull
{
	/// <inheritdoc/>
	public TCreator CreatedBy { get; set; } = default!;
	/// <inheritdoc/>
	public DateTimeOffset CreatedAt { get; set; } = default!;
	/// <inheritdoc/>
	public TEditor EditedBy { get; set; } = default!;
	/// <inheritdoc/>
	public DateTimeOffset? EditedAt { get; set; } = default!;
}

/// <inheritdoc cref="FullAuditedEntity{TKey, TCreator, TEditor}"/>
/// <remarks>
/// <typeparamref name="TKey"/> is supplied; <c>TCreator</c> defaults to <see cref="string"/>
/// and <c>TEditor</c> to <see cref="string"/>. For a custom creator or editor type use
/// <see cref="FullAuditedEntity{TKey, TCreator, TEditor}"/> and name all three.
/// </remarks>
public abstract class FullAuditedEntity<TKey> : FullAuditedEntity<TKey, string, string?>, IFullAuditedEntity<TKey>
	where TKey : IEquatable<TKey>
{ }

/// <inheritdoc cref="FullAuditedEntity{TKey, TCreator, TEditor}"/>
/// <remarks>
/// Nothing is supplied; <c>TKey</c> defaults to <see cref="Guid"/>, <c>TCreator</c> to
/// <see cref="string"/> and <c>TEditor</c> to <see cref="string"/>.
/// </remarks>
public abstract class FullAuditedEntity : FullAuditedEntity<Guid, string, string?>, IFullAuditedEntity
{ }
