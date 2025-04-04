// Copyright: 2024 Robert Peter Meyer
// License: MIT
//
// This source code is licensed under the MIT license found in the
// LICENSE file in the root directory of this source tree.
using BB84.EntityFrameworkCore.Entities.Abstractions;

namespace BB84.EntityFrameworkCore.Entities;

/// <summary>
/// The base implementation for the audited composite models.
/// </summary>
/// <inheritdoc cref="IAuditedCompositeEntity{TCreator, TEdited}"/>
public abstract class AuditedCompositeEntity<TCreator, TEdited> : IAuditedCompositeEntity<TCreator, TEdited>
{
	/// <inheritdoc/>
	public byte[] Timestamp { get; } = default!;

	/// <inheritdoc/>
	public TCreator Creator { get; set; } = default!;

	/// <inheritdoc/>
	public TEdited Editor { get; set; } = default!;
}

/// <inheritdoc cref="AuditedCompositeEntity{TCreator, TEdited}"/>
/// <inheritdoc cref="IAuditedCompositeEntity"/>
public abstract class AuditedCompositeEntity : AuditedCompositeEntity<string, string?>, IAuditedCompositeEntity
{ }
