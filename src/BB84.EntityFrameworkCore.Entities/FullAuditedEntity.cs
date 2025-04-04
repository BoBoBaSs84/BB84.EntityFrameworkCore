// Copyright: 2024 Robert Peter Meyer
// License: MIT
//
// This source code is licensed under the MIT license found in the
// LICENSE file in the root directory of this source tree.
using BB84.EntityFrameworkCore.Entities.Abstractions;

namespace BB84.EntityFrameworkCore.Entities;

/// <summary>
/// The base implementation for the full audited models.
/// </summary>
/// <inheritdoc cref="IFullAuditedEntity{TKey, TCreator, TEdited}"/>
public abstract class FullAuditedEntity<TKey, TCreator, TEdited> : IdentityEntity<TKey>, IFullAuditedEntity<TKey, TCreator, TEdited> where TKey : IEquatable<TKey>
{
	/// <inheritdoc/>
	public TCreator Creator { get; set; } = default!;
	/// <inheritdoc/>
	public DateTime Created { get; set; } = default!;
	/// <inheritdoc/>
	public TEdited Editor { get; set; } = default!;
	/// <inheritdoc/>
	public DateTime? Edited { get; set; } = default!;
}

/// <inheritdoc cref="FullAuditedEntity{TKey, TCreator, TEdited}"/>
/// <inheritdoc cref="IFullAuditedEntity{TKey}"/>
public abstract class FullAuditedEntity<TKey> : FullAuditedEntity<TKey, string, string?>, IFullAuditedEntity<TKey> where TKey : IEquatable<TKey>
{ }


/// <inheritdoc cref="FullAuditedEntity{TKey, TCreator, TEdited}"/>
/// <inheritdoc cref="IFullAuditedEntity{TCreator, TEdited}"/>
public abstract class FullAuditedEntity<TCreator, TEdited> : FullAuditedEntity<Guid, TCreator, TEdited>, IFullAuditedEntity<TCreator, TEdited>
{ }

/// <inheritdoc cref="FullAuditedEntity{TKey, TCreator, TEdited}"/>
/// <inheritdoc cref="IFullAuditedEntity"/>
public abstract class FullAuditedEntity : FullAuditedEntity<Guid, string, string?>, IFullAuditedEntity
{ }
