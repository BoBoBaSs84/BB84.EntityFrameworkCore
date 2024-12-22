using BB84.EntityFrameworkCore.Models.Abstractions;

namespace BB84.EntityFrameworkCore.Models;

/// <summary>
/// The base implementation for the full audited models.
/// </summary>
/// <inheritdoc cref="IFullAuditedModel{TKey, TCreator, TEdited}"/>
public abstract class FullAuditedModel<TKey, TCreator, TEdited> : IdentityModel<TKey>, IFullAuditedModel<TKey, TCreator, TEdited> where TKey : IEquatable<TKey>
{
	/// <inheritdoc/>
	public TCreator Creator { get; set; } = default!;
	/// <inheritdoc/>
	public DateTime Created { get; set; } = default!;
	/// <inheritdoc/>
	public TEdited Editor { get; set; } = default!;
	/// <inheritdoc/>
	public DateTime? Edited { get; set; } = default!;
}

/// <inheritdoc cref="FullAuditedModel{TKey, TCreator, TEdited}"/>
/// <inheritdoc cref="IFullAuditedModel{TKey}"/>
public abstract class FullAuditedModel<TKey> : FullAuditedModel<TKey, string, string?>, IFullAuditedModel<TKey> where TKey : IEquatable<TKey>
{ }


/// <inheritdoc cref="FullAuditedModel{TKey, TCreator, TEdited}"/>
/// <inheritdoc cref="IFullAuditedModel{TCreator, TEdited}"/>
public abstract class FullAuditedModel<TCreator, TEdited> : FullAuditedModel<Guid, TCreator, TEdited>, IFullAuditedModel<TCreator, TEdited>
{ }

/// <inheritdoc cref="FullAuditedModel{TKey, TCreator, TEdited}"/>
/// <inheritdoc cref="IFullAuditedModel"/>
public abstract class FullAuditedModel : FullAuditedModel<Guid, string, string?>, IFullAuditedModel
{ }
