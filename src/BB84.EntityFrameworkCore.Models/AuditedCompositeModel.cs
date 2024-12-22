using BB84.EntityFrameworkCore.Models.Abstractions;

namespace BB84.EntityFrameworkCore.Models;

/// <summary>
/// The base implementation for the audited composite models.
/// </summary>
/// <inheritdoc cref="IAuditedCompositeModel{TCreator, TEdited}"/>
public abstract class AuditedCompositeModel<TCreator, TEdited> : IAuditedCompositeModel<TCreator, TEdited>
{
	/// <inheritdoc/>
	public byte[] Timestamp { get; } = default!;

	/// <inheritdoc/>
	public TCreator Creator { get; set; } = default!;

	/// <inheritdoc/>
	public TEdited Editor { get; set; } = default!;
}

/// <inheritdoc cref="AuditedCompositeModel{TCreator, TEdited}"/>
/// <inheritdoc cref="IAuditedCompositeModel"/>
public abstract class AuditedCompositeModel : AuditedCompositeModel<string, string?>, IAuditedCompositeModel
{ }
