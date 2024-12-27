using BB84.EntityFrameworkCore.Entities.Abstractions;

namespace BB84.EntityFrameworkCore.Entities;

/// <summary>
/// The base implementation for the audited models.
/// </summary>
/// <inheritdoc cref="IAuditedEntity{TKey, TCreator, TEdited}"/>
public abstract class AuditedEntity<TKey, TCreator, TEdited> : IdentityEntity<TKey>, IAuditedEntity<TKey, TCreator, TEdited> where TKey : IEquatable<TKey>
{
	/// <inheritdoc/>
	public TCreator Creator { get; set; } = default!;

	/// <inheritdoc/>
	public TEdited Editor { get; set; } = default!;
}

/// <inheritdoc cref="AuditedEntity{TKey, TCreator, TEdited}"/>
/// <inheritdoc cref="IAuditedEntity{TKey}"/>
public abstract class AuditedEntity<TKey> : AuditedEntity<TKey, string, string?>, IAuditedEntity<TKey> where TKey : IEquatable<TKey>
{ }

/// <inheritdoc cref="AuditedEntity{TKey, TCreator, TEdited}"/>
/// <inheritdoc cref="IAuditedEntity{TCreator, TEdited}"/>
public abstract class AuditedEntity<TCreator, TEdited> : AuditedEntity<Guid, TCreator, TEdited>, IAuditedEntity<TCreator, TEdited>
{ }

/// <inheritdoc cref="AuditedEntity{TKey, TCreator, TEdited}"/>
/// <inheritdoc cref="IAuditedEntity"/>
public abstract class AuditedEntity : AuditedEntity<Guid, string, string?>, IAuditedEntity
{ }
