using BB84.EntityFrameworkCore.Models.Abstractions;

namespace BB84.EntityFrameworkCore.Models;

/// <summary>
/// The composite model class.
/// </summary>
/// <inheritdoc cref=" ICompositeModel"/>
public abstract class CompositeModel : ICompositeModel
{
	/// <inheritdoc/>
	public byte[] Timestamp { get; } = default!;
}
