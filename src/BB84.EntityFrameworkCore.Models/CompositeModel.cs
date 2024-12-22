using BB84.EntityFrameworkCore.Models.Abstractions;

namespace BB84.EntityFrameworkCore.Models;

/// <summary>
/// The base implementation for the composite models.
/// </summary>
/// <inheritdoc cref=" ICompositeModel"/>
public abstract class CompositeModel : ICompositeModel
{
	/// <inheritdoc/>
	public byte[] Timestamp { get; } = default!;
}
