using BB84.EntityFrameworkCore.Entities.Abstractions;

namespace BB84.EntityFrameworkCore.Entities;

/// <summary>
/// The base implementation for the composite models.
/// </summary>
/// <inheritdoc cref=" ICompositeEntity"/>
public abstract class CompositeEntity : ICompositeEntity
{
	/// <inheritdoc/>
	public byte[] Timestamp { get; } = default!;
}
