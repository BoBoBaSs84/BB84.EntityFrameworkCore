using BB84.EntityFrameworkCore.Models.Abstractions.Components;

namespace BB84.EntityFrameworkCore.Models.Abstractions;

/// <summary>
/// The interface for the enumerator models.
/// </summary>
/// <inheritdoc cref="IIdentityModel{TKey}"/>
/// <inheritdoc cref="IEnumerator"/>
/// <inheritdoc cref="ISoftDeletable"/>
public interface IEnumeratorModel<TKey> : IIdentityModel<TKey>, IEnumerator, ISoftDeletable where TKey : IEquatable<TKey>
{ }

/// <inheritdoc cref="IEnumeratorModel{TKey}"/>
/// <remarks>
/// The identity column is of type <see cref="int"/>.
/// </remarks>
public interface IEnumeratorModel : IEnumeratorModel<int>
{ }
