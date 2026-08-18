// Copyright: 2024 Robert Peter Meyer
// License: MIT
//
// This source code is licensed under the MIT license found in the
// LICENSE file in the root directory of this source tree.
using BB84.EntityFrameworkCore.Entities.Abstractions;

namespace BB84.EntityFrameworkCore.Entities;

/// <summary>
/// This abstract class provides a base implementation for entities that are
/// composed of multiple related components.
/// </summary>
/// <typeparam name="TToken">The type of the concurrency token.</typeparam>
public abstract class CompositeEntity<TToken> : ICompositeEntity<TToken>
{
	/// <inheritdoc/>
	public TToken Timestamp { get; set; } = default!;
}

/// <inheritdoc cref="CompositeEntity{TToken}"/>
/// <remarks>
/// The token is a <see cref="byte"/> array. For a custom token type use
/// <see cref="CompositeEntity{TToken}"/> and name it.
/// </remarks>
public abstract class CompositeEntity : CompositeEntity<byte[]>, ICompositeEntity
{
	/// <summary>
	/// Initializes a new instance of the <see cref="CompositeEntity"/> class.
	/// </summary>
	/// <inheritdoc cref="IdentityEntity{TKey}()" path="/remarks"/>
	protected CompositeEntity()
		=> Timestamp = [];
}
