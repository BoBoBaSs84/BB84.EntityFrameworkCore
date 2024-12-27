using BB84.EntityFrameworkCore.Entities.Abstractions;

namespace BB84.EntityFrameworkCore.Entities;

/// <summary>
/// The base implementation for the audited composite models.
/// </summary>
/// <inheritdoc cref="IAuditedCompositeEntity{TCreator, TEdited}"/>
public abstract class AuditedCompositeEntity<TCreator, TEdited> : IAuditedCompositeEntity<TCreator, TEdited>
{
	/// <inheritdoc/>
	public byte[] Timestamp { get; } = default!;

	/// <inheritdoc/>
	public TCreator Creator { get; set; } = default!;

	/// <inheritdoc/>
	public TEdited Editor { get; set; } = default!;
}

/// <inheritdoc cref="AuditedCompositeEntity{TCreator, TEdited}"/>
/// <inheritdoc cref="IAuditedCompositeEntity"/>
public abstract class AuditedCompositeEntity : AuditedCompositeEntity<string, string?>, IAuditedCompositeEntity
{ }
