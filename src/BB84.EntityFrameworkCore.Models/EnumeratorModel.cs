using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using BB84.EntityFrameworkCore.Models.Abstractions;

namespace BB84.EntityFrameworkCore.Models;

/// <summary>
/// The enumerator model class.
/// </summary>
/// <inheritdoc/>
public abstract class EnumeratorModel : IEnumeratorModel
{
  /// <inheritdoc/>
  [Column(Order = 1)]
  public int Id { get; } = default!;

  /// <inheritdoc/>
  [Column(Order = 2)]
  public byte[] Timestamp { get; } = default!;

  /// <inheritdoc/>
  [Column(Order = 3)]
  [MaxLength(50)]
  public string Name { get; set; } = default!;

  /// <inheritdoc/>
  [Column(Order = 4)]
  [MaxLength(250)]
  public string Description { get; set; } = default!;

  /// <inheritdoc/>
  [Column(Order = 5)]
  [DefaultValue(false)]
  public bool IsDeleted { get; set; } = default!;
}
