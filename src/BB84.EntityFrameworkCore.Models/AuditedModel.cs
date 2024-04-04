using BB84.EntityFrameworkCore.Models.Abstractions;

namespace BB84.EntityFrameworkCore.Models;

/// <summary>
/// The audited model class.
/// </summary>
/// <inheritdoc cref="IAuditedModel{TKey, TCreated, TModified}"/>
public abstract class AuditedModel<TKey, TCreated, TModified> : IdentityModel<TKey>, IAuditedModel<TKey, TCreated, TModified> where TKey : IEquatable<TKey>
{
	/// <inheritdoc/>
	public TCreated CreatedBy { get; set; } = default!;

	/// <inheritdoc/>
	public TModified ModifiedBy { get; set; } = default!;
}

/// <inheritdoc/>
public abstract class AuditedModel : AuditedModel<Guid, string, string?>, IAuditedModel
{ }
