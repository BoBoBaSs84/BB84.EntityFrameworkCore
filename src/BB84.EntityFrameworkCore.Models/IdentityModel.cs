using BB84.EntityFrameworkCore.Models.Abstractions;

namespace BB84.EntityFrameworkCore.Models;

/// <summary>
/// The base implementation for the identity models.
/// </summary>
/// <inheritdoc cref="IIdentityModel{TKey}"/>
public abstract class IdentityModel<TKey> : IIdentityModel<TKey> where TKey : IEquatable<TKey>
{
	/// <inheritdoc/>
	public TKey Id { get; set; } = default!;

	/// <inheritdoc/>
	public byte[] Timestamp { get; } = default!;
}

/// <inheritdoc cref="IdentityModel{TKey}"/>
/// <inheritdoc cref="IIdentityModel"/>
public abstract class IdentityModel : IdentityModel<Guid>, IIdentityModel
{ }
