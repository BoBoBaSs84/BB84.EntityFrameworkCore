using BB84.EntityFrameworkCore.Models.Abstractions;

namespace BB84.EntityFrameworkCore.Models;

/// <summary>
/// The base implementation for the enumerator models.
/// </summary>
/// <inheritdoc cref="IEnumeratorModel{TKey}"/>
public abstract class EnumeratorModel<TKey> : IEnumeratorModel<TKey> where TKey : IEquatable<TKey>
{
	/// <inheritdoc/>
	public TKey Id { get; set; } = default!;

	/// <inheritdoc/>
	public byte[] Timestamp { get; } = default!;

	/// <inheritdoc/>
	public required string Name { get; set; }

	/// <inheritdoc/>
	public string? Description { get; set; }

	/// <inheritdoc/>
	public bool IsDeleted { get; set; }
}

/// <inheritdoc cref="EnumeratorModel{TKey}"/>
/// <inheritdoc cref="IEnumeratorModel"/>
public abstract class EnumeratorModel : EnumeratorModel<int>, IEnumeratorModel
{ }
