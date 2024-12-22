using BB84.EntityFrameworkCore.Models.Abstractions.Components;

namespace BB84.EntityFrameworkCore.Models.Abstractions;

/// <summary>
/// The interface for the identity models.
/// </summary>
/// <inheritdoc cref="IIdentity{TKey}"/>
/// <inheritdoc cref="IConcurrency"/>
public interface IIdentityModel<TKey> : IIdentity<TKey>, IConcurrency where TKey : IEquatable<TKey>
{ }

/// <inheritdoc cref="IIdentityModel{TKey}"/>
/// <remarks>
/// The identity column is of type <see cref="Guid"/>.
/// </remarks>
public interface IIdentityModel : IIdentityModel<Guid>
{ }
