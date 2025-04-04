// Copyright: 2024 Robert Peter Meyer
// License: MIT
//
// This source code is licensed under the MIT license found in the
// LICENSE file in the root directory of this source tree.
using BB84.EntityFrameworkCore.Entities.Abstractions.Components;

namespace BB84.EntityFrameworkCore.Entities.Abstractions;

/// <summary>
/// The interface for the audited models.
/// </summary>
/// <inheritdoc cref="IIdentity{TKey}"/>
/// <inheritdoc cref="IUserAudited{TCreator, TEdited}"/>
public interface IAuditedEntity<TKey, TCreator, TEdited> : IIdentityEntity<TKey>, IUserAudited<TCreator, TEdited> where TKey : IEquatable<TKey>
{ }

/// <inheritdoc cref="IAuditedEntity{TKey, TCreator, TEdited}"/>
/// <remarks>
/// The user auditing columns are of type <see cref="string"/>.
/// </remarks>
public interface IAuditedEntity<TKey> : IAuditedEntity<TKey, string, string?>, IUserAudited where TKey : IEquatable<TKey>
{ }

/// <inheritdoc cref="IAuditedEntity{TKey, TCreator, TEdited}"/>
/// <remarks>
/// The identity column is of type <see cref="Guid"/>.
/// </remarks>
public interface IAuditedEntity<TCreator, TEdited> : IAuditedEntity<Guid, TCreator, TEdited>, IIdentityEntity
{ }

/// <inheritdoc cref="IAuditedEntity{TKey, TCreator, TEdited}"/>
/// <remarks>
/// The identity column is of type <see cref="Guid"/>.
/// The user auditing columns are of type <see cref="string"/>.
/// </remarks>
public interface IAuditedEntity : IAuditedEntity<Guid, string, string?>, IIdentityEntity, IUserAudited
{ }
