using System.ComponentModel.DataAnnotations.Schema;

using BB84.EntityFrameworkCore.Models.Abstractions;

namespace BB84.EntityFrameworkCore.Models;

/// <summary>
/// The composite model class.
/// </summary>
/// <inheritdoc cref=" ICompositeModel{TCreated, TModified}"/>
public abstract class CompositeModel<TCreated, TModified> : ICompositeModel<TCreated, TModified>
{
	/// <inheritdoc/>
	[Column(Order = 1)]
	public byte[] Timestamp { get; } = default!;

	/// <inheritdoc/>
	[Column(Order = 2)]
	public TCreated CreatedBy { get; set; } = default!;

	/// <inheritdoc/>
	[Column(Order = 3)]
	public TModified ModifiedBy { get; set; } = default!;
}

/// <inheritdoc/>
public abstract class CompositeModel : CompositeModel<string, string?>, ICompositeModel
{ }
