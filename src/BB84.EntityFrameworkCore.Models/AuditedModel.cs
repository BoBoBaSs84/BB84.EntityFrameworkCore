using System.ComponentModel.DataAnnotations.Schema;

using BB84.EntityFrameworkCore.Models.Abstractions;

namespace BB84.EntityFrameworkCore.Models;

/// <summary>
/// The audited model class.
/// </summary>
/// <inheritdoc cref="IAuditedModel{TKey, TCreated, TModified}"/>
public abstract class AuditedModel<TKey, TCreated, TModified> : IAuditedModel<TKey, TCreated, TModified> where TKey : IEquatable<TKey>
{
	/// <inheritdoc/>
	[Column(Order = 1)]
	public TKey Id { get; } = default!;

	/// <inheritdoc/>
	[Column(Order = 2)]
	public byte[] Timestamp { get; } = default!;

	/// <inheritdoc/>
	[Column(Order = 3)]
	public TCreated CreatedBy { get; set; } = default!;

	/// <inheritdoc/>
	[Column(Order = 4)]
	public TModified ModifiedBy { get; set; } = default!;
}

/// <inheritdoc/>
public abstract class AuditedModel : AuditedModel<Guid, string, string?>, IAuditedModel
{ }
