using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

using BB84.EntityFrameworkCore.Models.Abstractions;

namespace BB84.EntityFrameworkCore.Models;

/// <summary>
/// The enumerator model class.
/// </summary>
/// <inheritdoc/>
public abstract class EnumeratorModel : IEnumeratorModel
{
	/// <inheritdoc/>
	public int Id { get; set; } = default!;

	/// <inheritdoc/>
	public byte[] Timestamp { get; } = default!;

	/// <inheritdoc/>
	[MaxLength(50)]
	public string Name { get; set; } = default!;

	/// <inheritdoc/>
	[MaxLength(250)]
	public string Description { get; set; } = default!;

	/// <inheritdoc/>
	[DefaultValue(false)]
	public bool IsDeleted { get; set; } = default!;
}
