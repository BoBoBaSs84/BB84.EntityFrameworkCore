using BB84.EntityFrameworkCore.Models.Abstractions;

namespace BB84.EntityFrameworkCore.Models;

/// <summary>
/// The composite model class.
/// </summary>
/// <inheritdoc cref=" ICompositeModel{TCreated, TModified}"/>
public abstract class CompositeModel<TCreated, TModified> : ICompositeModel<TCreated, TModified>
{
	/// <inheritdoc/>
	public byte[] Timestamp { get; } = default!;

	/// <inheritdoc/>
	public TCreated CreatedBy { get; set; } = default!;

	/// <inheritdoc/>
	public TModified ModifiedBy { get; set; } = default!;
}

/// <inheritdoc/>
public abstract class CompositeModel : CompositeModel<string, string?>, ICompositeModel
{ }
