using BB84.EntityFrameworkCore.Models.Abstractions;

namespace BB84.EntityFrameworkCore.Models;

/// <summary>
/// The audited composite model class.
/// </summary>
/// <inheritdoc cref="IAuditedCompositeModel{TCreated, TModified}"/>
public abstract class AuditedCompositeModel<TCreated, TModified> : IAuditedCompositeModel<TCreated, TModified>
{
	/// <inheritdoc/>
	public byte[] Timestamp { get; } = default!;

	/// <inheritdoc/>
	public TCreated CreatedBy { get; set; } = default!;

	/// <inheritdoc/>
	public TModified ModifiedBy { get; set; } = default!;
}

/// <inheritdoc/>
public abstract class AuditedCompositeModel : AuditedCompositeModel<string, string?>, IAuditedCompositeModel
{ }
