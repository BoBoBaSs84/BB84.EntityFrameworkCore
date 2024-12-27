namespace BB84.EntityFrameworkCore.Entities.Abstractions.Components;

/// <summary>
/// The interface for soft deleteable components.
/// </summary>
public interface ISoftDeletable
{
	/// <summary>
	/// Indicates if the data row in a soft deleted state.
	/// </summary>
	bool IsDeleted { get; set; }
}
