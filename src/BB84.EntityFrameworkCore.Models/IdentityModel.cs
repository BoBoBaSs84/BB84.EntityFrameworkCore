﻿using BB84.EntityFrameworkCore.Models.Abstractions;

namespace BB84.EntityFrameworkCore.Models;

/// <summary>
/// The identity model class.
/// </summary>
/// <inheritdoc cref="IIdentityModel{TKey}"/>
public abstract class IdentityModel<TKey> : IIdentityModel<TKey> where TKey : IEquatable<TKey>
{
	/// <inheritdoc/>
	public TKey Id { get; set; } = default!;

	/// <inheritdoc/>
	public byte[] Timestamp { get; } = default!;
}

/// <inheritdoc/>
public abstract class IdentityModel : IdentityModel<Guid>, IIdentityModel
{ }
