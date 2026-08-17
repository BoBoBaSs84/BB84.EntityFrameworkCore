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
/// <typeparam name="TEditor">The type representing the last editor of the entity.</typeparam>
public abstract class AuditedEntity<TKey, TCreator, TEditor> : IdentityEntity<TKey>, IAuditedEntity<TKey, TCreator, TEditor>
	where TKey : IEquatable<TKey>
	where TCreator : notnull
{
	/// <inheritdoc/>
	public TCreator CreatedBy { get; set; } = default!;

	/// <inheritdoc/>
	public TEditor EditedBy { get; set; } = default!;
}

/// <inheritdoc cref="AuditedEntity{TKey, TCreator, TEditor}"/>
/// <remarks>
/// <typeparamref name="TKey"/> is supplied; <c>TCreator</c> defaults to <see cref="string"/>
/// and <c>TEditor</c> to <see cref="string"/>. For a custom creator or editor type use
/// <see cref="AuditedEntity{TKey, TCreator, TEditor}"/> and name all three.
/// </remarks>
public abstract class AuditedEntity<TKey> : AuditedEntity<TKey, string, string?>, IAuditedEntity<TKey>
	where TKey : IEquatable<TKey>
{ }

/// <inheritdoc cref="AuditedEntity{TKey, TCreator, TEditor}"/>
/// <remarks>
/// Nothing is supplied; <c>TKey</c> defaults to <see cref="Guid"/>, <c>TCreator</c> to
/// <see cref="string"/> and <c>TEditor</c> to <see cref="string"/>.
/// </remarks>
public abstract class AuditedEntity : AuditedEntity<Guid, string, string?>, IAuditedEntity
{ }
