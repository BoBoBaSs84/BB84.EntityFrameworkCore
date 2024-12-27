using BB84.EntityFrameworkCore.Entities.Abstractions.Components;

namespace BB84.EntityFrameworkCore.Entities.Abstractions;

/// <summary>
/// The interface for the enumerator models.
/// </summary>
/// <inheritdoc cref="IIdentityEntity{TKey}"/>
/// <inheritdoc cref="IEnumerator"/>
/// <inheritdoc cref="ISoftDeletable"/>
public interface IEnumeratorEntity<TKey> : IIdentityEntity<TKey>, IEnumerator, ISoftDeletable where TKey : IEquatable<TKey>
{ }

/// <inheritdoc cref="IEnumeratorEntity{TKey}"/>
/// <remarks>
/// The identity column is of type <see cref="int"/>.
/// </remarks>
public interface IEnumeratorEntity : IEnumeratorEntity<int>
{ }
