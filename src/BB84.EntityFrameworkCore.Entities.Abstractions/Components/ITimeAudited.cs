namespace BB84.EntityFrameworkCore.Entities.Abstractions.Components;

/// <summary>
/// The interface for time audited components.
/// </summary>
public interface ITimeAudited
{
	/// <summary>
	/// The initial creation time of the data row.
	/// </summary>
	DateTime Created { get; set; }

	/// <summary>
	/// The last editing time of the data row.
	/// </summary>
	DateTime? Edited { get; set; }
}
