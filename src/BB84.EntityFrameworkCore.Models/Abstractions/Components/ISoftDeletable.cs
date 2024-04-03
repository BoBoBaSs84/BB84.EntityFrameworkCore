namespace BB84.EntityFrameworkCore.Models.Abstractions.Components;

/// <summary>
/// The soft deleteable interface.
/// </summary>
public interface ISoftDeletable
{
  /// <summary>
  /// Is the data row in a soft deleted state?
  /// </summary>
  bool IsDeleted { get; set; }
}
