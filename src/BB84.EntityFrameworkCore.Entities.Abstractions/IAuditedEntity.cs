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
/// <typeparam name="TEditor">The type representing the user or entity that last modified this entity.</typeparam>
public interface IAuditedEntity<TKey, TCreator, TEditor> : IIdentityEntity<TKey>, IUserAudited<TCreator, TEditor>
	where TKey : IEquatable<TKey>
	where TCreator : notnull
{ }

/// <inheritdoc cref="IAuditedEntity{TKey, TCreator, TEditor}"/>
/// <remarks>
/// <typeparamref name="TKey"/> is supplied; <c>TCreator</c> defaults to <see cref="string"/>
/// and <c>TEditor</c> to <see cref="string"/>. For a custom creator or editor type use
/// <see cref="IAuditedEntity{TKey, TCreator, TEditor}"/> and name all three.
/// </remarks>
public interface IAuditedEntity<TKey> : IAuditedEntity<TKey, string, string?>, IUserAudited
	where TKey : IEquatable<TKey>
{ }

/// <inheritdoc cref="IAuditedEntity{TKey, TCreator, TEditor}"/>
/// <remarks>
/// Nothing is supplied; <c>TKey</c> defaults to <see cref="Guid"/>, <c>TCreator</c> to
/// <see cref="string"/> and <c>TEditor</c> to <see cref="string"/>.
/// </remarks>
public interface IAuditedEntity : IAuditedEntity<Guid, string, string?>, IIdentityEntity, IUserAudited
{ }
