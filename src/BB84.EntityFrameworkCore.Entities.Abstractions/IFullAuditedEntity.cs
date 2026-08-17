// Copyright: 2024 Robert Peter Meyer
// License: MIT
//
// This source code is licensed under the MIT license found in the
// LICENSE file in the root directory of this source tree.
using BB84.EntityFrameworkCore.Entities.Abstractions.Components;

namespace BB84.EntityFrameworkCore.Entities.Abstractions;

/// <summary>
/// Defines an entity contract that is fully audited, including information about its
/// creator, creation, last editor, last modification date and unique identity.
/// </summary>
/// <typeparam name="TKey">The type of the unique identifier for the entity.</typeparam>
/// <typeparam name="TCreator">The type representing the creator of the entity.</typeparam>
/// <typeparam name="TEditor">The type representing the last editor of the entity.</typeparam>
public interface IFullAuditedEntity<TKey, TCreator, TEditor> : IIdentityEntity<TKey>, IUserAudited<TCreator, TEditor>, ITimeAudited
	where TKey : IEquatable<TKey>
	where TCreator : notnull
{ }

/// <inheritdoc cref="IFullAuditedEntity{TKey, TCreator, TEditor}"/>
/// <remarks>
/// <typeparamref name="TKey"/> is supplied; <c>TCreator</c> defaults to <see cref="string"/>
/// and <c>TEditor</c> to <see cref="string"/>. For a custom creator or editor type use
/// <see cref="IFullAuditedEntity{TKey, TCreator, TEditor}"/> and name all three.
/// </remarks>
public interface IFullAuditedEntity<TKey> : IFullAuditedEntity<TKey, string, string?>, IUserAudited
	where TKey : IEquatable<TKey>
{ }

/// <inheritdoc cref="IFullAuditedEntity{TKey, TCreator, TEditor}"/>
/// <remarks>
/// Nothing is supplied; <c>TKey</c> defaults to <see cref="Guid"/>, <c>TCreator</c> to
/// <see cref="string"/> and <c>TEditor</c> to <see cref="string"/>.
/// </remarks>
public interface IFullAuditedEntity : IFullAuditedEntity<Guid, string, string?>, IIdentityEntity, IUserAudited
{ }
