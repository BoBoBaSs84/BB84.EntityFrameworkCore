// Copyright: 2024 Robert Peter Meyer
// License: MIT
//
// This source code is licensed under the MIT license found in the
// LICENSE file in the root directory of this source tree.
using BB84.EntityFrameworkCore.Entities.Abstractions.Components;

namespace BB84.EntityFrameworkCore.Entities.Abstractions;

/// <summary>
/// The interface for the audited composite models.
/// </summary>
/// <inheritdoc cref="IConcurrency"/>
/// <inheritdoc cref="IUserAudited{TCreator, TEditor}"/>
public interface IAuditedCompositeEntity<TCreator, TEditor> : IConcurrency, IUserAudited<TCreator, TEditor>
{ }

/// <inheritdoc cref="IAuditedCompositeEntity{TCreator, TEditor}"/>
/// <remarks>
/// The user auditing columns are of type <see cref="string"/>.
/// </remarks>
public interface IAuditedCompositeEntity : IAuditedCompositeEntity<string, string?>
{ }
