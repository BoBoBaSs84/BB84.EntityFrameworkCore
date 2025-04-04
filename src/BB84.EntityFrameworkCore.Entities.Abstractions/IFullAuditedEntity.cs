// Copyright: 2024 Robert Peter Meyer
// License: MIT
//
// This source code is licensed under the MIT license found in the
// LICENSE file in the root directory of this source tree.
using BB84.EntityFrameworkCore.Entities.Abstractions.Components;

namespace BB84.EntityFrameworkCore.Entities.Abstractions;

/// <summary>
/// The interface for full audited entities.
/// </summary>
/// <inheritdoc cref="IIdentity{TKey}"/>
/// <inheritdoc cref="IUserAudited{TCreator, TEdited}"/>
/// <inheritdoc cref="ITimeAudited"/>
public interface IFullAuditedEntity<TKey, TCreator, TEdited> : IIdentityEntity<TKey>, IUserAudited<TCreator, TEdited>, ITimeAudited where TKey : IEquatable<TKey>
{ }

/// <inheritdoc cref="IFullAuditedEntity{TKey, TCreator, TEdited}"/>
/// <remarks>
/// The user auditing columns are of type <see cref="string"/>.
/// </remarks>
public interface IFullAuditedEntity<TKey> : IFullAuditedEntity<TKey, string, string?>, IUserAudited where TKey : IEquatable<TKey>
{ }

/// <inheritdoc cref="IFullAuditedEntity{TKey, TCreator, TEdited}"/>
/// <remarks>
/// The identity column is of type <see cref="Guid"/>.
/// </remarks>
public interface IFullAuditedEntity<TCreator, TEdited> : IFullAuditedEntity<Guid, TCreator, TEdited>, IIdentityEntity
{ }

/// <inheritdoc cref="IFullAuditedEntity{TKey, TCreator, TEdited}"/>
/// <remarks>
/// The identity column is of type <see cref="Guid"/>.
/// The user auditing columns are of type <see cref="string"/>.
/// </remarks>
public interface IFullAuditedEntity : IFullAuditedEntity<Guid, string, string?>, IIdentityEntity, IUserAudited
{ }
