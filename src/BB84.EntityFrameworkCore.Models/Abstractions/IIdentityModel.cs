using BB84.EntityFrameworkCore.Models.Abstractions.Components;

namespace BB84.EntityFrameworkCore.Models.Abstractions;

/// <summary>
/// The model base interface.
/// </summary>
/// <inheritdoc cref="IIdentity{TKey}"/>
/// <inheritdoc cref="IAudited{TCreated, TModified}"/>
public interface IIdentityModel<TKey> : IIdentity<TKey>, IConcurrency where TKey : IEquatable<TKey>
{ }

/// <inheritdoc/>
/// <remarks>
/// The primary key is of type <see cref="Guid"/>.
/// </remarks>
public interface IIdentityModel : IIdentityModel<Guid>
{ }
