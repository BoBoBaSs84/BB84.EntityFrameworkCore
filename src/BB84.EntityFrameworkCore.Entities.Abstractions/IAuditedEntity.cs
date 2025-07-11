// Copyright: 2024 Robert Peter Meyer
// License: MIT
//
// This source code is licensed under the MIT license found in the
// LICENSE file in the root directory of this source tree.
using BB84.EntityFrameworkCore.Entities.Abstractions.Components;

namespace BB84.EntityFrameworkCore.Entities.Abstractions;

/// <summary>
/// Represents an entity contract that is audited with information about its creation
/// and last modification.
/// </summary>
/// <typeparam name="TKey">The type of the unique identifier for the entity.</typeparam>
/// <typeparam name="TCreator">The type representing the user or entity that created this entity.</typeparam>
/// <typeparam name="TEdited">The type representing the user or entity that last modified this entity.</typeparam>
public interface IAuditedEntity<TKey, TCreator, TEdited> : IIdentityEntity<TKey>, IUserAudited<TCreator, TEdited>
	where TKey : IEquatable<TKey>
	where TCreator : notnull
{ }

/// <inheritdoc cref="IAuditedEntity{TKey, TCreator, TEdited}"/>
/// <remarks>
/// The creator and editor types default to <see cref="string"/>.
/// </remarks>
public interface IAuditedEntity<TKey> : IAuditedEntity<TKey, string, string?>, IUserAudited
	where TKey : IEquatable<TKey>
{ }

/// <inheritdoc cref="IAuditedEntity{TKey, TCreator, TEdited}"/>
/// <remarks>
/// The unique identifier type defaults to <see cref="Guid"/>.
/// </remarks>
public interface IAuditedEntity<TCreator, TEdited> : IAuditedEntity<Guid, TCreator, TEdited>, IIdentityEntity
	where TCreator : notnull
{ }

/// <inheritdoc cref="IAuditedEntity{TKey, TCreator, TEdited}"/>
/// <remarks>
/// The unique identifier type defaults to <see cref="Guid"/> and the creator and editor
/// types default to <see cref="string"/>.
/// </remarks>
public interface IAuditedEntity : IAuditedEntity<Guid, string, string?>, IIdentityEntity, IUserAudited
{ }
