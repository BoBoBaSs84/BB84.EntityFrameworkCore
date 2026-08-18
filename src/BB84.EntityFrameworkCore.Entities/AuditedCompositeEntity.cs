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
/// editor, and a concurrency token.
/// </summary>
/// <typeparam name="TCreator">The type of the entity or user responsible for creating the entity.</typeparam>
/// <typeparam name="TEditor">The type of the entity or user responsible for editing the entity.</typeparam>
/// <typeparam name="TToken">The type of the concurrency token.</typeparam>
public abstract class AuditedCompositeEntity<TCreator, TEditor, TToken> : IAuditedCompositeEntity<TCreator, TEditor, TToken>
	where TCreator : notnull
{
	/// <inheritdoc/>
	public TToken Timestamp { get; set; } = default!;

	/// <inheritdoc/>
	public TCreator CreatedBy { get; set; } = default!;

	/// <inheritdoc/>
	public TEditor EditedBy { get; set; } = default!;
}

/// <inheritdoc cref="AuditedCompositeEntity{TCreator, TEditor, TToken}"/>
/// <remarks>
/// The creator and editor types are supplied; <c>TToken</c> defaults to a <see cref="byte"/> array.
/// For a custom token type use <see cref="AuditedCompositeEntity{TCreator, TEditor, TToken}"/>
/// and name all three.
/// </remarks>
public abstract class AuditedCompositeEntity<TCreator, TEditor> : AuditedCompositeEntity<TCreator, TEditor, byte[]>, IAuditedCompositeEntity<TCreator, TEditor>
	where TCreator : notnull
{
	/// <summary>
	/// Initializes a new instance of the <see cref="AuditedCompositeEntity{TCreator, TEditor}"/> class.
	/// </summary>
	/// <inheritdoc cref="IdentityEntity{TKey}()" path="/remarks"/>
	protected AuditedCompositeEntity()
		=> Timestamp = [];
}

/// <inheritdoc cref="AuditedCompositeEntity{TCreator, TEditor, TToken}"/>
/// <remarks>
/// The creator and editor types default to <see cref="string"/> and the token to a
/// <see cref="byte"/> array.
/// </remarks>
public abstract class AuditedCompositeEntity : AuditedCompositeEntity<string, string?>, IAuditedCompositeEntity;
