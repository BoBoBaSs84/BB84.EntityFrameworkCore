using BB84.EntityFrameworkCore.Entities.Abstractions.Components;

namespace BB84.EntityFrameworkCore.Entities.Abstractions;

/// <summary>
/// The interface for the identity models.
/// </summary>
/// <inheritdoc cref="IIdentity{TKey}"/>
/// <inheritdoc cref="IConcurrency"/>
public interface IIdentityEntity<TKey> : IIdentity<TKey>, IConcurrency where TKey : IEquatable<TKey>
{ }

/// <inheritdoc cref="IIdentityEntity{TKey}"/>
/// <remarks>
/// The identity column is of type <see cref="Guid"/>.
/// </remarks>
public interface IIdentityEntity : IIdentityEntity<Guid>
{ }
