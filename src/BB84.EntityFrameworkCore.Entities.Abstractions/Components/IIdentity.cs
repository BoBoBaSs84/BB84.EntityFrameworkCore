﻿namespace BB84.EntityFrameworkCore.Entities.Abstractions.Components;

/// <summary>
/// The interface for identity based components.
/// </summary>
/// <typeparam name="TKey">The type of the primary key.</typeparam>
public interface IIdentity<TKey> where TKey : IEquatable<TKey>
{
	/// <summary>
	/// The primary key of the database table.
	/// </summary>
	TKey Id { get; set; }
}

/// <inheritdoc cref="IIdentity{TKey}"/>
/// <remarks>
/// The primary key is of type <see cref="Guid"/>.
/// </remarks>
public interface IIdentity : IIdentity<Guid>
{ }
