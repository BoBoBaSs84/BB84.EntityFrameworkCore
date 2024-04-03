namespace BB84.EntityFrameworkCore.Models.Abstractions.Components;

/// <summary>
/// The audited interface.
/// </summary>
/// <typeparam name="TCreated">The type of the created by column.</typeparam>
/// <typeparam name="TModified">The type of the modified by column.</typeparam>
public interface IAudited<TCreated, TModified>
{
  /// <summary>
  /// The creator of the data row.
  /// </summary>
  TCreated CreatedBy { get; set; }

  /// <summary>
  /// The last modifier of the data row.
  /// </summary>
  TModified ModifiedBy { get; set; }
}

/// <inheritdoc/>
/// <remarks>
/// The auditing key is of type <see cref="string"/>.
/// </remarks>
public interface IAudited : IAudited<string, string?>
{ }
