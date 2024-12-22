using BB84.EntityFrameworkCore.Models.Abstractions;

namespace BB84.EntityFrameworkCore.Models;

/// <summary>
/// The base implementation for the audited models.
/// </summary>
/// <inheritdoc cref="IAuditedModel{TKey, TCreator, TEdited}"/>
public abstract class AuditedModel<TKey, TCreator, TEdited> : IdentityModel<TKey>, IAuditedModel<TKey, TCreator, TEdited> where TKey : IEquatable<TKey>
{
	/// <inheritdoc/>
	public TCreator Creator { get; set; } = default!;

	/// <inheritdoc/>
	public TEdited Editor { get; set; } = default!;
}

/// <inheritdoc cref="AuditedModel{TKey, TCreator, TEdited}"/>
/// <inheritdoc cref="IAuditedModel{TKey}"/>
public abstract class AuditedModel<TKey> : AuditedModel<TKey, string, string?>, IAuditedModel<TKey> where TKey : IEquatable<TKey>
{ }

/// <inheritdoc cref="AuditedModel{TKey, TCreator, TEdited}"/>
/// <inheritdoc cref="IAuditedModel{TCreator, TEdited}"/>
public abstract class AuditedModel<TCreator, TEdited> : AuditedModel<Guid, TCreator, TEdited>, IAuditedModel<TCreator, TEdited>
{ }

/// <inheritdoc cref="AuditedModel{TKey, TCreator, TEdited}"/>
/// <inheritdoc cref="IAuditedModel"/>
public abstract class AuditedModel : AuditedModel<Guid, string, string?>, IAuditedModel
{ }
